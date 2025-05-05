namespace TEL_ProjectBus.DAL.Interfaces;

// Ensure the IUnitOfWork interface is defined in the correct namespace
public interface IUnitOfWork : IDisposable
{
	void Save();
}
