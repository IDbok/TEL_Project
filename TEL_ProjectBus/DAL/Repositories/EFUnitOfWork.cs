using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Repositories;

public class EFUnitOfWork : IUnitOfWork
{
	private AppDbContext db;
	//private PhoneRepository phoneRepository;

	public EFUnitOfWork(string connectionString)
	{
		db = new AppDbContext(connectionString);
	}
	//public IRepository<Phone> Phones
	//{
	//	get
	//	{
	//		if (phoneRepository == null)
	//			phoneRepository = new PhoneRepository(db);
	//		return phoneRepository;
	//	}
	//}

	public void Save()
	{
		db.SaveChanges();
	}

	private bool disposed = false;

	public virtual void Dispose(bool disposing)
	{
		if (!this.disposed)
		{
			if (disposing)
			{
				db.Dispose();
			}
			this.disposed = true;
		}
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}


