using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.DAL.Entities;
using TEL_ProjectBus.DAL.Entities.Budgets;
using TEL_ProjectBus.DAL.Entities.Customers;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace Infrastructure;
public static class DbInitializer
{
	public static void Seed(AppDbContext context, UserManager<User>? userManager = null, RoleManager<IdentityRole>? roleManager = null)
	{
		// проверить существование БД, если её нет — создать
		context.Database.EnsureDeleted();
		context.Database.EnsureCreated();

		if (true)
			return;

		if (context.Customers.Any() || context.Projects.Any() || context.Budgets.Any())
			return;

		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};

		var folderPath = @"C:\Users\bokar\source\repos\TEL_Project\TEL_ProjectBus\TestData\";

		// --- Роли и пользователи ---
		if (userManager != null && roleManager != null)
		{
			// создаём тестовые роли и пользователей под каждую роль
			CreateUserAndRoleIfNotExists(userManager, roleManager, "admin", "Admin@123", "Admin").Wait();
			CreateUserAndRoleIfNotExists(userManager, roleManager, "testuser", "Test@123", "User").Wait();
		}

		// --- Клиенты ---
		var customersJson = File.ReadAllText(folderPath + "test_customers.json");
		var customers = JsonSerializer.Deserialize<List<Customer>>(customersJson, options);
		foreach (var customer in customers) // <-- вот здесь исправлено
		{
			customer.DateCreated = DateTime.SpecifyKind((DateTime)customer.DateCreated, DateTimeKind.Utc);
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

	private static async Task CreateUserAndRoleIfNotExists(
		UserManager<User> userManager,
		RoleManager<IdentityRole> roleManager,
		string username,
		string password,
		string role)
	{
		if (!await roleManager.RoleExistsAsync(role))
		{
			await roleManager.CreateAsync(new IdentityRole(role));
		}

		var user = await userManager.FindByNameAsync(username);
		if (user == null)
		{
			user = new User
			{
				UserName = username,
				Email = $"{username}@example.com",
				EmailConfirmed = true
			};
			await userManager.CreateAsync(user, password);
			await userManager.AddToRoleAsync(user, role);
		}
	}

}
