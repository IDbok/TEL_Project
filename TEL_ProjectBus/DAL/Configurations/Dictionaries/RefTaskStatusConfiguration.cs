﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Tasks;
using TEL_ProjectBus.DAL.Extensions;

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

		builder.ConfigureIntId();
		builder.ConfigureAudit();
	}
}
