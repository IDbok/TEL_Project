using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.DAL.Configurations;

public class ProjectApproverTemplateConfiguration : IEntityTypeConfiguration<ProjectApproverTemplate>
{
	public void Configure(EntityTypeBuilder<ProjectApproverTemplate> builder)
	{
		builder.HasKey(pat => pat.Id);

		builder.HasOne(pat => pat.ProjectStage)
			   .WithMany(ps => ps.Templates)
			   .HasForeignKey(pat => pat.ProjectStageId);

		builder.HasOne(pat => pat.User)
			   .WithMany()
			   .HasForeignKey(pat => pat.UserId)
			   .OnDelete(DeleteBehavior.Restrict);
	}
}
