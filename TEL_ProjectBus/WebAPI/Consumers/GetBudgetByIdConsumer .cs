using MassTransit;
using TEL_ProjectBus.WebAPI.Messages.Queries;

namespace TEL_ProjectBus.WebAPI.Consumers;

public class GetBudgetByIdConsumer(ILogger<GetBudgetByIdConsumer> logger) : IConsumer<GetBudgetByIdQuery>
{
	public async Task Consume(ConsumeContext<GetBudgetByIdQuery> context)
	{
		try
		{
			logger.LogInformation($"[GetBudgetByIdConsumer] Received query for {context.Message.BudgetItemId}");

			var budgetItemId = context.Message.BudgetItemId;
			// 238CC05B-532B-435F-BCEF-E7FB06226C08
			// TODO: Заменить на реальное обращение в БД
			//var budgetItem = Database.BudgetItems.FirstOrDefault(x => x.Id == budgetItemId);

			var newGuid = Guid.NewGuid();

			var budgetLines = new List<BudgetLineDto>
			{
				new BudgetLineDto
				{
					WBSTemplate = "Менеджмент проекта",
					BudgetName = "Project Leader  Руководитель проекта",
					Role = "PL_P18",
					Hours = 560,
					Quantity = 1,
					CPTCCPcs = 115000,
					RGPPCS = 33,
					Probability = 100,
					DataPlan = DateTime.Parse("2024-11-20"),
					CPTCCPlan = 115000,
					DataFact = DateTime.Parse("2024-12-21"),
					PriceTCCPcs = 172419.79m,
					PriceTCC = 172419.79m
				},
				new BudgetLineDto
				{
					WBSTemplate = "Менеджмент проекта",
					BudgetName = "Design manager  Менеджер по проектированию",
					Role = "PL_P18",
					Hours = 100,
					Quantity = 1,
					CPTCCPcs = 115000,
					RGPPCS = 33,
					Probability = 100,
					DataPlan = DateTime.Parse("2024-11-20"),
					CPTCCPlan = 115000,
					DataFact = DateTime.Parse("2024-12-21"),
					PriceTCCPcs = 172419.79m,
					PriceTCC = 172419.79m
				},
				new BudgetLineDto
				{
					WBSTemplate = "Менеджмент проекта",
					BudgetName = "Segment Sales Director  Директор по продажам в сегмент",
					Role = "PL_P18",
					Hours = 32,
					Quantity = 1,
					CPTCCPcs = 115000,
					RGPPCS = 33,
					Probability = 100,
					DataPlan = DateTime.Parse("2024-11-20"),
					CPTCCPlan = 115000,
					DataFact = DateTime.Parse("2024-12-21"),
					PriceTCCPcs = 172419.79m,
					PriceTCC = 172419.79m
				}
			};
			var budgetItemResponse = new GetBudgetByIdResponse
			{
				BudgetItemId = newGuid,
				ArticleNumber1C = "123456",
				VisibleOnPipeline = true,
				BudgetName = "Project Leader  Руководитель проекта",
				Role = "PL_P18",
				Hours = 560,
				Nomenclature = "Nomenclature",
				Classifier = "Classifier",
				Quantity = 1,
				EC = "EC",
				CP_TCC_Pcs = 115000,
				RGP_PcsPercent = 33,
				Probability = 100,
				PlannedDate = DateTime.Parse("2024-11-20"),
				CreatedAt = DateTime.Now,

				budgetLines = budgetLines
			};

			if (budgetItemResponse is null)
			{
				// в masstransit удобно отправлять ошибки через исключения:
				throw new Exception($"Budget item '{budgetItemId}' not found");
			}

			await context.RespondAsync(budgetItemResponse
				//	new GetBudgetByIdResponse
				//{
				//	BudgetItemId = budgetItem.Id,
				//	ArticleNumber1C = budgetItem.ArticleNumber1C,
				//	VisibleOnPipeline = budgetItem.VisibleOnPipeline,
				//	BudgetName = budgetItem.BudgetName,
				//	Role = budgetItem.Role,
				//	Hours = budgetItem.Hours,
				//	Nomenclature = budgetItem.Nomenclature,
				//	Classifier = budgetItem.Classifier,
				//	Quantity = budgetItem.Quantity,
				//	EC = budgetItem.EC,
				//	CP_TCC_Pcs = budgetItem.CP_TCC_Pcs,
				//	RGP_PcsPercent = budgetItem.RGP_PcsPercent,
				//	Probability = budgetItem.Probability,
				//	PlannedDate = budgetItem.PlannedDate,
				//	CreatedAt = budgetItem.CreatedAt
				//}
				);
		}
		catch (Exception ex)
		{
			//Console.WriteLine($"Error processing request: {ex.Message}");
			await context.RespondAsync<GetBudgetByIdResponse>(new
			{
				ErrorMessage = ex.Message
			});
		}
	}
}
