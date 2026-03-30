using LocationSystem.Application.Dtos;

namespace LocationSystem.Api.GraphQL.Types
{
    public class UserType : ObjectType<UserDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserDto> descriptor)
        {
            descriptor.Name("AppUserDto");
            descriptor.Field(u => u.Id).Type<NonNullType<IdType>>();
            descriptor.Field(u => u.Name).Type<NonNullType<StringType>>();
            descriptor.Field(u => u.Email).Type<NonNullType<StringType>>();
            descriptor.Field(u => u.UserType).Type<NonNullType<StringType>>();
            descriptor.Field(u => u.IsDisabled).Type<NonNullType<BooleanType>>();
            descriptor.Field(u => u.Roles).Type<ListType<RoleType>>();
            //descriptor.Field(u => u.Roles).Type<ListType<RoleType>>().Resolve(async c =>
            //{
            //    //var user = c.Parent<UserDto>();

            //    //// 使用 UserRolesDataLoader 加载用户的角色
            //    //var userRolesDataLoader = c.Service<UserRolesDataLoader>();
            //    //var roles = await userRolesDataLoader.LoadAsync(user.Id);

            //    //if (roles == null || !roles.Any()) return null;

            //    //// 使用 RoleDataLoader 加载角色详情
            //    //var roleDataLoader = c.Service<RoleDataLoader>();
            //    //var roleTasks = roles.Select(role => roleDataLoader.LoadAsync(role.Id));
            //    //var loadedRoles = await Task.WhenAll(roleTasks);

            //    //// 过滤掉 null 值
            //    //var validRoles = loadedRoles.Where(r => r != null);

            //    //// 使用 AutoMapper 映射为 RoleDto
            //    //var mapper = c.Service<IMapper>();
            //    //return validRoles.Select(role => mapper.Map<RoleDto>(role));
            //});
        }
    }
}