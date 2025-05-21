using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Projects;
using TEL_ProjectBus.DAL.Extensions;

namespace TEL_ProjectBus.DAL.Configurations;

public class ProjectApproveStatusConfiguration : IEntityTypeConfiguration<ProjectApproveStatus>
{
	public void Configure(EntityTypeBuilder<ProjectApproveStatus> builder)
	{
		builder.ToTable("ProjectApproveStatus");

		builder.HasKey(pas => pas.Id);

		builder.HasOne(pas => pas.Project)
			   .WithMany(p => p.ApproveStatuses)
			   .HasForeignKey(pas => pas.ProjectId);

		builder.HasOne(pas => pas.ApproveStatus)
			   .WithMany(s => s.ProjectApproveStatuses)
			   .HasForeignKey(pas => pas.ApproveStatusId);

		//builder.HasOne(pas => pas.ProjectStage) // убираю для избавления от лишних связей с enum сущностями
		//	   .WithMany(ps => ps.ProjectApproveStatuses)
		//	   .HasForeignKey(pas => pas.ProjectStageId);

		builder.HasOne(pas => pas.Approver)
			   .WithMany()
			   .HasForeignKey(pas => pas.ApprovedByUserId)
			   .OnDelete(DeleteBehavior.Restrict);

		builder.Property(pas => pas.ProjectId).HasColumnName("ID_Project");
		builder.Property(pas => pas.ApproveStatusId).HasColumnName("ID_ApproveStatus");
		builder.Property(pas => pas.ApprovedByUserId).HasColumnName("ApprovedBy");
		builder.Property(pas => pas.ProjectStageId).HasColumnName("ID_ProjectStage");

		builder.ConfigureLongId();
		builder.ConfigureAudit();
	}
}