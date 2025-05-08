using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TEL_ProjectBus.DAL.Entities.Projects;
using TEL_ProjectBus.DAL.Extensions;

namespace TEL_ProjectBus.DAL.Configurations;

public class ProjectPortfolioCrossConfiguration : IEntityTypeConfiguration<ProjectPortfolioCross>
{
	public void Configure(EntityTypeBuilder<ProjectPortfolioCross> builder)
	{
		builder.ToTable("Portfolio_Project_Cross");
		builder.HasKey(p => p.Id);

		builder.Property(p => p.ProjectId).HasColumnName("ID_Project");
		builder.Property(p => p.PortfolioId).HasColumnName("ID_Portfolio");

		builder.ConfigureIntId();
		builder.ConfigureAudit();
	}
}
