using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.DAL.Configurations;

public class ProjectApproveStatusConfiguration : IEntityTypeConfiguration<ProjectApproveStatus>
{
	public void Configure(EntityTypeBuilder<ProjectApproveStatus> builder)
	{
		builder.ToTable("ProjectApproveStatus");

		builder.HasKey(pas => pas.Id);

		builder.HasOne(pas => pas.Project)
			   .WithMany()
			   .HasForeignKey(pas => pas.ProjectId);

		builder.HasOne(pas => pas.ApproveStatus)
			   .WithMany(s => s.ProjectApproveStatuses)
			   .HasForeignKey(pas => pas.ApproveStatusId);

		builder.HasOne(pas => pas.ProjectStage)
			   .WithMany(ps => ps.ProjectApproveStatuses)
			   .HasForeignKey(pas => pas.ProjectStageId);

		builder.HasOne(pas => pas.Approver)
			   .WithMany()
			   .HasForeignKey(pas => pas.ApprovedByUserId)
			   .OnDelete(DeleteBehavior.Restrict);
	}
}