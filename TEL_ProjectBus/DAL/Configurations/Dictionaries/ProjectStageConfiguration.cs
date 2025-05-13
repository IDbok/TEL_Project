using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Projects;
using TEL_ProjectBus.DAL.Extensions;

namespace TEL_ProjectBus.DAL.Configurations.Dictionaries;

public class ProjectStageConfiguration : IEntityTypeConfiguration<ProjectStage>
{
	public void Configure(EntityTypeBuilder<ProjectStage> builder)
	{
		builder.ToTable("Ref_ProjectStage");

		builder.HasKey(ps => ps.Id);

		builder.Property(ps => ps.Name)
			.HasColumnName("StageName");

		builder.ConfigureIntId();
		builder.ConfigureAudit();
	}
}
