using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Tasks;
using Task = TEL_ProjectBus.DAL.Entities.Tasks.Task;

namespace TEL_ProjectBus.DAL.Configurations.Tasks;

public class TaskConfiguration : IEntityTypeConfiguration<Task>
{
	public void Configure(EntityTypeBuilder<Task> builder)
	{
		builder.HasKey(t => t.Id);

		builder.Property(t => t.TaskName)
			   .HasMaxLength(255);

		builder.Property(t => t.TaskDescription) // todo: вопрос по архитектуре. Как указать максимальную длину текста?
			   .HasMaxLength(4000);

		builder.HasOne(t => t.TaskOwner)
			   .WithMany()
			   .HasForeignKey(t => t.TaskOwnerId)
			   .OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(t => t.TaskAuthor)
			   .WithMany()
			   .HasForeignKey(t => t.TaskAuthorId)
			   .OnDelete(DeleteBehavior.Restrict);

		builder.Property(t => t.TaskName).HasColumnName("Task_Name");
		builder.Property(t => t.TaskDescription).HasColumnName("Task_Description");
		builder.Property(t => t.TaskOwnerId).HasColumnName("Task_Owner");
		builder.Property(t => t.TaskAuthorId).HasColumnName("Task_Author");
		builder.Property(t => t.TaskStart).HasColumnName("Task_Start");
		builder.Property(t => t.TaskEnd).HasColumnName("Task_End");
		builder.Property(e => e.ChangedByUserId).HasColumnName("ChangedBy");
		builder.Property(e => e.DateChanged).HasColumnName("DateChanged");
	}
}
