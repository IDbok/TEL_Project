using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.Entities.Budgets;
using TEL_ProjectBus.DAL.Extensions;

namespace TEL_ProjectBus.DAL.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
	public void Configure(EntityTypeBuilder<Budget> builder)
	{
		builder.ToTable("Budget");

		builder.HasKey(b => b.Id);

		builder.Property(b => b.Name)
			   .HasMaxLength(255);

		builder.HasOne(p => p.Project)
			   .WithMany(p => p.Budgets)
			   .HasForeignKey(b => b.ProjectId);

		builder.Property(b => b.BudgetGroupId).HasColumnName("ID_BudgetGroup");
		builder.Property(b => b.ProjectId).HasColumnName("ID_Project");
		builder.Property(b => b.ERPId).HasColumnName("Budget_ERP_ID");
		builder.Property(b => b.VisOnPipeline).HasColumnName("VisOnPipeline");
		builder.Property(b => b.Name).HasColumnName("BudgetName");
		builder.Property(b => b.RoleId).HasColumnName("ID_Role");
		builder.Property(b => b.ManHoursCost).HasColumnName("ManHoursCost");
		builder.Property(b => b.Description).HasColumnName("Item_Description");
		builder.Property(b => b.ClassifierId).HasColumnName("ID_Classifier");
		builder.Property(b => b.Amount).HasColumnName("Amount");
		builder.Property(b => b.EC).HasColumnName("EC");
		builder.Property(b => b.RgpPercent).HasColumnName("RGP_Prct");
		builder.Property(b => b.Version).HasColumnName("Budget_Verison");
		builder.Property(b => b.CPTCCPcs).HasColumnName("CP_TCC_Pcs");
		builder.Property(b => b.Probability).HasColumnName("Probability");
		builder.Property(b => b.DatePlan).HasColumnName("Date_Plan");
		builder.Property(b => b.CalcCPTCCPlan).HasColumnName("Calc_CP_TCC_Plan");
		builder.Property(b => b.DateFact).HasColumnName("Date_Fact");
		builder.Property(b => b.CPTCCFact).HasColumnName("CP_TCC_Fact");
		builder.Property(b => b.CalcPriceTCPcs).HasColumnName("Calc_Price_TC_Pcs");
		builder.Property(b => b.CalcPriceTCC).HasColumnName("Calc_Price_TCC");
		builder.Property(b => b.CalcCV).HasColumnName("Calc_CV");
		builder.Property(b => b.CalcSV).HasColumnName("Calc_SV");
		builder.Property(b => b.CalcEV).HasColumnName("Calc_EV");
		builder.Property(b => b.CalcCPI).HasColumnName("Calc_CPI");
		builder.Property(b => b.CalcSPI).HasColumnName("Calc_SPI");


		builder.Property(b => b.Id).HasColumnName("ID_Budget");
		builder.ConfigureAudit();
	}
}
