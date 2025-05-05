namespace TEL_ProjectBus.DAL.Entities.Projects;

public class ProjectApproverTemplate
{
	public Guid Id { get; set; }
	public int ProjectStageId { get; set; }
	public Guid? RoleId { get; set; } // для IdentityRole.Id
	public Guid? UserId { get; set; }

	public ProjectStage ProjectStage { get; set; } = null!;
	public User? User { get; set; }
}
