using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.DAL.Configurations;

public class ProjectPortfolioConfiguration : IEntityTypeConfiguration<ProjectPortfolio>
{
	public void Configure(EntityTypeBuilder<ProjectPortfolio> builder)
	{
		builder.ToTable("ProjectPortfolio");

		builder.HasKey(pas => pas.Id);
	}
}
