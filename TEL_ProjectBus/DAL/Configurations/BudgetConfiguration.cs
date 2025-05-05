using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Budgets;

namespace TEL_ProjectBus.DAL.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
	public void Configure(EntityTypeBuilder<Budget> builder)
	{
		builder.HasKey(b => b.Id);

		builder.Property(b => b.BudgetName)
			   .HasMaxLength(255);

		builder.HasMany(b => b.Expenses)
			   .WithOne(e => e.Budget)
			   .HasForeignKey(e => e.BudgetId);
	}
}
