using LocationSystem.Application.Features.Users.Commands.CreateUser;
using LocationSystem.Application.Utilities;
using LocationSystem.Presentation.Models;
using Mapster;

namespace LocationSystem.Presentation.GraphQL
{
    public class Mutation
    {
        public async Task<UserType> CreateUserAsync(
           CreateUserCommand command,
           [Service] IMediator mediator)
        {
            var model = await mediator.Send(command);
            return model.Adapt<UserType>();
        }
    }
}
