using DAL.Entities;

namespace DAL;
public static class DbInitializer
{
	public static void Seed(AppDbContext context)
	{

		if (context.BudgetItems.Any()) return; // уже есть данные — пропускаем

		var budgetItem = new BudgetItemDto
		{
			Id = Guid.NewGuid(),
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
			CreatedAt = DateTime.UtcNow
		};

		budgetItem.BudgetLines = new List<BudgetLineDto>
		{
			new BudgetLineDto
			{
				Id = Guid.NewGuid(),
				BudgetItemId = budgetItem.Id,
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
				Id = Guid.NewGuid(),
				BudgetItemId = budgetItem.Id,
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

		context.BudgetItems.Add(budgetItem);
		context.SaveChanges();
	}
}
