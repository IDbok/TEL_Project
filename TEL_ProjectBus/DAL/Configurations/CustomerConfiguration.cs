using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TEL_ProjectBus.DAL.Entities.Customers;

namespace TEL_ProjectBus.DAL.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
	public void Configure(EntityTypeBuilder<Customer> builder)
	{
		builder.HasKey(e => e.Id);
		builder.Property(e => e.CustomerName)
			.HasMaxLength(100);

		builder.Property(e => e.Address)
			.HasMaxLength(255);

		builder.Property(e => e.CompanyName)
			.HasMaxLength(100);

		builder.Property(e => e.ContactPerson)
			.HasMaxLength(255);

		builder.HasMany(e => e.Projects)
			  .WithOne()
			  .HasForeignKey(p => p.CustomerId);
	}
}
