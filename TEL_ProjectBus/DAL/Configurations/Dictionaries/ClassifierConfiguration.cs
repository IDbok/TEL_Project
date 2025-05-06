using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Reference;

namespace TEL_ProjectBus.DAL.Configurations.Dictionaries;

public class ClassifierConfiguration : IEntityTypeConfiguration<Classifier>
{
	public void Configure(EntityTypeBuilder<Classifier> builder)
	{
		builder.ToTable("Classifier");
		builder.HasKey(c => c.Id);
	}
}
