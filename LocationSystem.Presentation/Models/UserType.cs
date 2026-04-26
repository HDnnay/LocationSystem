﻿﻿﻿﻿﻿﻿﻿﻿﻿using HotChocolate.Types;
using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Application.GrapqLDTOs.Users;
using LocationSystem.Presentation.DataLoaders;
using UserRolePermissions = LocationSystem.Domain.Entities.UserRolePermissions;

namespace LocationSystem.Presentation.Models
{
    public class UserType : ObjectType<UserGraphqLDto>  // ← 直接使用 Application 层的 DTO
    {
        protected override void Configure(IObjectTypeDescriptor<UserGraphqLDto> descriptor)
        {
            descriptor.Name("user");
            descriptor.Field(t => t.Id).Type<NonNullType<IdType>>().Description("用户ID");
            descriptor.Field(t => t.Name).Type<StringType>().Description("用户名");
            descriptor.Field(t => t.Email).Type<StringType>().Description("邮箱");
            descriptor.Field(t => t.UserType).Type<EnumType<UserRolePermissions.UserType>>().Description("用户类型");
            descriptor.Field(t => t.IsDisabled).Type<BooleanType>().Description("是否禁用");
            descriptor.Field(t => t.IsDelete).Type<BooleanType>().Description("是否删除");
            descriptor.Field(t => t.CreateTime).Type<DateTimeType>().Description("创建时间");
            descriptor.Field(t => t.DeleteTime).Type<DateTimeType>().Description("删除时间");
            descriptor.Field("roles").Type<ListType<RoleType>>().Description("用户角色列表").Resolve(async context =>
            {
                var user = context.Parent<UserGraphqLDto>();
                var dataLoader = context.DataLoader<UserRoleDataLoader>();
                var roles = await dataLoader.LoadAsync(user.Id, context.RequestAborted);
                return roles ?? new List<RoleGraphqLDto>();
            });
        }
    }
}