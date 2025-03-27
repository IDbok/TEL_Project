using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL;
public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options)
	{
	}

	public DbSet<BudgetItemDto> BudgetItems => Set<BudgetItemDto>();
	public DbSet<BudgetLineDto> BudgetLines => Set<BudgetLineDto>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Пример Fluent-конфигурации
		// Указываем, что у BudgetLine есть внешний ключ BudgetItemId -> BudgetItem.Id
		modelBuilder.Entity<BudgetLineDto>()
			.HasOne(bl => bl.BudgetItem)
			.WithMany(bi => bi.BudgetLines)
			.HasForeignKey(bl => bl.BudgetItemId)
			.OnDelete(DeleteBehavior.Cascade);

		// Можно добавить индексы, ограничения, и т. п.
	}
}
