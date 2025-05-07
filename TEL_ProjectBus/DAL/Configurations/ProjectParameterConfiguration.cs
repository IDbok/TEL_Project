using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.DAL.Configurations;

public class ProjectParameterConfiguration : IEntityTypeConfiguration<ProjectParameter>
{
	public void Configure(EntityTypeBuilder<ProjectParameter> builder)
	{
		builder.ToTable("ProjectParameters");

		builder.HasKey(pp => pp.Id);

		builder.Property(pp => pp.Description).HasMaxLength(4000);

		builder.HasOne(pp => pp.Project)
			   .WithMany(p => p.Parameters)
			   .HasForeignKey(pp => pp.ProjectId)
				.OnDelete(DeleteBehavior.NoAction);
	}
}
