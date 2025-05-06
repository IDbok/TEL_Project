using Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.DAL.Entities;
using TEL_ProjectBus.WebAPI.Consumers;
using TEL_ProjectBus.WebAPI.Messages.Queries;

// ��������� Request/Response ������� - ������� � ������ ����������
// Controller: ������� ����� ������� (REST API).
// Consumer: ���������� ������� (������� �������).
// RequestClient: ���������� ������ � �������.
// MassTransit: ���������� ��� ������ � ����� ���������.

// REST Controller -> IRequestClient<GetBudgetByIdQuery> -> MassTransit Bus -> RabbitMQ -> GetBudgetByIdConsumer -> ���������� GetBudgetByIdResponse


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
	var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
	//options.UseNpgsql(connectionString);
	options.UseSqlServer(connectionString, sqlOptions =>
	{
		sqlOptions.EnableRetryOnFailure(); // ��������� ����
	})
	.EnableSensitiveDataLogging() // ��������� ��������
	.EnableDetailedErrors();     // ���� ��� �������

	try
	{
		using var conn = new SqlConnection(connectionString);
		//conn.Open();
		// Console.WriteLine("������� ������������!");
	}
	catch (Exception ex)
	{
		Console.WriteLine($"������ �����������: {ex.Message}");
	}
});

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowLocalhost", builder =>
	{
		builder.WithOrigins("http://localhost:5000", "http://localhost:4082")
			   .AllowAnyHeader()
			   .AllowAnyMethod()
			   .AllowCredentials();
	});
});


var useInMemory = builder.Configuration.GetValue<bool>("UseInMemoryTransport");

// Add services to the container.
builder.Logging.AddConsole().SetMinimumLevel(LogLevel.Debug);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// ��������� Swagger � ��������� XML-������������
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "TEL_ProjectBus",
		Version = "v1"
	});

	// ��������� ������� � �������� � ������������
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	c.IncludeXmlComments(xmlPath);
});

// ����������� MassTransit � RabbitMQ
builder.Services.AddMassTransit(x =>
{
	x.SetKebabCaseEndpointNameFormatter(); // ���������� kebab-case ��� ���������� ��������

	// ������������ ����������� ���������
	x.AddConsumer<GetBudgetByIdConsumer>();
	x.AddConsumer<GetProjectsConsumer>();
	//x.AddConsumer<CurrentTimeConsumer>();

	// ����������� RequestClient
	x.AddRequestClient<GetBudgetByIdQuery>();
	x.AddRequestClient<GetProjectsQuery>();

	if (useInMemory)
		x.UsingInMemory((context, cfg) =>
		{
			cfg.ReceiveEndpoint("get-budget-by-id-queue", e =>
			{
				e.ConfigureConsumer<GetBudgetByIdConsumer>(context);
			});

			cfg.ConfigureEndpoints(context);
		});
	else
		x.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host("rabbitmq://localhost", h =>
		{
			h.Username("guest");
			h.Password("guest");
		});


		cfg.ReceiveEndpoint("get-budget-by-id-queue", e =>
		{
			e.ConfigureConsumeTopology = false;
			e.Bind("TEL_ProjectBus.Messages.Queries:GetBudgetByIdQuery", x =>
			{
				x.ExchangeType = "fanout";
				// routingKey �� ����� ��� fanout
			});
			e.ConfigureConsumer<GetBudgetByIdConsumer>(context);
		});

		cfg.ConfigureEndpoints(context);
	});
});

// ��������� Identity-������
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
	// ��������� ���������� � ������
	options.Password.RequireDigit = true;
	options.Password.RequireUppercase = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders(); // ���������� ������ ��� ������ ������, ������������� email � �.�.


// ��������� Middleware ��� �������� JWT
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer("Jwt", options =>
{
	// ����������� JWT-��������������
	var jwtSettings = builder.Configuration.GetSection("JwtSettings");
	var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));

	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = jwtSettings["Issuer"],
		ValidAudience = jwtSettings["Audience"],
		LifetimeValidator = (notBefore, expires, token, parameters) => expires > DateTime.UtcNow, // todo: ���������
		IssuerSigningKey = key
	};
})
// �������, �.�. ����� �������� ����� iis 
//.AddNegotiate("AD", options =>
//{
//	options.PersistKerberosCredentials = false;
//	options.PersistNtlmCredentials = false;
//	// options.Events = new NegotiateEvents { ... }; // ����� �������� ��������
//})
;

// �������� �����������
builder.Services.AddAuthorizationBuilder()
	//.AddPolicy("ADPolicy", policy =>
	//		policy.AddAuthenticationSchemes("AD").RequireAuthenticatedUser())
	.AddPolicy("AdminOnly", policy =>
			policy.RequireRole("Admin"))
;

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();// �������������� HTTP ? HTTPS

app.UseCors("AllowLocalhost");


app.UseAuthentication(); // ��������� JWT � ������ ������� (������ � �������������� �����, ��������� User � HttpContext)
app.UseAuthorization();  // ���������, �������� �� ������ � endpoint'� (�������, ���� �� � User ����� �� ���������� ������� (��������, ���� Admin))

app.MapControllers(); // ���������� ������������� ������������

try
{
	using (var scope = app.Services.CreateScope())
	{
		var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
		//db.Database.Migrate(); // �������� ��� ��������

		var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
		var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

		DbInitializer.Seed(db, userManager, roleManager); // ������� ��������� ������
	}
}
catch (Exception ex)
{
	// �������� ������
	var logger = app.Services.GetRequiredService<ILogger<Program>>();
	logger.LogError(ex, "������ ��� ������������� ���� ������.");
}


app.Run();

// todo: �������� ���� � ������������ �������������� � �� ����� seed
// todo: ��������� ���������� �� ��������������� ������� �����

// todo: ������ ��������� Windows Auth ������ LDAP. ������� ���������� ���������� ��������� ��� IIS. ���� � �����������, ��� ��� ��������
// todo: ��� ������ � IIS ���������� ��������� ��������� web.config �����, ���� ������ �������� ��� � ������
