using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Tasks;

namespace TEL_ProjectBus.DAL.Configurations.Dictionaries;

public class RefTaskStatusConfiguration : IEntityTypeConfiguration<RefTaskStatus>
{
	public void Configure(EntityTypeBuilder<RefTaskStatus> builder)
	{
		builder.ToTable("Ref_TaskStatus");

		builder.HasKey(rs => rs.Id);

		builder.Property(rs => rs.TaskStatusName)
			   .HasMaxLength(100)
			   .IsRequired();

		builder.Property(rs => rs.TaskStatusName).HasColumnName("TaskStatus_Name");
		builder.Property(rs => rs.DateChanged).HasColumnName("DateChanged");
		builder.Property(rs => rs.ChangedByUserId).HasColumnName("ChangedBy");
	} 
}
