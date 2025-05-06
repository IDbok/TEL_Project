using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.DAL.Configurations;

public class ProjectPortfolioCrossConfiguration : IEntityTypeConfiguration<ProjectPortfolioCross>
{
	public void Configure(EntityTypeBuilder<ProjectPortfolioCross> builder)
	{
		builder.ToTable("Portfolio_Project_Cross");
		builder.HasKey(p => p.Id);
	}
}
