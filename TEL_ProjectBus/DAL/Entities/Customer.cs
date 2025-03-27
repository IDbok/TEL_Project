namespace TEL_ProjectBus.DAL.Entities;

public class Customer
{
	public Guid Id { get; set; }
	public string CustomerName { get; set; }
	public string Address { get; set; }
	public string CompanyName { get; set; }
	public string ContactPerson { get; set; }
	public DateTime DateCreated { get; set; }
	public DateTime DateChanged { get; set; }

	public ICollection<Project> Projects { get; set; }
}

