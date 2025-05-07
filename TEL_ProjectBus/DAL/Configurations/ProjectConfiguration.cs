using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.DAL.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
	public void Configure(EntityTypeBuilder<Project> builder)
	{
		builder.ToTable("Project");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.ProjectName)
			   .HasMaxLength(100)
			   .IsRequired();

		builder.Property(p => p.ProjectCode)
			   .HasMaxLength(200);

		builder.HasOne(e => e.Customer)
			  .WithMany(c => c.Projects)
			  .HasForeignKey(p => p.CustomerId);
	}
}