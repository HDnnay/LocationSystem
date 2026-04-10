using AutoMapper;
using LocationSystem.Application.Dtos;
using LocationSystem.Domain.Entities.Articles;
using LocationSystem.Domain.Entities.Menus;
using LocationSystem.Domain.Entities.UserRolePermissions;

namespace LocationSystem.Application.Mappings
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
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType.ToString()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));

            // 角色映射
            CreateMap<Role, RoleDto>();

            // 权限映射
            CreateMap<Permission, PermissionDto>();

            // 文章映射
            CreateMap<Article, ArticleDto>();
            CreateMap<ArticleTag, TagDto>();
            CreateMap<ArticleComment, ArticleCommentDto>();
        }
    }
}