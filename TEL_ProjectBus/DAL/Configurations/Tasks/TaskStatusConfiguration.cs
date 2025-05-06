using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using TaskStatus = TEL_ProjectBus.DAL.Entities.Tasks.TaskStatus;

namespace TEL_ProjectBus.DAL.Configurations.Tasks;

public class TaskStatusConfiguration : IEntityTypeConfiguration<TaskStatus>
{
	public void Configure(EntityTypeBuilder<TaskStatus> builder)
	{
		builder.ToTable("TaskStatus");

		builder.HasKey(ts => ts.Id);

		builder.HasOne(ts => ts.Task)
			   .WithMany(t => t.Statuses)
			   .HasForeignKey(ts => ts.TaskId);

		builder.HasOne(ts => ts.Status)
			   .WithMany()
			   .HasForeignKey(ts => ts.StatusId);

		builder.HasOne(ta => ta.ChangedByUser)
			.WithMany()
			.HasForeignKey(ta => ta.ChangedByUserId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.Property(e => e.TaskId).HasColumnName("ID_Tasks");
		builder.Property(e => e.StatusId).HasColumnName("ID_TaskStatus");
		builder.Property(e => e.DateChanged).HasColumnName("DateChanged");
		builder.Property(e => e.ChangedByUserId).HasColumnName("ChangedBy");
	}
}
