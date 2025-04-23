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

// Использую Request/Response паттерн - запросы и ответы асинхронно
// Controller: входная точка запроса (REST API).
// Consumer: обработчик запроса (слушает очередь).
// RequestClient: отправляет запрос в очередь.
// MassTransit: библиотека для работы с шиной сообщений.

// REST Controller -> IRequestClient<GetBudgetByIdQuery> -> MassTransit Bus -> RabbitMQ -> GetBudgetByIdConsumer -> возвращает GetBudgetByIdResponse


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

// Добавляем Swagger и загружаем XML-документацию
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "TEL_ProjectBus",
		Version = "v1"
	});

	// Добавляем саммари и описание к контроллерам
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	c.IncludeXmlComments(xmlPath);
});

// Настраиваем MassTransit с RabbitMQ
builder.Services.AddMassTransit(x =>
{
	x.SetKebabCaseEndpointNameFormatter(); // Используем kebab-case для именования очередей

	// Регистрируем обработчики сообщений
	x.AddConsumer<GetBudgetByIdConsumer>();
	x.AddConsumer<GetProjectsConsumer>();
	//x.AddConsumer<CurrentTimeConsumer>();

	// Регистрация RequestClient
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
				// routingKey не нужен для fanout
			});
			e.ConfigureConsumer<GetBudgetByIdConsumer>(context);
		});

		cfg.ConfigureEndpoints(context);
	});
});

// Добавляем Identity-сервис
builder.Services.AddIdentity<User, IdentityRole>(options =>
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


// Добавляем Middleware для проверки JWT
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

	//options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	//options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer( options =>
{

	// Настраиваем JWT-аутентификацию
	var jwtSettings = builder.Configuration.GetSection("JwtSettings");
	var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));

	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = jwtSettings["Issuer"],
		ValidAudience = jwtSettings["Audience"],
		LifetimeValidator = (notBefore, expires, token, parameters) => expires > DateTime.UtcNow, // todo: проверить
		IssuerSigningKey = key
	};
});

// Включаем авторизацию
builder.Services.AddAuthorization();


//builder.Services.AddHostedService<MessagePublisher>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();// Перенаправляет HTTP ? HTTPS

app.UseAuthentication(); // Проверяет JWT в каждом запросе (достаёт и расшифровывает токен, добавляет User в HttpContext)
app.UseAuthorization();  // Проверяет, разрешён ли доступ к endpoint'у (смотрит, есть ли у User права на выполнение запроса (например, роль Admin))

app.MapControllers(); // Подключает маршрутизацию контроллеров

using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	//db.Database.Migrate(); // применит все миграции
	DbInitializer.Seed(db); // добавит начальные данные
}

app.Run();
