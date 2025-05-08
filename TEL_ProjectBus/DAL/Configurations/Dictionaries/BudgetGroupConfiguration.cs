using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Budgets;
using TEL_ProjectBus.DAL.Extensions;

namespace TEL_ProjectBus.DAL.Configurations.Dictionaries;

public class BudgetGroupConfiguration : IEntityTypeConfiguration<BudgetGroup>
{
	public void Configure(EntityTypeBuilder<BudgetGroup> builder)
	{
		builder.ToTable("Ref_BudgetGroup");

		builder.HasKey(bg => bg.Id);

		builder.Property(p => p.Name).HasColumnName("BudgetGroup_Name");

		builder.ConfigureIntId();
		builder.ConfigureAudit();
	}
}
