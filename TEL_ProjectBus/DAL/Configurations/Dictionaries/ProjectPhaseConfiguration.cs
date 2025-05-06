using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.DAL.Configurations.Dictionaries;

public class ProjectPhaseConfiguration : IEntityTypeConfiguration<ProjectPhase>
{
	public void Configure(EntityTypeBuilder<ProjectPhase> builder)
	{
		builder.ToTable("Ref_ProjectPhase");

		builder.HasKey(p => p.Id);

	}
}
