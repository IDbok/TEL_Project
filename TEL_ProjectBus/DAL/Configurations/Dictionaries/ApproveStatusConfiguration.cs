using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Reference;

namespace TEL_ProjectBus.DAL.Configurations.Dictionaries;

public class ApproveStatusConfiguration : IEntityTypeConfiguration<ApproveStatus>
{
	public void Configure(EntityTypeBuilder<ApproveStatus> builder) => builder.HasKey(a => a.Id);
}
