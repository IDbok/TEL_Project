using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TEL_ProjectBus.DAL.Entities.Projects;
using TEL_ProjectBus.DAL.Extensions;

namespace TEL_ProjectBus.DAL.Configurations;

public class ProjectPortfolioConfiguration : IEntityTypeConfiguration<ProjectPortfolio>
{
	public void Configure(EntityTypeBuilder<ProjectPortfolio> builder)
	{
		builder.ToTable("ProjectPortfolio");

		builder.HasKey(pas => pas.Id);

		builder.Property(p => p.Name).HasColumnName("PortfolioName");
		builder.Property(p => p.Description).HasColumnName("Portfolio_Description");

		builder.ConfigureIntId();
		builder.ConfigureAudit();
	}
}
