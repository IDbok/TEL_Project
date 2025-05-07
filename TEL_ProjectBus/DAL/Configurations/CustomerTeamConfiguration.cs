using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Customers;

namespace TEL_ProjectBus.DAL.Configurations;

public class CustomerTeamConfiguration : IEntityTypeConfiguration<CustomerTeam>
{
	public void Configure(EntityTypeBuilder<CustomerTeam> builder)
	{
		builder.ToTable("Customer_Team");

		builder.HasKey(ct => ct.Id);

		builder.Property(ct => ct.CustomerMemberRole)
			.HasMaxLength(100);

		builder.Property(ct => ct.CustomerTeamMemberName)
			.HasMaxLength(255);

		builder.Property(ct => ct.Phone)
			.HasMaxLength(255);

		builder.Property(ct => ct.Email)
			.HasMaxLength(255);

		builder.HasOne(ct => ct.Customer)
			   .WithMany(c => c.Teams)
			   .HasForeignKey(ct => ct.CustomerId);
	}
}
