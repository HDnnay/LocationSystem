using LocationSystem.Application.Features.Auth.Login.Commands;
using LocationSystem.Application.Features.Users.Commands.CreateUser;
using LocationSystem.Application.GrapqLDTOs.Auth;
using LocationSystem.Application.GrapqLDTOs.Users;
using LocationSystem.Application.Utilities;
using Mapster;

namespace LocationSystem.Presentation.GraphQL
{


    public class Mutation
    {
        public async Task<UserGraphqLDto> CreateUserAsync(
           CreateUserCommand command,
           [Service] IMediator mediator)
        {
            var model = await mediator.Send(command);
            return model.Adapt<UserGraphqLDto>();
        }
        public async Task<LoginResponseGraphqLDto> LoginAsync(LoginCommand command, [Service] IMediator mediator)
        {
            var model = await mediator.Send(command);
            return model.Adapt<LoginResponseGraphqLDto>();

        }
    }
}
