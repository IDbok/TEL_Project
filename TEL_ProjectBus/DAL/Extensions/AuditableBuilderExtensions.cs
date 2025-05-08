using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Extensions;

public static class AuditableBuilderExtensions
{
	public static EntityTypeBuilder<T> ConfigureAudit<T>(this EntityTypeBuilder<T> builder)
		where T : AuditableEntity
	{
		builder.Property(p => p.ChangedByUserId).HasColumnName("ChangedBy");
		builder.Property(p => p.DateChanged).HasColumnName("DateChanged");
		return builder;
	}

	public static EntityTypeBuilder<T> ConfigureId<T, TKey>(this EntityTypeBuilder<T> builder, string conlumnName = "ID")
		where T : class, IHasIdentity<TKey>
		
	{
		builder.Property(p => p.Id).HasColumnName(conlumnName);
		return builder;
	}
	public static EntityTypeBuilder<T> ConfigureIntId<T>(this EntityTypeBuilder<T> builder)
		where T : class, IHasIdentity<int> => builder.ConfigureId<T, int>();
	public static EntityTypeBuilder<T> ConfigureLongId<T>(this EntityTypeBuilder<T> builder)
		where T : class, IHasIdentity<long> => builder.ConfigureId<T, long>();
	public static EntityTypeBuilder<T> ConfigureStringId<T>(this EntityTypeBuilder<T> builder)
		where T : class, IHasIdentity<string> => builder.ConfigureId<T, string>();
}

