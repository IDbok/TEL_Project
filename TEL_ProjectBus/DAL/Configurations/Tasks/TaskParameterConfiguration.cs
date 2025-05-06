using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Tasks;

namespace TEL_ProjectBus.DAL.Configurations.Tasks;

public class TaskParameterConfiguration : IEntityTypeConfiguration<TaskParameter>
{
	public void Configure(EntityTypeBuilder<TaskParameter> builder)
	{
		builder.ToTable("TaskParameters");

		builder.HasKey(t => t.Id);

		builder.HasOne(tp => tp.Task)
			   .WithMany(t => t.Parameters)
			   .HasForeignKey(tp => tp.TaskId);

		builder.HasOne(tp => tp.ChangedByUser)
			.WithMany()
			.HasForeignKey(tp => tp.ChangedByUserId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.Property(e => e.TaskId).HasColumnName("ID_Tasks");
		builder.Property(e => e.ProjectId).HasColumnName("ID_Project");
		builder.Property(e => e.HumanHoursResource).HasColumnName("HumanHoursResource");
		builder.Property(e => e.DateChanged).HasColumnName("DateChanged");
		builder.Property(e => e.ChangedByUserId).HasColumnName("ChangedBy");
	}
}
