using AutoMapper;
using LocationSystem.Application.Features.Menus.Models;
using LocationSystem.Application.Features.Permissions.Models;
using LocationSystem.Domain.Entities.Menus;
using LocationSystem.Domain.Entities.UserRolePermissions;

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

            // 用户映射
            CreateMap<User, LocationSystem.Application.Features.Users.Models.UserDto>()
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType.ToString()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));

            // 用户映射（Dtos）
            CreateMap<User, LocationSystem.Application.Dtos.UserDto>()
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType.ToString()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));

            // 角色映射
            CreateMap<Role, LocationSystem.Application.Features.Users.Models.RoleDto>();
            CreateMap<Role, LocationSystem.Application.Dtos.RoleDto>();

            // 权限映射
            CreateMap<Permission, LocationSystem.Application.Features.Users.Models.PermissionDto>();
            CreateMap<Permission, LocationSystem.Application.Dtos.PermissionDto>();
        }
    }
}