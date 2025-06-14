﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Reference;
using TEL_ProjectBus.DAL.Extensions;
using TEL_ProjectBus.DAL.Entities.Common;

namespace TEL_ProjectBus.DAL.Configurations.Dictionaries;

public class ClassifierConfiguration : IEntityTypeConfiguration<Classifier>
{
	public void Configure(EntityTypeBuilder<Classifier> builder)
	{
		builder.ToTable("Classifier");
		builder.HasKey(c => c.Id);

		builder.Property(p => p.Code).HasColumnName("ClassifierCode");

		builder.ConfigureId<Classifier, ClassifierKey>();
		builder.ConfigureAudit();
	}
}
