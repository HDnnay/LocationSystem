using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Dtos.RentHouses
{
    public class RentHouseDto
    {
        public RentHouseDto() { }
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Address { get; set; }
        public required string Description { get; set; }
        public required HouserType Type { get; set; }
        public Guid CreateUserId { get; set; }
        public string? Phone { get; set; }
        /// <summary>
        /// 月租
        /// </summary>
        public decimal MonthlyRent { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public decimal Deposit { get; set; }
        public DateTime CreateTime { get; set; }
        public string ImageSrc { get; set; }

    }
}
