using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.DAL.Entities;
using TEL_ProjectBus.DAL.Entities.Budgets;
using TEL_ProjectBus.DAL.Entities.Customers;
using TEL_ProjectBus.DAL.Entities.Projects;
using TEL_ProjectBus.DAL.Entities.Reference;

namespace Infrastructure;
public static class DbInitializer
{
	private static string[] testRoles = new[]
	{
		"Admin", // оставил, вдруг понадобится
		"PM",  // Project Manager
		"PL",  // Project Leader
		"SD",  // Sales Director
		"SM",  // Sales Manager
		"SiM", // Site Manager
		"PE",  // Engineer
		"RO",  // Resource Owner
		"LC",  // Logistics Coordinator
		"DM",  // Design Manager
		"IM",  // Installation Manager
		"AM",  // Administrative Manager
		"SSD"  // Segment Sales Director
	};

	public static async Task Seed(AppDbContext context, UserManager<User>? userManager = null, RoleManager<IdentityRole>? roleManager = null)
	{
		context.Database.EnsureDeleted();
		context.Database.EnsureCreated();

		if (context.Customers.Any() || context.Projects.Any() || context.Budgets.Any())
			return;

		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};

		var folderPath = Path.Combine(AppContext.BaseDirectory, "Seed", "TestData");

		if (userManager != null && roleManager != null)
		{
			for (int i = 0; i < testRoles.Length; i++)
			{
				var role = testRoles[i];

				// фиксированный GUID: 000...001, 002, 003 … до 999 ролей хватит
				var guid = $"00000000-0000-0000-0000-000000000{(i + 1).ToString("D3")}";

				await EnsureUserWithFixedIdAsync(
					userManager,
					roleManager,
					guid,
					$"{role.ToLower()}_test",
					"Test@123",
					role);
			}
		}

		context.Database.OpenConnection();

		// --- Справочники ---
		var classifiersJson = File.ReadAllText(Path.Combine(folderPath , "test_classifiers.json"));
		var classifiers = JsonSerializer.Deserialize<List<Classifier>>(classifiersJson, options);

		context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Classifier] ON");
		context.Classifiers.AddRange(classifiers);
		context.SaveChanges();
		context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Classifier] OFF");

		var budgetGroupsJson = File.ReadAllText(Path.Combine(folderPath, "test_budget_groups.json"));
		var budgetGroups = JsonSerializer.Deserialize<List<BudgetGroup>>(budgetGroupsJson, options);
		context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Ref_BudgetGroup] ON");
		context.BudgetGroups.AddRange(budgetGroups);
		context.SaveChanges();
		context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Ref_BudgetGroup] OFF");

		// --- Клиенты ---
		var customersJson = File.ReadAllText(Path.Combine(folderPath,  "test_customers.json"));
		var customers = JsonSerializer.Deserialize<List<Customer>>(customersJson, options);
		foreach (var customer in customers)
		{
			customer.DateCreated = DateTime.SpecifyKind((DateTime)customer.DateCreated, DateTimeKind.Utc);
			customer.DateChanged = DateTime.SpecifyKind((DateTime)customer.DateChanged, DateTimeKind.Utc);
		}
		context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Customer] ON");
		context.Customers.AddRange(customers);
		context.SaveChanges();
		context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Customer] OFF");

		// --- Проекты ---
		var projectsJson = File.ReadAllText(Path.Combine(folderPath, "test_projects.json"));
		var projects = JsonSerializer.Deserialize<List<Project>>(projectsJson, options);
		foreach (var project in projects)
		{
			project.DateInitiation = DateTime.SpecifyKind(project.DateInitiation, DateTimeKind.Utc);
		}
		context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Project] ON");
		context.Projects.AddRange(projects);
		context.SaveChanges();
		context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Project] OFF");

		// --- Параметры проектов ---
		var projectParamsJson = File.ReadAllText(Path.Combine(folderPath, "test_project_params.json"));
		var projectParams = JsonSerializer.Deserialize<List<ProjectParameter>>(projectParamsJson, options);
		foreach (var projectParam in projectParams)
		{
			projectParam.ProjectBegin = DateTime.SpecifyKind(projectParam.ProjectBegin, DateTimeKind.Utc);
			projectParam.ProjectEnd = DateTime.SpecifyKind(projectParam.ProjectEnd, DateTimeKind.Utc);
			projectParam.DateChanged = DateTime.SpecifyKind((DateTime)projectParam.DateChanged, DateTimeKind.Utc);
		}
		context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [ProjectParameters] ON");
		context.ProjectParameters.AddRange(projectParams);
		context.SaveChanges();
		context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [ProjectParameters] OFF");

		// --- Бюджеты ---
		var budgetsJson = File.ReadAllText(Path.Combine(folderPath, "test_budgets.json"));
		var budgets = JsonSerializer.Deserialize<List<Budget>>(budgetsJson, options);
		foreach (var budget in budgets)
		{
			budget.DateChanged = DateTime.SpecifyKind((DateTime)budget.DateChanged, DateTimeKind.Utc);
		}
		context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Budget] ON");
		context.Budgets.AddRange(budgets);
		context.SaveChanges();
		context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Budget] OFF");

		context.Database.CloseConnection();
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

	private static async Task<User> EnsureUserWithFixedIdAsync(
	UserManager<User> userManager,
	RoleManager<IdentityRole> roleManager,
	string id,          // нужный Id
	string userName,
	string password,
	string role)
	{
		// если роль ещё не создавалась
		if (!await roleManager.RoleExistsAsync(role))
			await roleManager.CreateAsync(new IdentityRole(role));

		// ищем именно по Id
		var user = await userManager.FindByIdAsync(id);
		if (user == null)
		{
			user = new User
			{
				Id = id,                     // ← фиксируем Id
				UserName = userName,
				NormalizedUserName = userName.ToUpper(),
				Email = $"{userName}@example.com",
				NormalizedEmail = $"{userName}@example.com".ToUpper(),
				EmailConfirmed = true
			};
			await userManager.CreateAsync(user, password);
		}

		// роль всё равно привязываем
		await userManager.AddToRoleAsync(user, role);
		return user;
	}

}
