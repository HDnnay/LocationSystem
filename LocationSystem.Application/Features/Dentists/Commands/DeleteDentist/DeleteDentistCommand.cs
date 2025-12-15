using LocationSystem.Application.Utilities;
using System;

namespace LocationSystem.Application.Features.Dentists.Commands.DeleteDentist
{
    public class DeleteDentistCommand : IRequset
    {
        public Guid Id { get; set; }
    }
}