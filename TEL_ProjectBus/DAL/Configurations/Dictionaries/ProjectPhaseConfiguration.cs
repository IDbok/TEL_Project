using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TEL_ProjectBus.DAL.Entities.Projects;
using TEL_ProjectBus.DAL.Extensions;

namespace TEL_ProjectBus.DAL.Configurations.Dictionaries;

public class ProjectPhaseConfiguration : IEntityTypeConfiguration<ProjectPhase>
{
	public void Configure(EntityTypeBuilder<ProjectPhase> builder)
	{
		builder.ToTable("Ref_ProjectPhase");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.PhaseName)
			.HasColumnName("PhaseName");

		builder.ConfigureIntId();
		builder.ConfigureAudit();

	}
}
