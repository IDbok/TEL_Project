using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.Common.Extensions;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.BLL.Mappers;

public static class ProjectMapper
{
	public static T ToDto<T>(this Project project)
		where T : ProjectDto, new()
	{
		if (project == null) throw new ArgumentNullException(nameof(project));
		if (project.Parameter == null) throw new ArgumentNullException(nameof(project.Parameter));
		if (project.Customer == null) throw new ArgumentNullException(nameof(project.Customer));
		if (project.Classifier == null) throw new ArgumentNullException(nameof(project.Classifier));

		return new T
		{
			Id = project.Id,
			Name = project.Name,
			Code = project.Code,

			Phase = new ProjectPhaseDto
			{
				Id = project.Parameter.ProjectPhase.ToInt(),
				Name = project.Parameter.ProjectPhase.GetDescription(),
			},

			DateInitiation = project.DateInitiation,
			DateCreated = project.DateCreated,
			DateChanged = project.DateChanged,

			ChangedByUserId = project.ChangedByUserId,
			Classifier = new ClassifierDto
			{
				Id = project.Classifier.Id,
				Code = project.Classifier.Code,
			},
			Customer = new CustomerDto
			{
				Id = project.Customer.Id,
				Name = project.Customer.Name,
				Address = project.Customer.Address,
				CompanyName = project.Customer.CompanyName,
				ContactPerson = project.Customer.ContactPerson,
				Uuid = project.Customer.Uuid
			},
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
