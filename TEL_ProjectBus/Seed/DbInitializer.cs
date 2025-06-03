using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.DAL.Entities;
using TEL_ProjectBus.DAL.Entities.Budgets;
using TEL_ProjectBus.DAL.Entities.Customers;
using TEL_ProjectBus.DAL.Entities.Projects;
using TEL_ProjectBus.DAL.Entities.Reference;
using TEL_ProjectBus.DAL.Extensions;

namespace Infrastructure;
public static class DbInitializer
{
	public static string[] testRoles = new[]
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

	public static List<Role> testRoles2 = new()
{
	new Role
	{
		Name = "Admin",
		DisplayName = "Admin",
		DisplayNameRu = "Администратор",
		Description = "Администратор системы"
	},
	new Role
	{
		Name = "PM",
		DisplayName = "Project Manager",
		DisplayNameRu = "Руководитель проекта",
		Description = "Руководитель проекта"
	},
	new Role
	{
		Name = "PL",
		DisplayName = "Project Leader",
		DisplayNameRu = "Руководитель группы",
		Description = "Руководитель группы"
	},
	new Role
	{
		Name = "SD",
		DisplayName = "Sales Director",
		DisplayNameRu = "Директор по продажам",
		Description = "Директор по продажам"
	},
	new Role
	{
		Name = "SM",
		DisplayName = "Sales Manager",
		DisplayNameRu = "Менеджер по продажам",
		Description = "Менеджер по продажам"
	},
	new Role
	{
		Name = "SiM",
		DisplayName = "Site Manager",
		DisplayNameRu = "Менеджер по монтажу",
		Description = "Менеджер по монтажу"
	},
	new Role
	{
		Name = "PE",
		DisplayName = "Engineer",
		DisplayNameRu = "Инженер",
		Description = "Инженер"
	},
	new Role
	{
		Name = "RO",
		DisplayName = "Resource Owner",
		DisplayNameRu = "Владелец ресурса",
		Description = "Владелец ресурса"
	},
	new Role
	{
		Name = "LC",
		DisplayName = "Logistics Coordinator",
		DisplayNameRu = "Координатор логистики",
		Description = "Координатор логистики"
	},
	new Role
	{
		Name = "DM",
		DisplayName = "Design Manager",
		DisplayNameRu = "Менеджер по проектированию",
		Description = "Менеджер по проектированию"
	},
	new Role
	{
		Name = "IM",
		DisplayName = "Installation Manager",
		DisplayNameRu = "Менеджер по монтажу",
		Description = "Менеджер по монтажу"
	},
	new Role
	{
		Name = "AM",
		DisplayName = "Administrative Manager",
		DisplayNameRu = "Административный менеджер",
		Description = "Административный менеджер"
	},
	new Role
	{
		Name = "SSD",
		DisplayName = "Segment Sales Director",
		DisplayNameRu = "Директор по продажам сегмента",
		Description = "Директор по продажам сегмента"
	}
};

	public static string testUserPassword = "Test@123";

	public static async Task Seed(AppDbContext context, 
		UserManager<User>? userManager = null, RoleManager<Role>? roleManager = null, 
		bool recreateDb = false, bool clearDbData = false, bool loadTestData = false)

	{
        var isInMemory = context.Database.IsInMemory(); // проверка для тестов, т.к. in-memory БД не поддерживает ExecuteSqlRaw

        if ((recreateDb))
		{
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();
		}

		if (!isInMemory && clearDbData && !recreateDb)          // чистим, если БД оставляем
			await ClearDataAsync(context);

		if (loadTestData)
        {
            if (isInMemory)
                await SeedViaDbSets(context, userManager, roleManager);        // добавляем через ctx.Set<T>().AddRange(...)
            else
                await LoadTestData(context, userManager, roleManager);
        }
	}

	private static async Task LoadTestData(AppDbContext context, UserManager<User>? userManager, RoleManager<Role>? roleManager)
	{
		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};
		options.Converters.Add(new ClassifierKeyJsonConverter());

		var folderPath = Path.Combine(AppContext.BaseDirectory, "Seed", "TestData");

		if (userManager != null && roleManager != null)
		{
			await CreateTestUsers(userManager, roleManager);
		}

		context.Database.OpenConnection();

		// --- Справочники ---
		var classifiersJson = File.ReadAllText(Path.Combine(folderPath, "test_classifiers.json"));
		var classifiers = JsonSerializer.Deserialize<List<Classifier>>(classifiersJson, options);
		SeedData(context, classifiers, "Classifier");

		var budgetGroupsJson = File.ReadAllText(Path.Combine(folderPath, "budget_groups.json"));
		var budgetGroups = JsonSerializer.Deserialize<List<BudgetGroup>>(budgetGroupsJson, options);
		SeedData(context, budgetGroups, "Ref_BudgetGroup");

		var projectPhaseJson = File.ReadAllText(Path.Combine(folderPath, "project_phases.json"));
		var projectPhases = JsonSerializer.Deserialize<List<ProjectPhase>>(projectPhaseJson, options);
		SeedData(context, projectPhases, "Ref_ProjectPhase");

		var projectStageJson = File.ReadAllText(Path.Combine(folderPath, "project_stages.json"));
		var projectStages = JsonSerializer.Deserialize<List<ProjectStage>>(projectStageJson, options);
		SeedData(context, projectStages, "Ref_ProjectStage");

		var projectStatusJson = File.ReadAllText(Path.Combine(folderPath, "project_statuses.json"));
		var projectStatuses = JsonSerializer.Deserialize<List<ProjectStatus>>(projectStatusJson, options);
		SeedData(context, projectStatuses, "Ref_ProjectStatus");

		// --- Клиенты ---
		var customersJson = File.ReadAllText(Path.Combine(folderPath, "test_customers.json"));
		var customers = JsonSerializer.Deserialize<List<Customer>>(customersJson, options);
		foreach (var customer in customers)
		{
			customer.DateChanged = DateTime.SpecifyKind((DateTime)customer.DateChanged, DateTimeKind.Utc);
		}
		SeedData(context, customers, "Customer");

		// --- Проекты ---
		var projectsJson = File.ReadAllText(Path.Combine(folderPath, "test_projects.json"));
		var projects = JsonSerializer.Deserialize<List<Project>>(projectsJson, options);
		foreach (var project in projects)
		{
			project.DateInitiation = DateTime.SpecifyKind(project.DateInitiation, DateTimeKind.Utc);
		}
		SeedData(context, projects, "Project");
		//context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Project] ON");
		//context.Projects.AddRange(projects);
		//context.SaveChanges();
		//context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Project] OFF");

		// --- Параметры проектов ---
		var projectParamsJson = File.ReadAllText(Path.Combine(folderPath, "test_project_params.json"));
		var projectParams = JsonSerializer.Deserialize<List<ProjectParameter>>(projectParamsJson, options);
		foreach (var projectParam in projectParams)
		{
			projectParam.ProjectBegin = DateTime.SpecifyKind(projectParam.ProjectBegin, DateTimeKind.Utc);
			projectParam.ProjectEnd = DateTime.SpecifyKind(projectParam.ProjectEnd, DateTimeKind.Utc);
			projectParam.DateChanged = DateTime.SpecifyKind((DateTime)projectParam.DateChanged, DateTimeKind.Utc);
		}
		SeedData(context, projectParams, "ProjectParameters");
		//context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [ProjectParameters] ON");
		//context.ProjectParameters.AddRange(projectParams);
		//context.SaveChanges();
		//context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [ProjectParameters] OFF");

		// --- Бюджеты ---
		var budgetsJson = File.ReadAllText(Path.Combine(folderPath, "test_budgets.json"));
		var budgets = JsonSerializer.Deserialize<List<Budget>>(budgetsJson, options);
		//foreach (var budget in budgets)
		//{
		//	budget.DateChanged = DateTime.SpecifyKind((DateTime)budget.DateChanged, DateTimeKind.Utc);
		//}
		SeedData(context, budgets, "Budget");
		//context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Budget] ON");
		//context.Budgets.AddRange(budgets);
		//context.SaveChanges();
		//context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Budget] OFF");

		context.Database.CloseConnection();
	}

    private static async Task SeedViaDbSets(
    AppDbContext ctx,
    UserManager<User>? userManager = null,
    RoleManager<Role>? roleManager = null)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        jsonOptions.Converters.Add(new ClassifierKeyJsonConverter());

        /* ───── 1. пользователей и роли ───── */
        if (userManager != null && roleManager != null)
            await CreateTestUsers(userManager, roleManager);   // готовый метод

        /* ───── 2. читаем файлы ───── */
        var folder = Path.Combine(AppContext.BaseDirectory, "Seed", "TestData");

        // --- Справочники ---
        ctx.SeedFromJson<Classifier>(folder, "test_classifiers.json", jsonOptions);
        ctx.SeedFromJson<BudgetGroup>(folder, "budget_groups.json", jsonOptions);
        ctx.SeedFromJson<ProjectPhase>(folder, "project_phases.json", jsonOptions);
        ctx.SeedFromJson<ProjectStage>(folder, "project_stages.json", jsonOptions);
        ctx.SeedFromJson<ProjectStatus>(folder, "project_statuses.json", jsonOptions);

        // --- Клиенты ---
        ctx.SeedFromJson<Customer>(folder, "test_customers.json", jsonOptions, fixDates: e =>
        {
            e.DateChanged = DateTime.SpecifyKind((DateTime)e.DateChanged, DateTimeKind.Utc);
        });

        // --- Проекты ---
        ctx.SeedFromJson<Project>(folder, "test_projects.json", jsonOptions, fixDates: e =>
        {
            e.DateInitiation = DateTime.SpecifyKind(e.DateInitiation, DateTimeKind.Utc);
        });

        // --- Параметры проектов ---
        ctx.SeedFromJson<ProjectParameter>(folder, "test_project_params.json", jsonOptions, fixDates: e =>
        {
            e.ProjectBegin = DateTime.SpecifyKind(e.ProjectBegin, DateTimeKind.Utc);
            e.ProjectEnd = DateTime.SpecifyKind(e.ProjectEnd, DateTimeKind.Utc);
            e.DateChanged = DateTime.SpecifyKind((DateTime)e.DateChanged, DateTimeKind.Utc);
        });

        // --- Бюджеты ---
        ctx.SeedFromJson<Budget>(folder, "test_budgets.json", jsonOptions);

        /* ───── 3. сохраняем одним коммитом ───── */
        await ctx.SaveChangesAsync();
    }

    public static void SeedFromJson<T>(
    this DbContext ctx,
    string folder,
    string file,
    JsonSerializerOptions options,
    Action<T>? fixDates = null)   // необязательный «пост-процессор»
    where T : class
    {
        var json = File.ReadAllText(Path.Combine(folder, file));
        var entities = JsonSerializer.Deserialize<List<T>>(json, options)!;

        if (fixDates != null)
            foreach (var e in entities) fixDates(e);

        ctx.Set<T>().AddRange(entities);        // ← ровно то, что нужно In-Memory-БД
    }

    public static void SeedFromJson<T>(
		this DbContext ctx,
		string folderPath,
		string fileName,
		JsonSerializerOptions options,
		string tableName)          // "Classifier" или "Ref_BudgetGroup" и т.д.
		where T : class
	{
		var json = File.ReadAllText(Path.Combine(folderPath, fileName));
		var entities = JsonSerializer.Deserialize<List<T>>(json, options)!;

		ctx.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT [{tableName}] ON");
		ctx.Set<T>().AddRange(entities);
		ctx.SaveChanges();
		ctx.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {tableName} OFF");
	}

	public static void SeedData<T>(
		this DbContext ctx,
		List<T> entities,
		string tableName)          // "Classifier" или "Ref_BudgetGroup" и т.д.
		where T : class
	{
		var identityInsertEnabled = false;
		try
		{
			ctx.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT [{tableName}] ON");
			identityInsertEnabled = true;
		}
		catch (Exception ex)
		{
			// Optionally log: identity insert not enabled, likely no identity column
			// Console.WriteLine($"IDENTITY_INSERT not enabled for {tableName}: {ex.Message}");
		}

		ctx.Set<T>().AddRange(entities);
		ctx.SaveChanges();

		if (identityInsertEnabled)
		{
			try
			{
				ctx.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT [{tableName}] OFF");
			}
			catch
			{
				// Optionally log: failed to turn off IDENTITY_INSERT
			}
		}
	}

	private static async Task CreateTestUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
	{
		for (int i = 0; i < testRoles2.Count; i++)
		{
			var role = testRoles2[i];

			// фиксированный GUID: 000...001, 002, 003 … до 999 ролей хватит
			var guid = $"00000000-0000-0000-0000-000000000{(i + 1).ToString("D3")}";

			await EnsureUserWithFixedIdAsync(
				userManager,
				roleManager,
				guid,
				$"{role.Name!.ToLower()}_test",
				testUserPassword,
				role);
		}
	}
	private static async Task ClearDataAsync(AppDbContext context)
	{
		// порядок строгий: сначала «дети», потом «родители»
		const string sql = @"
        DELETE FROM [Budget];
        DELETE FROM [ProjectParameters];
        DELETE FROM [Project];
        DELETE FROM [Customer];
        DELETE FROM [Ref_BudgetGroup];
        DELETE FROM [Classifier];
		DELETE FROM [Ref_ProjectPhase];
		DELETE FROM [Ref_ProjectStage];
		DELETE FROM [Ref_ProjectStatus];";

		await context.Database.ExecuteSqlRawAsync(sql);
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
	RoleManager<Role> roleManager,
	string id,          // нужный Id
	string userName,
	string password,
	Role role)
	{
		if(role.Name == null) throw new ArgumentNullException(nameof(role));
		// если роль ещё не создавалась
		if (!await roleManager.RoleExistsAsync(role.Name!))
			await roleManager.CreateAsync(role);

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
		await userManager.AddToRoleAsync(user, role.Name!);
		return user;
	}

}
