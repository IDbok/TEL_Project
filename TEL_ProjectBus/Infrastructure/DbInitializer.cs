using System.Text.Json;
using TEL_ProjectBus.DAL.Entities;

namespace Infrastructure;
public static class DbInitializer
{
	public static void Seed(AppDbContext context)
	{
		// проверить существование БД, если её нет — создать
		context.Database.EnsureDeleted();
		context.Database.EnsureCreated();

		if (context.Customers.Any() || context.Projects.Any() || context.Budgets.Any() 
			|| context.Expenses.Any() )
			return;

		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};

		var folderPath = @"C:\Users\bokar\source\repos\TEL_Project\TEL_ProjectBus\TestData\";

		// --- Клиенты ---
		var customersJson = File.ReadAllText(folderPath + "test_customers.json");
		var customers = JsonSerializer.Deserialize<List<Customer>>(customersJson, options);
		foreach (var customer in customers) // <-- вот здесь исправлено
		{
			customer.DateCreated = DateTime.SpecifyKind(customer.DateCreated, DateTimeKind.Utc);
			customer.DateChanged = DateTime.SpecifyKind(customer.DateChanged, DateTimeKind.Utc);
		}
		context.Customers.AddRange(customers);

		// --- Проекты ---
		var projectsJson = File.ReadAllText(folderPath + "test_projects_with_3_customers.json");
		var projects = JsonSerializer.Deserialize<List<Project>>(projectsJson, options);
		foreach (var project in projects) // <-- здесь исправлено
		{
			project.DateInitiation = DateTime.SpecifyKind(project.DateInitiation, DateTimeKind.Utc);
		}
		context.Projects.AddRange(projects);

		// --- Бюджеты ---
		var budgetsJson = File.ReadAllText(folderPath + "test_budgets_for_two_projects.json");
		var budgets = JsonSerializer.Deserialize<List<Budget>>(budgetsJson, options);
		foreach (var budget in budgets) // <-- здесь исправлено
		{
			budget.DateChanged = DateTime.SpecifyKind(budget.DateChanged, DateTimeKind.Utc);
		}
		context.Budgets.AddRange(budgets);

		context.SaveChanges();

	}

}
