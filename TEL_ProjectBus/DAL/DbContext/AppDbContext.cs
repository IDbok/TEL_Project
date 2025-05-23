using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TEL_ProjectBus.DAL.Entities;
using TEL_ProjectBus.DAL.Entities.Budgets;
using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Entities.Customers;
using TEL_ProjectBus.DAL.Entities.Projects;
using TEL_ProjectBus.DAL.Entities.Reference;
using TEL_ProjectBus.DAL.Entities.Tasks;
using TEL_ProjectBus.DAL.Extensions;
using Task = TEL_ProjectBus.DAL.Entities.Tasks.Task;
using TaskStatus = TEL_ProjectBus.DAL.Entities.Tasks.TaskStatus;

namespace TEL_ProjectBus.DAL.DbContext;
public class AppDbContext : IdentityDbContext<User, Role, string>
{
	public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options)
	{
	}
	public AppDbContext(string connectionString)
		: base(new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlServer(connectionString)
			.Options)
	{
	}

	#region Tables

	// Core entities
	public DbSet<Project> Projects { get; set; }
	public DbSet<Customer> Customers { get; set; }
	public DbSet<Budget> Budgets { get; set; }
	public DbSet<RefreshToken> RefreshTokens { get; set; }

	// Dictionaries
	public DbSet<BudgetGroup> BudgetGroups { get; set; }
	public DbSet<Classifier> Classifiers { get; set; }
	public DbSet<ApproveStatus> ApproveStatuses { get; set; }
	public DbSet<ProjectStage> ProjectStages { get; set; }
	public DbSet<ProjectPhase> ProjectPhases { get; set; }
	public DbSet<ProjectStatus> ProjectStatuses { get; set; }
	public DbSet<RefTaskStatus> RefTaskStatuses { get; set; }

	// Project approval / parameters
	public DbSet<ProjectApproveStatus> ProjectApproveStatuses { get; set; }
	public DbSet<ProjectParameter> ProjectParameters { get; set; }

	// Budget approval / versions
	public DbSet<BudgetApprove> BudgetApproves { get; set; }
	public DbSet<BudgetVersionParameter> BudgetVersionParameters { get; set; }

	// Tasks
	public DbSet<Task> Tasks { get; set; }
	public DbSet<TaskStatus> TaskStatuses { get; set; }
	public DbSet<TaskProgress> TaskProgresses { get; set; }
	public DbSet<TaskParameter> TaskParameters { get; set; }
	public DbSet<TaskAttachment> TaskAttachments { get; set; }

	// Customer extras
	public DbSet<CustomerTeam> CustomerTeams { get; set; }

	#endregion

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

		//var converter = new StringToGuidConverter();

		//foreach (var entityType in modelBuilder.Model.GetEntityTypes()
		//		 .Where(t => typeof(AuditableEntity).IsAssignableFrom(t.ClrType)))
		//{
		//	var prop = entityType.FindProperty(nameof(AuditableEntity.ChangedByUserId));
		//	if (prop is not null)
		//	{
		//		prop.SetValueConverter(converter);           // сам конвертер
		//		prop.SetColumnType("uniqueidentifier");      // чтобы миграция не попыталась изменить тип
		//	}
		//}
	}

	protected override void ConfigureConventions(ModelConfigurationBuilder cb)
	{
		base.ConfigureConventions(cb);

		cb.Properties<ClassifierKey>()
		  .HaveConversion<ClassifierKeyConverter>()
		  .HaveColumnType("int");  
	}

}
