using HotChocolate.Types;
using LocationSystem.Api.GraphQL.Commands;
using LocationSystem.Api.GraphQL.Types;
using LocationSystem.Application.Contrats;
using LocationSystem.Application.Features.Articles.Models;
using LocationSystem.Application.Features.Menus.Commands.AssignPermissionsToMenu;
using LocationSystem.Application.Features.Menus.Commands.CreateMenu;
using LocationSystem.Application.Features.Menus.Commands.DeleteMenu;
using LocationSystem.Application.Features.Menus.Commands.UpdateMenu;
using LocationSystem.Application.Features.Menus.Models;
using LocationSystem.Application.Features.Roles.Commands.CreateRole;
using LocationSystem.Application.Features.Roles.Commands.DeleteRole;
using LocationSystem.Application.Features.Roles.Commands.UpdateRole;
using LocationSystem.Application.Features.Users.Commands.AssignRoles;
using LocationSystem.Application.Features.Users.Commands.CreateUser;
using LocationSystem.Application.Features.Users.Commands.DeleteUser;
using LocationSystem.Application.Features.Users.Commands.UpdateUser;
using LocationSystem.Application.Features.Users.Models;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities.Articles;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dtos = LocationSystem.Application.Dtos;

namespace LocationSystem.Api.GraphQL
{
    public class MutationType : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            // 菜单相关操作
            descriptor.Field(m => m.CreateMenu(default!))
                .Name("createMenu")
                .Description("创建菜单")
                .Argument("command", a => a.Type<NonNullType<CreateMenuCommandType>>())
                .Type<MenuType>();

            descriptor.Field(m => m.UpdateMenu(default!, default!))
                .Name("updateMenu")
                .Description("更新菜单")
                .Argument("id", a => a.Type<NonNullType<IdType>>())
                .Argument("command", a => a.Type<NonNullType<UpdateMenuCommandType>>())
                .Type<MenuType>();

            descriptor.Field(m => m.DeleteMenu(default!))
                .Name("deleteMenu")
                .Description("删除菜单")
                .Argument("id", a => a.Type<NonNullType<IdType>>())
                .Type<SuccessResponseType>();

            descriptor.Field(m => m.AssignPermissionsToMenu(default!, default!))
                .Name("assignPermissionsToMenu")
                .Description("为菜单分配权限")
                .Argument("id", a => a.Type<NonNullType<IdType>>())
                .Argument("permissionIds", a => a.Type<NonNullType<ListType<NonNullType<IdType>>>>())
                .Type<SuccessResponseType>();

            // 用户相关操作
            descriptor.Field(m => m.CreateUser(default!))
                .Name("createUser")
                .Description("创建用户")
                .Argument("command", a => a.Type<NonNullType<CreateUserCommandType>>())
                .Type<NonNullType<IdType>>();

            descriptor.Field(m => m.UpdateUser(default!, default!))
                .Name("updateUser")
                .Description("更新用户")
                .Argument("id", a => a.Type<NonNullType<IdType>>())
                .Argument("command", a => a.Type<NonNullType<UpdateUserCommandType>>())
                .Type<UserType>();

            descriptor.Field(m => m.DeleteUser(default!))
                .Name("deleteUser")
                .Description("删除用户")
                .Argument("id", a => a.Type<NonNullType<IdType>>())
                .Type<SuccessResponseType>();

            descriptor.Field(m => m.AssignRolesToUser(default!, default!))
                .Name("assignRolesToUser")
                .Description("为用户分配角色")
                .Argument("id", a => a.Type<NonNullType<IdType>>())
                .Argument("roleIds", a => a.Type<NonNullType<ListType<NonNullType<IdType>>>>())
                .Type<SuccessResponseType>();

            // 角色相关操作
            descriptor.Field(m => m.CreateRole(default!))
                .Name("createRole")
                .Description("创建角色")
                .Argument("command", a => a.Type<NonNullType<CreateRoleCommandType>>())
                .Type<RoleType>();

            descriptor.Field(m => m.UpdateRole(default!, default!))
                .Name("updateRole")
                .Description("更新角色")
                .Argument("id", a => a.Type<NonNullType<IdType>>())
                .Argument("command", a => a.Type<NonNullType<UpdateRoleCommandType>>())
                .Type<RoleType>();

            descriptor.Field(m => m.DeleteRole(default!))
                .Name("deleteRole")
                .Description("删除角色")
                .Argument("id", a => a.Type<NonNullType<IdType>>())
                .Type<SuccessResponseType>();

            descriptor.Field(m => m.AssignPermissionsToRole(default!, default!))
                .Name("assignPermissionsToRole")
                .Description("为角色分配权限")
                .Argument("id", a => a.Type<NonNullType<IdType>>())
                .Argument("permissionIds", a => a.Type<NonNullType<ListType<NonNullType<IdType>>>>())
                .Type<SuccessResponseType>();

            // 文章相关操作
            descriptor.Field(m => m.CreateArticle(default!, default!, default!, default!, default!, default!, default!))
                .Name("createArticle")
                .Description("创建文章")
                .Argument("title", a => a.Type<NonNullType<StringType>>())
                .Argument("content", a => a.Type<NonNullType<StringType>>())
                .Argument("isVisiable", a => a.Type<NonNullType<BooleanType>>())
                .Argument("userId", a => a.Type<NonNullType<IdType>>())
                .Argument("topic", a => a.Type<StringType>())
                .Argument("subtitle", a => a.Type<StringType>())
                .Argument("tagIds", a => a.Type<ListType<IdType>>())
                .Type<ArticleType>();

            descriptor.Field(m => m.UpdateArticle(default!, default!, default!, default!, default!, default!, default!))
                .Name("updateArticle")
                .Description("更新文章")
                .Argument("id", a => a.Type<NonNullType<IdType>>())
                .Argument("title", a => a.Type<NonNullType<StringType>>())
                .Argument("content", a => a.Type<NonNullType<StringType>>())
                .Argument("isVisiable", a => a.Type<NonNullType<BooleanType>>())
                .Argument("topic", a => a.Type<StringType>())
                .Argument("subtitle", a => a.Type<StringType>())
                .Argument("tagIds", a => a.Type<ListType<IdType>>())
                .Type<ArticleType>();

            descriptor.Field(m => m.DeleteArticle(default!))
                .Name("deleteArticle")
                .Description("删除文章")
                .Argument("id", a => a.Type<NonNullType<IdType>>())
                .Type<SuccessResponseType>();
        }
    }

    public class Mutation
    {
        private readonly IMediator _mediator;
        private readonly IArticleRepository _articleRepository;
        private readonly ITagRepository _tagRepository;
        private readonly AutoMapper.IMapper _mapper;

        public Mutation(IMediator mediator, IArticleRepository articleRepository, ITagRepository tagRepository, AutoMapper.IMapper mapper)
        {
            _mediator = mediator;
            _articleRepository = articleRepository;
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        // 菜单相关操作
        public async Task<MenuDto> CreateMenu(CreateMenuCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<MenuDto> UpdateMenu(Guid id, UpdateMenuCommand command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

        public async Task<SuccessResponse> DeleteMenu(Guid id)
        {
            var command = new DeleteMenuCommand { MenuId = id };
            await _mediator.Send(command);
            return new SuccessResponse { Success = true };
        }

        public async Task<SuccessResponse> AssignPermissionsToMenu(Guid id, List<Guid> permissionIds)
        {
            var command = new AssignPermissionsToMenuCommand { MenuId = id, PermissionIds = permissionIds };
            await _mediator.Send(command);
            return new SuccessResponse { Success = true };
        }

        // 用户相关操作
        public async Task<Guid> CreateUser(CreateUserCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<UserDto> UpdateUser(Guid id, UpdateUserCommand command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

        public async Task<SuccessResponse> DeleteUser(Guid id)
        {
            var command = new DeleteUserCommand { UserId = id };
            await _mediator.Send(command);
            return new SuccessResponse { Success = true };
        }

        public async Task<SuccessResponse> AssignRolesToUser(Guid id, List<Guid> roleIds)
        {
            var command = new AssignRolesCommand { UserId = id, RoleIds = roleIds };
            await _mediator.Send(command);
            return new SuccessResponse { Success = true };
        }

        // 角色相关操作
        public async Task<Dtos.RoleDto> CreateRole(CreateRoleCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<Dtos.RoleDto> UpdateRole(Guid id, UpdateRoleCommand command)
        {
            command.RoleId = id;
            return await _mediator.Send(command);
        }

        public async Task<SuccessResponse> DeleteRole(Guid id)
        {
            var command = new DeleteRoleCommand { RoleId = id };
            await _mediator.Send(command);
            return new SuccessResponse { Success = true };
        }

        public async Task<SuccessResponse> AssignPermissionsToRole(Guid id, List<Guid> permissionIds)
        {
            var command = new UpdateRoleCommand { RoleId = id, PermissionIds = permissionIds };
            await _mediator.Send(command);
            return new SuccessResponse { Success = true };
        }

        // 文章相关操作
        public async Task<ArticleDto> CreateArticle(string title, string content, bool isVisiable, Guid userId, string? topic, string? subtitle, List<Guid>? tagIds)
        {
            var article = new Article(title, content, isVisiable, userId, topic, subtitle);

            if (tagIds != null && tagIds.Any())
            {
                var tags = await _tagRepository.GetByIdsAsync(tagIds);
                article.UpdateTags(tags.ToList());
            }

            await _articleRepository.AddAsync(article);
            await _articleRepository.SaveChangesAsync();

            return _mapper.Map<ArticleDto>(article);
        }

        public async Task<ArticleDto> UpdateArticle(Guid id, string title, string content, bool isVisiable, string? topic, string? subtitle, List<Guid>? tagIds)
        {
            var article = await _articleRepository.GetByIdAsync(id);
            if (article == null)
            {
                throw new Exception($"文章不存在，ID: {id}");
            }

            // 使用 Article 实体的 Update 方法更新属性
            article.Update(title, content, isVisiable, topic, subtitle);

            if (tagIds != null)
            {
                var tags = await _tagRepository.GetByIdsAsync(tagIds);
                article.UpdateTags(tags.ToList());
            }

            await _articleRepository.UpdateAsync(article);
            await _articleRepository.SaveChangesAsync();

            return _mapper.Map<ArticleDto>(article);
        }

        public async Task<SuccessResponse> DeleteArticle(Guid id)
        {
            var article = await _articleRepository.GetByIdAsync(id);
            if (article == null)
            {
                throw new Exception($"文章不存在，ID: {id}");
            }

            await _articleRepository.DeleteAsync(article);
            await _articleRepository.SaveChangesAsync();

            return new SuccessResponse { Success = true };
        }
    }


}