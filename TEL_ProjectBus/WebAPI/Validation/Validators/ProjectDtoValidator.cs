using FluentValidation;
using TEL_ProjectBus.BLL.DTOs;

namespace TEL_ProjectBus.WebAPI.Validation.Validators;

/// <summary>
/// Validates data for <see cref="ProjectDto"/>.
/// </summary>
public class ProjectDtoValidator : AbstractValidator<ProjectDto>
{
	public ProjectDtoValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(200);

		RuleFor(x => x.Code)
			.NotEmpty()
			.MaximumLength(200);
	}
}
