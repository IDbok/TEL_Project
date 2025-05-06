using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Tasks;

namespace TEL_ProjectBus.DAL.Configurations.Tasks;

public class TaskProgressConfiguration : IEntityTypeConfiguration<TaskProgress>
{
	public void Configure(EntityTypeBuilder<TaskProgress> builder)
	{
		builder.ToTable("TaskProgress");

		builder.HasKey(tp => tp.Id);

		builder.HasOne(tp => tp.Task)
			   .WithMany(t => t.Progresses)
			   .HasForeignKey(tp => tp.TaskId);

		builder.HasOne(ta => ta.ChangedByUser)
			.WithMany()
			.HasForeignKey(ta => ta.ChangedByUserId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.Property(e => e.TaskId).HasColumnName("ID_Tasks");
		builder.Property(e => e.ProgressPercentage).HasColumnName("ProgressPercentage");
		builder.Property(e => e.DateChanged).HasColumnName("DateChanged");
		builder.Property(e => e.ChangedByUserId).HasColumnName("ChangedBy");
	}
}
