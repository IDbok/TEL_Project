using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TEL_ProjectBus.DAL.Entities.Budgets;

namespace TEL_ProjectBus.DAL.Configurations;

public class ProjectVOBudgetCrossConfiguration : IEntityTypeConfiguration<ProjectVOBudgetCross>
{
	public void Configure(EntityTypeBuilder<ProjectVOBudgetCross> builder)
	{
		builder.ToTable("ProjectVO_Budget_Cross");

		builder.HasKey(pvbc => pvbc.Id);
	}
}
