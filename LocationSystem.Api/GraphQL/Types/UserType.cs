using HotChocolate.Types;
using LocationSystem.Application.Dtos;
using LocationSystem.Domain.Entities;
using LocationSystem.Api.GraphQL.DataLoaders;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LocationSystem.Api.GraphQL.Types
{
    public class UserType : ObjectType<UserDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserDto> descriptor)
        {
            descriptor.Field(u => u.Id).Type<NonNullType<IdType>>();
            descriptor.Field(u => u.Name).Type<NonNullType<StringType>>();
            descriptor.Field(u => u.Email).Type<NonNullType<StringType>>();
            descriptor.Field(u => u.UserType).Type<NonNullType<StringType>>();
            descriptor.Field(u => u.IsDisabled).Type<NonNullType<BooleanType>>();
            
            descriptor.Field("roles")
                .Type<ListType<RoleType>>()
                .ResolveWith<UserResolvers>(r => r.GetRoles(default!, default!, default!, default!));
        }

        private class UserResolvers
        {
            public async Task<List<RoleDto>> GetRoles(
                [Parent] UserDto user,
                UserRolesDataLoader userRolesDataLoader,
                IMapper mapper,
                CancellationToken cancellationToken)
            {
                var roles = await userRolesDataLoader.LoadAsync(user.Id, cancellationToken);
                return mapper.Map<List<RoleDto>>(roles);
            }
        }
    }
}