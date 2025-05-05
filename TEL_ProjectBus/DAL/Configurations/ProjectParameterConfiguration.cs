using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.DAL.Configurations;

public class ProjectParameterConfiguration : IEntityTypeConfiguration<ProjectParameter>
{
	public void Configure(EntityTypeBuilder<ProjectParameter> builder)
	{
		builder.HasKey(pp => pp.Id);

		builder.Property(pp => pp.ProjectDescription).HasMaxLength(4000);

		builder.HasOne(pp => pp.Project)
			   .WithMany()
			   .HasForeignKey(pp => pp.ProjectId);
	}
}
