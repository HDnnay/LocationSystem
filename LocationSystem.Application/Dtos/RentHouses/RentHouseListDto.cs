using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Dtos.RentHouses
{
    public class RentHouseListDto
    {
        public RentHouseListDto() { }
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Address { get; set; }
        public required string Description { get; set; }
        public required HouserType Type { get; set; }
        public Guid CreateUserId { get; set; }
        public string? Phone { get; set; }
        public string? ImageSrc { get; set; }
        public decimal MonthlyRent { get; set; }
        public decimal Deposit { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
