using HotChocolate.Types;
using LocationSystem.Application.Dtos;
using LocationSystem.Domain.Entities;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LocationSystem.Api.GraphQL
{
    public class RoleType : ObjectType<RoleDto>
    {
        protected override void Configure(IObjectTypeDescriptor<RoleDto> descriptor)
        {
            descriptor.Field(r => r.Id).Type<NonNullType<IdType>>();
            descriptor.Field(r => r.Name).Type<NonNullType<StringType>>();
            descriptor.Field(r => r.Code).Type<NonNullType<StringType>>();
            descriptor.Field(r => r.Description).Type<StringType>();
            descriptor.Field(r => r.IsDisabled).Type<NonNullType<BooleanType>>();
            descriptor.Field(r => r.CreatedAt).Type<NonNullType<DateTimeType>>();
            descriptor.Field(r => r.UpdatedAt).Type<DateTimeType>();
            
            descriptor.Field("permissions")
                .Type<ListType<PermissionType>>()
                .ResolveWith<RoleResolvers>(r => r.GetPermissions(default!, default!, default!, default!));
        }

        private class RoleResolvers
        {
            public async Task<List<PermissionDto>> GetPermissions(
                [Parent] RoleDto role,
                RolePermissionsDataLoader rolePermissionsDataLoader,
                IMapper mapper,
                CancellationToken cancellationToken)
            {
                var permissions = await rolePermissionsDataLoader.LoadAsync(role.Id, cancellationToken);
                return mapper.Map<List<PermissionDto>>(permissions);
            }
        }
    }
}