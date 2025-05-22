using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Projects;
using TEL_ProjectBus.DAL.Extensions;

namespace TEL_ProjectBus.DAL.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
	public void Configure(EntityTypeBuilder<Project> builder)
	{
		builder.ToTable("Project");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Name)
			   .HasMaxLength(100)
			   .IsRequired();

		builder.Property(p => p.Code)
			   .HasMaxLength(200);

		builder.HasOne(e => e.Customer)
			  .WithMany(c => c.Projects)
			  .HasForeignKey(p => p.CustomerId);

		builder.Property(p => p.Name).HasColumnName("ProjectName");
		builder.Property(p => p.Code).HasColumnName("ProjectCode");
		builder.Property(p => p.DateInitiation).HasColumnName("DateInitiation");
		builder.Property(p => p.ClassifierId).HasColumnName("ID_Classifier");
		builder.Property(p => p.CustomerId).HasColumnName("ID_Customer");
		builder.Property(p => p.DateCreated).HasColumnName("DateCreated");

		builder.Property(p => p.Id).HasColumnName("ID_Project");
		builder.ConfigureAudit();

	}
}