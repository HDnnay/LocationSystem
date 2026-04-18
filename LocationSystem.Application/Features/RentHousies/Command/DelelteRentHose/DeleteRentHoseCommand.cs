using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.RentHousies.Command.DelelteRentHose
{
    public class DeleteRentHoseCommand : IRequset
    {
        public Guid? Id { get; set; }
    }
}
