using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TEL_ProjectBus.DAL.Entities.Budgets;
using TEL_ProjectBus.DAL.Extensions;

namespace TEL_ProjectBus.DAL.Configurations;

public class ProjectVOConfiguration : IEntityTypeConfiguration<ProjectVO>
{
	public void Configure(EntityTypeBuilder<ProjectVO> builder)
	{
		builder.ToTable("ProjectVO");

		builder.HasKey(x => x.Id);

		builder.Property(p => p.Name).HasColumnName("VO_Name");
		builder.Property(p => p.Description).HasColumnName("VO_Description");

		builder.ConfigureIntId();
		builder.ConfigureAudit();
	}
}
