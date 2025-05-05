using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Budgets;

namespace TEL_ProjectBus.DAL.Configurations;

public class BudgetApproveConfiguration : IEntityTypeConfiguration<BudgetApprove>
{
	public void Configure(EntityTypeBuilder<BudgetApprove> builder)
	{
		builder.HasKey(ba => ba.Id);

		builder.Property(ba => ba.Comment).HasMaxLength(4000);

		builder.HasOne(ba => ba.Budget)
			   .WithMany()
			   .HasForeignKey(ba => ba.BudgetId);

		builder.HasOne(ba => ba.ApproveStatus)
			   .WithMany(s => s.BudgetApproves)
			   .HasForeignKey(ba => ba.ApproveStatusId);

		builder.HasOne(ba => ba.User)
			   .WithMany()
			   .HasForeignKey(ba => ba.UserId)
			   .OnDelete(DeleteBehavior.Restrict);
	}
}
