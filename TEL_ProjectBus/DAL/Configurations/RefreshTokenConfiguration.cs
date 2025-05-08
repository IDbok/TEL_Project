using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities;

namespace TEL_ProjectBus.DAL.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
	public void Configure(EntityTypeBuilder<RefreshToken> builder)
	{
		builder.HasKey(r => r.Id);

		builder.Property(r => r.Token)
			   .IsRequired()
			   .HasMaxLength(1000);
	}
}
