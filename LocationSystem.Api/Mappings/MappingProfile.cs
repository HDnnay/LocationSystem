using AutoMapper;
using LocationSystem.Application.Features.Menus.Models;
using LocationSystem.Application.Features.Permissions.Models;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 菜单映射
            CreateMap<Menu, MenuDto>()
                .ForMember(dest => dest.ChildMenus, opt => opt.MapFrom(src => src.Children));

            // 权限映射
            CreateMap<Permission, PermissionDto>()
                .ForMember(dest => dest.ChildPermissions, opt => opt.MapFrom(src => src.ChildPermissions));
        }
    }
}