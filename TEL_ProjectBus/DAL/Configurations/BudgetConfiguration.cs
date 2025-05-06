using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Budgets;

namespace TEL_ProjectBus.DAL.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
	public void Configure(EntityTypeBuilder<Budget> builder)
	{
		builder.ToTable("Budget");

		builder.HasKey(b => b.Id);

		builder.Property(b => b.Name)
			   .HasMaxLength(255);

		builder.HasOne(p => p.Project)
			   .WithMany()
			   .HasForeignKey(b => b.ProjectId);
	}
}
