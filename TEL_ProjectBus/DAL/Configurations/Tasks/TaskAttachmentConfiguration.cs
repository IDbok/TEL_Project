using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Tasks;

namespace TEL_ProjectBus.DAL.Configurations.Tasks;

public class TaskAttachmentConfiguration : IEntityTypeConfiguration<TaskAttachment>
{
	public void Configure(EntityTypeBuilder<TaskAttachment> builder)
	{
		builder.HasKey(ta => ta.Id);

		builder.Property(ta => ta.Description)
			.HasMaxLength(255);

		builder.Property(ta => ta.SourceOfFile)
			.HasMaxLength(255);

		builder.Property(ta => ta.FileName)
			.HasMaxLength(255);

		builder.Property(ta => ta.FileFormat)
			.HasMaxLength(100);

		builder.HasOne(ta => ta.Task)
			   .WithMany(t => t.Attachments)
			   .HasForeignKey(ta => ta.TaskId);

		builder.HasOne(ta => ta.ChangedByUser)
			.WithMany()
			.HasForeignKey(ta => ta.ChangedByUserId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.Property(e => e.TaskId).HasColumnName("ID_Tasks");
		builder.Property(e => e.Content).HasColumnName("Content");
		builder.Property(e => e.Description).HasColumnName("Description");
		builder.Property(e => e.SourceOfFile).HasColumnName("SourceOfFile");
		builder.Property(e => e.FileName).HasColumnName("File_Name");
		builder.Property(e => e.FileFormat).HasColumnName("File_Format");
		builder.Property(e => e.FileVersion).HasColumnName("File_Version");
		builder.Property(e => e.ChangedByUserId).HasColumnName("ChangedBy");
		builder.Property(e => e.DateChanged).HasColumnName("DateChanged");

	}
}
