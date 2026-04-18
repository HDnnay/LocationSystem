using Mapster;
using LocationSystem.Domain.Entities;
using LocationSystem.Domain.Entities.Articles;
using LocationSystem.Domain.Entities.Menus;
using LocationSystem.Domain.Entities.UserRolePermissions;
using LocationSystem.Application.Dtos.RentHouses;
using LocationSystem.Application.Dtos.Articles;
using LocationSystem.Application.Dtos.Menus;
using LocationSystem.Application.Dtos.Permissions;
using LocationSystem.Application.Dtos.Roles;
using LocationSystem.Application.Dtos.Users;

namespace LocationSystem.Application.Mappings
{
    public class MappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // 菜单映射
            config.NewConfig<Menu, MenuDto>()
                .Map(dest => dest.ChildMenus, src => src.Children);

            // 权限映射
            config.NewConfig<Permission, PermissionDto>()
                .Map(dest => dest.ChildPermissions, src => src.ChildPermissions);

            // 用户映射
            config.NewConfig<User, UserDto>()
                .Map(dest => dest.UserType, src => src.UserType.ToString())
                .Map(dest => dest.Email, src => src.Email.Value);

            // 角色映射
            config.NewConfig<Role, RoleDto>();

            // 权限映射
            config.NewConfig<Permission, PermissionDto>();

            // 文章映射
            config.NewConfig<Article, ArticleDto>();
            config.NewConfig<ArticleTag, TagDto>();
            config.NewConfig<ArticleComment, ArticleCommentDto>();

            // 租房映射
            config.NewConfig<RentHouse, RentHouseListDto>();
        }
    }
}