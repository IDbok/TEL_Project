using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Budgets;

namespace TEL_ProjectBus.DAL.Configurations;

public class BudgetVersionParameterConfiguration : IEntityTypeConfiguration<BudgetVersionParameter>
{
	public void Configure(EntityTypeBuilder<BudgetVersionParameter> builder)
	{
		builder.HasKey(bvp => bvp.Id);

		builder.Property(bvp => bvp.VersionName).HasMaxLength(100);

		builder.HasOne(bvp => bvp.Budget)
			   .WithMany()
			   .HasForeignKey(bvp => bvp.BudgetId);
	}
}
