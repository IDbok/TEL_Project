using FluentValidation;
using TEL_ProjectBus.WebAPI.Messages.Commands.Budgets;

namespace TEL_ProjectBus.WebAPI.Validation.Validators;

public class UpdateBudgetCommandValidator : AbstractValidator<UpdateBudgetCommand>
{
	public UpdateBudgetCommandValidator()
	{
		Include(new BudgetLineDtoValidator());
	}
}
