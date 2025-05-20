using AutoMapper;
using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.DAL.Entities.Budgets;

namespace TEL_ProjectBus.BLL.Mappers.MappingProfiles;

public class BudgetProfile : Profile
{
	public BudgetProfile()
	{
		// Entity → DTO
		CreateMap<Budget, BudgetLineDto>()
			.ForMember(d => d.BudgetGroup,
					   o => o.MapFrom(s => new BudgetGroupDto
					   {
						   Id = s.BudgetGroupId,
						   Name = s.BudgetGroup.Name
					   }));

		// DTO → Entity
		CreateMap<BudgetLineDto, Budget>()
			.ForMember(d => d.BudgetGroupId, o => o.MapFrom(s => s.BudgetGroup.Id))
			.ForMember(d => d.BudgetGroup, o => o.Ignore());   // EF сам подтянет
	}
}
