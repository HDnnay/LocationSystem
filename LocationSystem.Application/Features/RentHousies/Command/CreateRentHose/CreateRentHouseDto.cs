using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.RentHousies.Command.CreateRentHose
{
    public class CreateRentHouseDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Address { get; set; }
        public required HouserType Type { get; set; }
        public string? Phone { get; set; }

        public required decimal MonthlyRent { get; set; }

        public required decimal Deposit { get; set; }
    }
}
