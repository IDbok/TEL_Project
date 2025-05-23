﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TEL_ProjectBus.DAL.Entities.Projects;
using TEL_ProjectBus.DAL.Extensions;

namespace TEL_ProjectBus.DAL.Configurations.Dictionaries;

public class ProjectStatusConfiguration : IEntityTypeConfiguration<ProjectStatus>
{
	public void Configure(EntityTypeBuilder<ProjectStatus> builder)
	{
		builder.ToTable("Ref_ProjectStatus");

		builder.HasKey(ps => ps.Id);

		builder.Property(ps => ps.Name)
			   .HasMaxLength(100)
			   .IsRequired();

		builder.Property(ps => ps.Name).HasColumnName("StatusName");

		builder.ConfigureIntId();
		builder.ConfigureAudit();
	}
}
