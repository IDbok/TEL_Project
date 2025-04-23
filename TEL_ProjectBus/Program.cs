using Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using TEL_ProjectBus;
using TEL_ProjectBus.Consumers;
using TEL_ProjectBus.DAL.Entities;
using TEL_ProjectBus.Messages.Queries;

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
	options.UseNpgsql(connectionString);
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

	//options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	//options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer( options =>
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
});

// �������� �����������
builder.Services.AddAuthorization();


//builder.Services.AddHostedService<MessagePublisher>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();// �������������� HTTP ? HTTPS

app.UseAuthentication(); // ��������� JWT � ������ ������� (������ � �������������� �����, ��������� User � HttpContext)
app.UseAuthorization();  // ���������, �������� �� ������ � endpoint'� (�������, ���� �� � User ����� �� ���������� ������� (��������, ���� Admin))

app.MapControllers(); // ���������� ������������� ������������

using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	//db.Database.Migrate(); // �������� ��� ��������
	DbInitializer.Seed(db); // ������� ��������� ������
}

app.Run();
