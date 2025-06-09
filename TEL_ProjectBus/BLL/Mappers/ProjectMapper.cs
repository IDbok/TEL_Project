using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.BLL.Mappers;

public static class ProjectMapper
{
	public static T ToDto<T>(this Project project)
		where T : ProjectDto, new()
	{
		if (project == null) throw new ArgumentNullException(nameof(project));
		return new T
		{
			Name = project.Name,
			Code = project.Code,
		};
	}

	public static Project ToEntity<T>(this T projectDto)
		where T : ProjectDto
	{
		if (projectDto == null) throw new ArgumentNullException(nameof(projectDto));
		return new Project
		{
			Name = projectDto.Name,
			Code = projectDto.Code,
			ClassifierId = projectDto.Classifier.Id,
			CustomerId = projectDto.Customer.Id,
			DateInitiation = projectDto.DateInitiation,
			DateCreated = projectDto.DateCreated,
			ChangedByUserId = projectDto.ChangedByUserId,
			DateChanged = projectDto.DateChanged,
		};
	}
}
