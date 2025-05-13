using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TEL_ProjectBus.DAL.Entities.Projects;
using TEL_ProjectBus.DAL.Extensions;

namespace TEL_ProjectBus.DAL.Configurations;

public class ProjectParameterConfiguration : IEntityTypeConfiguration<ProjectParameter>
{
	public void Configure(EntityTypeBuilder<ProjectParameter> builder)
	{
		builder.ToTable("ProjectParameters");

		builder.HasKey(pp => pp.Id);

		builder.Property(pp => pp.Description).HasMaxLength(4000);

		builder.HasOne(pp => pp.Project)
			   .WithMany(p => p.Parameters)
			   .HasForeignKey(pp => pp.ProjectId)
				.OnDelete(DeleteBehavior.NoAction);

		builder.HasOne(pp => pp.Classifier)
			   .WithMany(c => c.ProjectParameters)
			   .HasForeignKey(pp => pp.ClassifierId)
				.OnDelete(DeleteBehavior.NoAction);

		builder.Property(p => p.ProjectId).HasColumnName("ID_Project");
		builder.Property(p => p.ProjectOwnerId).HasColumnName("ProjectOwner");
		builder.Property(p => p.ClassifierId).HasColumnName("ID_Classifier");
		//builder.Property(p => p.ProjectPhaseId).HasColumnName("ID_ProjectPhase");
		//builder.Property(p => p.ProjectStageId).HasColumnName("ID_ProjectStage");
		//builder.Property(p => p.ProjectStatusId).HasColumnName("ID_ProjectStatus");
		builder.Property(p => p.ProjectPhase).HasColumnName("ID_ProjectPhase");
		builder.Property(p => p.ProjectStage).HasColumnName("ID_ProjectStage");
		builder.Property(p => p.ProjectStatus).HasColumnName("ID_ProjectStatus");
		builder.Property(p => p.ProjectBegin).HasColumnName("ProjectBegin");
		builder.Property(p => p.ProjectEnd).HasColumnName("ProjectEnd");

		builder.ConfigureLongId();
		builder.ConfigureAudit();
	}
}
