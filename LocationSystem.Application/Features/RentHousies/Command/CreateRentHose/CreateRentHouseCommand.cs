using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.RentHousies.Command.CreateRentHose
{
    public class CreateRentHouseCommand : IRequset
    {
        public CreateRentHouseDto Model { get; set; }
    }
}
