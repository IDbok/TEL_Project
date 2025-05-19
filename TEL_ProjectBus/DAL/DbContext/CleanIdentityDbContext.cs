using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TEL_ProjectBus.DAL.DbContext;

public class CleanIdentityDbContext : IdentityDbContext
{
	public CleanIdentityDbContext(DbContextOptions<CleanIdentityDbContext> options)
		: base(options) { }

	public CleanIdentityDbContext(string connectionString)
		: base(new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlServer(connectionString)
			.Options)
	{
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);   // никаких дополнительных маппингов
										 // и всё!
	}
}
