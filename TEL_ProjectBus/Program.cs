using MassTransit;
using Microsoft.OpenApi.Models;
using System.Reflection;
using TEL_ProjectBus;
using TEL_ProjectBus.Consumers;
using TEL_ProjectBus.Messages.Queries;

// ��������� Request/Response ������� - ������� � ������ ����������
// Controller: ������� ����� ������� (REST API).
// Consumer: ���������� ������� (������� �������).
// RequestClient: ���������� ������ � �������.
// MassTransit: ���������� ��� ������ � ����� ���������.

// REST Controller -> IRequestClient<GetBudgetByIdQuery> -> MassTransit Bus -> RabbitMQ -> GetBudgetByIdConsumer -> ���������� GetBudgetByIdResponse


var builder = WebApplication.CreateBuilder(args);

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

// ��������� MassTransit � RabbitMQ
builder.Services.AddMassTransit(x =>
{
	x.SetKebabCaseEndpointNameFormatter(); // ���������� kebab-case ��� ���������� ��������

	// ������������ ����������� ���������
	x.AddConsumer<GetBudgetByIdConsumer>();
	//x.AddConsumer<CurrentTimeConsumer>();

	// ����������� RequestClient
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
				// routingKey �� ����� ��� fanout
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

app.UseHttpsRedirection();// �������������� HTTP ? HTTPS

//app.UseAuthorization();

app.MapControllers(); // ���������� ������������� ������������

app.Run();
