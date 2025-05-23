using Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using TEL_ProjectBus.BLL.Budgets;
using TEL_ProjectBus.BLL.Mappers.MappingProfiles;
using TEL_ProjectBus.BLL.Projects;
using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.DAL.Entities;
using TEL_ProjectBus.WebAPI.Consumers.Budgets;
using TEL_ProjectBus.WebAPI.Consumers.Projects;
using TEL_ProjectBus.WebAPI.Messages.Queries.Budgets;
using TEL_ProjectBus.WebAPI.Messages.Queries.Projects;

// todo: избавиться от наследования в контрактах. + добавить автомаппер / после согласования архитектуры проекта и самих контрактов 


// Использую Request/Response паттерн - запросы и ответы асинхронно
// Controller: входная точка запроса (REST API).
// Consumer: обработчик запроса (слушает очередь).
// RequestClient: отправляет запрос в очередь.
// MassTransit: библиотека для работы с шиной сообщений.

// REST Controller -> IRequestClient<GetBudgetByIdQuery> -> MassTransit Bus -> RabbitMQ -> GetBudgetByIdConsumer -> возвращает GetBudgetByIdResponse


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

if (builder.Environment.IsDevelopment())
	builder.Logging.SetMinimumLevel(LogLevel.Debug);

#region ──────────────────────────────────  DB  ──────────────────────────────────

builder.Services.AddDbContext<AppDbContext>(options =>
{
	var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
	//options.UseNpgsql(connectionString);
	options.UseSqlServer(connectionString, sqlOptions =>
	{
		sqlOptions.EnableRetryOnFailure(); // временный сбой
	})
	.EnableSensitiveDataLogging() // параметры запросов
	.EnableDetailedErrors();     // стек при ошибках
});

#endregion

#region ──────────────────────────────  Services  ────────────────────────────────

//builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<BudgetService>(); // или AddScoped<IBudgetService, BudgetService>()
builder.Services.AddScoped<ProjectService>();

#endregion

#region ─────────────────────────────── Identity  ───────────────────────────────

// Добавляем Identity-сервис
builder.Services
	.AddIdentity<User, Role>(options =>
{
	// Настройка требований к паролю
	options.Password.RequireDigit = true;
	options.Password.RequireUppercase = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders(); // подключаем токены для сброса пароля, подтверждения email и т.д.
#endregion

#region ───────────────────────────  JWT‑аутентификация  ──────────────────────────
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));
// Добавляем Middleware для проверки JWT
builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // ← «главная»
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // проверки
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // 401/403
})
.AddJwtBearer(options =>
{
	// Настраиваем JWT-аутентификацию
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidIssuer = jwtSettings["Issuer"],
		ValidAudience = jwtSettings["Audience"],
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateIssuerSigningKey = true,
		LifetimeValidator = (notBefore, expires, token, parameters) => expires > DateTime.UtcNow, // todo: проверить
		IssuerSigningKey = key
	};
});

#endregion

#region ──────────────────────────────  MassTransit  ─────────────────────────────

var useInMemory = builder.Configuration.GetValue<bool>("UseInMemoryTransport");

var host = builder.Configuration["RabbitMq:Host"];
var user = builder.Configuration["RabbitMq:User"];
var pass = builder.Configuration["RabbitMq:Password"];

if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
{
	throw new ArgumentException("RabbitMQ connection parameters are not set.");
}

// Настраиваем MassTransit с RabbitMQ
builder.Services.AddMassTransit(x =>
{
	x.SetKebabCaseEndpointNameFormatter(); // Используем kebab-case для именования очередей

	// Регистрируем обработчики сообщений
	x.AddConsumer<CreateBudgetConsumer>();
	x.AddConsumer<GetBudgetByIdConsumer>();
	x.AddConsumer<GetBudgetsByProjectIdConsumer>();
	x.AddConsumer<UpdateBudgetConsumer>();
	x.AddConsumer<DeleteBudgetConsumer>();

	x.AddConsumer<CreateProjectConsumer>();
	x.AddConsumer<GetProjectsConsumer>();
	x.AddConsumer<GetProjectProfileConsumer>();
	x.AddConsumer<UpdateProjectProfileConsumer>();

	// Регистрация RequestClient
	x.AddRequestClient<GetBudgetByIdQuery>();
	x.AddRequestClient<GetProjectsQuery>();
	x.AddRequestClient<GetProjectProfileQuery>();

	if (useInMemory)
		x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));
	else
		x.UsingRabbitMq((context, cfg) =>
		{
			cfg.Host(host, h =>
			{
				h.Username(user);
				h.Password(pass);
			});

			cfg.ConfigureEndpoints(context);
		});
});

#endregion

#region ─────────────────────────────────  CORS  ────────────────────────────────

var allowedOrigins = builder.Configuration
	.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
	options.AddPolicy("Cors", builder =>
	{
		builder.WithOrigins(allowedOrigins)
			   .AllowAnyHeader()
			   .AllowAnyMethod()
			   .AllowCredentials();
	});
});

#endregion

#region ──────────────────────────────  Swagger  ────────────────────────────────

// Добавляем Swagger и загружаем XML-документацию
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "TEL_ProjectBus",
		Version = "v1"
	});

	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "JWT Bearer token",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		BearerFormat = "JWT"
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id   = "Bearer" }
			},
			Array.Empty<string>()
		}
	});

	// Добавляем саммари и описание к контроллерам
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	c.IncludeXmlComments(xmlPath);
});

#endregion

builder.Services.AddControllers();

var app = builder.Build();

#region ─────────────────────────────  Middleware  ───────────────────────────────

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage(); // todo: что это?
	app.UseSwagger();
	app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
	app.UseHttpsRedirection();// Перенаправляет HTTP ? HTTPS
app.UseCors("Cors");
app.UseAuthentication(); // Проверяет JWT в каждом запросе (достаёт и расшифровывает токен, добавляет User в HttpContext)
app.UseAuthorization();  // Проверяет, разрешён ли доступ к endpoint'у (смотрит, есть ли у User права на выполнение запроса (например, роль Admin))
app.MapControllers(); // Подключает маршрутизацию контроллеров

#endregion

#region ──────────────────────────────  DB seed  ────────────────────────────────
var useDbSeed = builder.Configuration.GetValue<bool>("DbSeed:UseDbSeed");

if (useDbSeed)
	using (var scope = app.Services.CreateScope())
	{
		var recreateDb = builder.Configuration.GetValue<bool>("DbSeed:RecreateDb");
		var clearDbData = builder.Configuration.GetValue<bool>("DbSeed:ClearData");
		var loadTestData = builder.Configuration.GetValue<bool>("DbSeed:LoadTestData"); 
		var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
		//db.Database.Migrate(); // применит все миграции

		await DbInitializer.Seed(db, 
			scope.ServiceProvider.GetRequiredService<UserManager<User>>(), 
			scope.ServiceProvider.GetRequiredService<RoleManager<Role>>(),
			recreateDb: recreateDb, // true - удалить и создать БД заново
			clearDbData: clearDbData, // true - очистить данные в БД,
			loadTestData: loadTestData
			);
	}

#endregion

app.Run();
