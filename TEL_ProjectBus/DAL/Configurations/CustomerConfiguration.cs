using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TEL_ProjectBus.DAL.Entities.Customers;
using TEL_ProjectBus.DAL.Extensions;

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

		builder.Property(e => e.Name).HasColumnName("Customer_Name");
		builder.Property(e => e.Address).HasColumnName("Customer_Address");
		builder.Property(e => e.CompanyName).HasColumnName("Customer_CompanyName");
		builder.Property(e => e.ContactPerson).HasColumnName("Customer_ContactPerson");
		builder.Property(e => e.Uuid).HasColumnName("Customer_UUID");

		builder.ConfigureIntId();
		//builder.ConfigureAudit();

		builder.Property(p => p.ChangedByUserId).HasColumnName("ChangeBy"); // todo: опечатка в БД
		builder.Property(p => p.DateChanged).HasColumnName("DateChanged");
	}
}
