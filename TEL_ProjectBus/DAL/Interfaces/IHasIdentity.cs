namespace TEL_ProjectBus.DAL.Interfaces;

public interface IHasIdentity<TId>
{
	TId Id { get; set; }
}

