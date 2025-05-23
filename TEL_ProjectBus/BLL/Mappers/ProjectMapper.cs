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
			ClassifierId = projectDto.ClassifierCode,
			CustomerId = 1, // todo: get from customer
			DateInitiation = projectDto.DateInitiation,
			DateCreated = projectDto.DateCreated,
			ChangedByUserId = "00000000-0000-0000-0000-000000000002", // pm // todo: get from user

		};
	}
}
