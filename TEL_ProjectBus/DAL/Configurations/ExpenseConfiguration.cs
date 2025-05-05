using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Budgets;

namespace TEL_ProjectBus.DAL.Configurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
	public void Configure(EntityTypeBuilder<Expense> builder)
	{
		builder.HasKey(e => e.Id);

		builder.Property(e => e.Amount)
			.HasColumnType("decimal(19,0)");

		builder.Property(e => e.RgpPercent)
			.HasColumnType("decimal(19,0)");

		builder.Property(e => e.Description)
			.HasMaxLength(255);

		builder.HasOne(e => e.Budget)
			   .WithMany(b => b.Expenses)
			   .HasForeignKey(e => e.BudgetId);
	}
}
