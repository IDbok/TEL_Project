using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.BLL.Mappers;

public static class ProjectProfileMapper
{
	public static T ToDto<T>(Project p, ProjectParameter pp)
		where T : ProjectProfileDto, new()
	{
		if (p == null || pp == null) throw new ArgumentNullException("Project or ProjectParameter cannot be null.");

		if (p.Id != pp.ProjectId) throw new ArgumentException("Project ID does not match with ProjectParameter ID.");

		return new T
		{
			Name = p.Name,
			Code = p.Code,
			PreparedBy = "н/д",
			Department = "н/д",
			Customer = p.Customer.ToString() ?? "Н/Д",
			ResponsibleFullName = pp.ProjectOwner.ToString(),
			ContactPhone = pp.ProjectOwner.PhoneNumber ?? "Н/Д",
			DateCreated = p.DateCreated,
			GoalsAndRequirements = "н/д",
			CustomerNeeds = "н/д",
			SuccessCriteria = "н/д",
			HighLevelRisks = "н/д",
			ScheduleAndBudget = "н/д",
			AssumptionsAndConstraints = "н/д",
			ConfidenceRequirements = "н/д",
			ProjectManager = "н/д"
		};
	}
}

