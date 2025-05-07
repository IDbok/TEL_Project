
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

using TEL_ProjectBus.DAL.Entities.Budgets;
using TEL_ProjectBus.DAL.Entities.Projects;
using TEL_ProjectBus.DAL.Entities.Reference;

//CreatBudgetTestJson();
//CreatProjectJson();
//CreatClassifierJson();
//CreateBudgetGroupJson();

void CreateBudgetGroupJson()
{
	var jsonpath = @"C:\Users\bokar\source\repos\TEL_Project\TEL_ProjectBus\Seed\TestData\test_budget_groups.json";
	// Create a new Project objects for tests
	var itemsCount = 3;
	var random = new Random();
	var itemList = new List<BudgetGroup>();
	for (int i = 0; i < itemsCount; i++)
	{
		var item = new BudgetGroup
		{
			Id = i + 1,
			Name = $"BudgetGroup {i + 1}",
		};
		itemList.Add(item);
	}
	SaveJson(jsonpath, itemList);
}

void CreatClassifierJson()
{
	var jsonpath = @"C:\Users\bokar\source\repos\TEL_Project\TEL_ProjectBus\Seed\TestData\test_Classifiers.json";

	// Create a new Project objects for tests
	var itemsCount = 3000;
	var random = new Random();
	var itemList = new List<Classifier>();

	for (int i = 0; i < itemsCount; i++)
	{
		var item = new Classifier
		{
			Id = i + 1,
			ClassifierCode = $"Classifier {i + 1}",
		};
		itemList.Add(item);
	}
	SaveJson(jsonpath, itemList);
}
void CreatProjectJson()
{
	var jsonpath = @"C:\Users\bokar\source\repos\TEL_Project\TEL_ProjectBus\Seed\TestData\test_projects.json";

	// Create a new Project objects for tests
	var projectItemsCount = 30;
	var random = new Random();
	var projectList = new List<Project>();

	for (int i = 0; i < projectItemsCount; i++)
	{
		var project = new Project
		{
			Id = i + 1,
			ProjectName = $"Project {i + 1}",
			ProjectCode = $"Code {i + 1}",
			DateInitiation = DateTime.Now,
			CustomerId = random.Next(1, 4),
			ClassifierId = random.Next(1, 40),
		};
		projectList.Add(project);
	}
	SaveJson(jsonpath, projectList);
}

void CreatBudgetTestJson()
{
	var jsonpath = @"C:\Users\bokar\source\repos\TEL_Project\TEL_ProjectBus\Seed\TestData\test_budgets.json";

	var budgerList = new List<Budget>();

	// Create a new Budget objects for tests
	var budgetItemsCount = 100;
	var random = new Random();

	for (int i = 0; i < budgetItemsCount; i++)
	{
		var budget = new Budget
		{
			Id = i + 1,
			BudgetGroupId = random.Next(1, 4),
			ProjectId = random.Next(1,3),
			ERPId = Guid.NewGuid().ToString(),
			ClassifierId = random.Next(41, 2000),
			VisOnPipeline = true,
			Name = $"Budget {i + 1}",
			Description = $"Description for Budget {i + 1}",
			RoleId = 1,
			ManHoursCost = 100.0m,
			Amount = 1000.0m,
			RgpPercent = 10.0m,
			Version = 1,
			Probability = 0.5m,
			DatePlan = DateOnly.FromDateTime(DateTime.Now),
			DateFact = DateOnly.FromDateTime(DateTime.Now),
			CPTCCFact = 100.0m,
			CalcPriceTCPcs = 10.0m,
			CalcPriceTCC = 100.0m,
			CalcCV = 10.0m,
			CalcSV = 5.0m,
			CalcEV = 50.0m,
			CalcCPI = 1.2m,
			CalcSPI = 1.5m
		};
		budgerList.Add(budget);
	}

	SaveJson(jsonpath, budgerList);
}
void SaveJson<T>(string jsonpath, T projectList)
{
	// Serialize the list of Project objects back to JSON
	var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
	var jsonString = JsonSerializer.Serialize(projectList, options);
	// Write the JSON string to a new file
	File.WriteAllText(jsonpath, jsonString, Encoding.UTF8);
}

class ProjectTest
{
	public string Id { get; set; }
	public string ProjectName { get; set; } = string.Empty;
	public string ProjectCode { get; set; } = string.Empty;
	public DateTime DateInitiation { get; set; }
	public string CustomerId { get; set; } = string.Empty;
	public string ClassifierId { get; set; } = string.Empty;

}

