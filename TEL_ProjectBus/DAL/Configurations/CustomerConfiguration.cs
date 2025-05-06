using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TEL_ProjectBus.DAL.Entities.Customers;

namespace TEL_ProjectBus.DAL.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
	public void Configure(EntityTypeBuilder<Customer> builder)
	{
		builder.ToTable("Customer");

		builder.HasKey(e => e.Id);
		builder.Property(e => e.Name)
			.HasMaxLength(100);

		builder.Property(e => e.Address)
			.HasMaxLength(255);

		builder.Property(e => e.CompanyName)
			.HasMaxLength(100);

		builder.Property(e => e.ContactPerson)
			.HasMaxLength(255);
	}
}
