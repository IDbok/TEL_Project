﻿using FluentValidation;
using TEL_ProjectBus.BLL.DTOs;

namespace TEL_ProjectBus.WebAPI.Validation.Validators;

/// <summary>
/// Validates data for <see cref="BudgetLineDto"/>.
/// </summary>
public class BudgetLineDtoValidator : AbstractValidator<BudgetLineDto>
{
	public BudgetLineDtoValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(200);

		RuleFor(x => x.ProjectId)
			.GreaterThan(0);

		RuleFor(x => x.RoleId)
			.GreaterThan(0);

		RuleFor(x => x.BudgetGroup)
			.NotNull();

		RuleFor(x => x.BudgetGroup.Id)
			.GreaterThan(0);

		RuleFor(x => x.ManHoursCost)
			.GreaterThanOrEqualTo(0);

		RuleFor(x => x.Amount)
			.GreaterThanOrEqualTo(0)
			.When(x => x.Amount.HasValue);

		RuleFor(x => x.RgpPercent)
			.InclusiveBetween(0, 100);

		RuleFor(x => x.Probability)
			.InclusiveBetween(0, 100)
			.When(x => x.Probability.HasValue);
	}
}