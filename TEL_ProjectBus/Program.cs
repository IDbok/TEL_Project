using MassTransit;
using Microsoft.OpenApi.Models;
using System.Reflection;
using TEL_ProjectBus;
using TEL_ProjectBus.Consumers;
using TEL_ProjectBus.Messages.Queries;

// Использую Request/Response паттерн - запросы и ответы асинхронно
// Controller: входная точка запроса (REST API).
// Consumer: обработчик запроса (слушает очередь).
// RequestClient: отправляет запрос в очередь.
// MassTransit: библиотека для работы с шиной сообщений.

// REST Controller -> IRequestClient<GetBudgetByIdQuery> -> MassTransit Bus -> RabbitMQ -> GetBudgetByIdConsumer -> возвращает GetBudgetByIdResponse


var builder = WebApplication.CreateBuilder(args);

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

// Настройка MassTransit с RabbitMQ
builder.Services.AddMassTransit(x =>
{
	x.SetKebabCaseEndpointNameFormatter(); // Используем kebab-case для именования очередей

	// Регистрируем обработчики сообщений
	x.AddConsumer<GetBudgetByIdConsumer>();
	//x.AddConsumer<CurrentTimeConsumer>();

	// Регистрация RequestClient
	x.AddRequestClient<GetBudgetByIdQuery>();

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

//builder.Services.AddHostedService<MessagePublisher>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();// Перенаправляет HTTP ? HTTPS

//app.UseAuthorization();

app.MapControllers(); // Подключает маршрутизацию контроллеров

app.Run();
