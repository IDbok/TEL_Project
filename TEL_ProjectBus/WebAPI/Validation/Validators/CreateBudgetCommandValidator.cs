using FluentValidation;
using TEL_ProjectBus.WebAPI.Messages.Commands.Budgets;

namespace TEL_ProjectBus.WebAPI.Validation.Validators;

/// <summary>
/// Validates data for <see cref="CreateBudgetCommand"/>.
/// </summary>
public class CreateBudgetCommandValidator : AbstractValidator<CreateBudgetCommand>
{
	public CreateBudgetCommandValidator()
	{
		Include(new BudgetLineDtoValidator());

		// отдельно можно добавить правила для CreateBudgetCommand, если нужно
		//RuleFor(x => x.DateUpdate)                           
		//	.NotEmpty()
		//	.LessThanOrEqualTo(DateTime.UtcNow)              
		//	.WithMessage("DateUpdate must be in the past.");
	}
}
