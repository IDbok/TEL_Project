using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.DAL.Configurations.Dictionaries;

public class ProjectStatusConfiguration : IEntityTypeConfiguration<ProjectStatus>
{
	public void Configure(EntityTypeBuilder<ProjectStatus> builder)
	{
		builder.ToTable("Ref_ProjectStatus");

		builder.HasKey(ps => ps.Id);

		builder.Property(ps => ps.StatusName)
			   .HasMaxLength(100)
			   .IsRequired();

		builder.Property(ps => ps.StatusName).HasColumnName("ProjectStatus_Name");
		builder.Property(ps => ps.DateChanged).HasColumnName("DateChanged");
		builder.Property(ps => ps.ChangedByUserId).HasColumnName("ChangedBy");
	}
}
