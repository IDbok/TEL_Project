using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TEL_ProjectBus.DAL.Entities.Budgets;

namespace TEL_ProjectBus.DAL.Configurations;

public class ProjectVOConfiguration : IEntityTypeConfiguration<ProjectVO>
{
	public void Configure(EntityTypeBuilder<ProjectVO> builder)
	{
		builder.ToTable("ProjectVO");

		builder.HasKey(x => x.Id);

	}
}
