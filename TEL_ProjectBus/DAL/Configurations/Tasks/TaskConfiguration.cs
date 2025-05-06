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

		builder.Property(t => t.Name)
			   .HasMaxLength(255);

		builder.Property(t => t.Description) // todo: вопрос по архитектуре. Как указать максимальную длину текста?
			   .HasMaxLength(4000);

		builder.HasOne(t => t.Owner)
			   .WithMany()
			   .HasForeignKey(t => t.OwnerId)
			   .OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(t => t.Author)
			   .WithMany()
			   .HasForeignKey(t => t.AuthorId)
			   .OnDelete(DeleteBehavior.Restrict);

		builder.Property(t => t.Name).HasColumnName("Task_Name");
		builder.Property(t => t.Description).HasColumnName("Task_Description");
		builder.Property(t => t.OwnerId).HasColumnName("Task_Owner");
		builder.Property(t => t.AuthorId).HasColumnName("Task_Author");
		builder.Property(t => t.Start).HasColumnName("Task_Start");
		builder.Property(t => t.End).HasColumnName("Task_End");
		builder.Property(e => e.ChangedByUserId).HasColumnName("ChangedBy");
		builder.Property(e => e.DateChanged).HasColumnName("DateChanged");
	}
}
