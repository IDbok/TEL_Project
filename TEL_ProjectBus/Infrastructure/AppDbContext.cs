using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities;

namespace Infrastructure;
public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options)
	{
	}
	// DbSets
	public DbSet<Project> Projects { get; set; }
	public DbSet<Customer> Customers { get; set; }
	public DbSet<Budget> Budgets { get; set; }
	public DbSet<BudgetItem> BudgetItems { get; set; }
	public DbSet<Expense> Expenses { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Настройка сущности Project
		modelBuilder.Entity<Project>(entity =>
		{
			entity.HasKey(e => e.Id);

			entity.Property(e => e.ProjectName).HasMaxLength(100);
			entity.Property(e => e.ProjectCode).HasMaxLength(200);
			entity.Property(e => e.DateInitiation);

			entity.HasMany(e => e.Budgets)
				.WithOne()
				.HasForeignKey(b => b.ProjectId);
		});

		// Настройка сущности Customer
		modelBuilder.Entity<Customer>(entity =>
		{
			entity.HasKey(e => e.Id);

			entity.Property(e => e.CustomerName).HasMaxLength(100);
			entity.Property(e => e.Address).HasMaxLength(255);
			entity.Property(e => e.CompanyName).HasMaxLength(100);
			entity.Property(e => e.ContactPerson).HasMaxLength(255);

			entity.HasMany(e => e.Projects)
				.WithOne()
				.HasForeignKey(p => p.CustomerId);
		});

		// Настройка сущности Budget
		modelBuilder.Entity<Budget>(entity =>
		{
			entity.HasKey(e => e.Id);

			entity.Property(e => e.BudgetName).HasMaxLength(255);

			entity.HasMany(e => e.Expenses)
				.WithOne(e => e.Budget)
				.HasForeignKey(e => e.BudgetId);

			entity.HasMany<BudgetItem>()
				.WithOne(bi => bi.Budget)
				.HasForeignKey(bi => bi.BudgetId);
		});

		// Настройка сущности BudgetItem
		modelBuilder.Entity<BudgetItem>(entity =>
		{
			entity.HasKey(e => e.Id);

			entity.Property(e => e.Amount).HasColumnType("decimal(19, 0)");
			entity.Property(e => e.DatePlanned);
		});

		// Настройка сущности Expense
		modelBuilder.Entity<Expense>(entity =>
		{
			entity.HasKey(e => e.Id);

			entity.Property(e => e.Amount).HasColumnType("decimal(19, 0)");
			entity.Property(e => e.Description).HasMaxLength(255);
			entity.Property(e => e.RgpPercent).HasColumnType("decimal(19, 0)");
		});
	}
}
