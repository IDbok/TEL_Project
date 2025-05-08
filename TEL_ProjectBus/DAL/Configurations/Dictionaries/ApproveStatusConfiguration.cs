using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Reference;
using TEL_ProjectBus.DAL.Extensions;

namespace TEL_ProjectBus.DAL.Configurations.Dictionaries;

public class ApproveStatusConfiguration : IEntityTypeConfiguration<ApproveStatus>
{

	public void Configure(EntityTypeBuilder<ApproveStatus> builder)
	{
		builder.ToTable("Ref_ApproveStatus");

		builder.HasKey(a => a.Id);

		builder.Property(p => p.StatusName).HasColumnName("StatusName");

		builder.ConfigureIntId();
		builder.ConfigureAudit();
	}

}
