using LocationSystem.Application.Features.RentHousies.Queries.ShareDtos;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.RentHousies.Queries.GetRentHouseList
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
        /// <summary>
        /// 月租
        /// </summary>
        public decimal MonthlyRent { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public decimal Deposit { get; set; }
        public DateTime CreateTime
        {
            get; set;
        }
    }
    public static class RentHouseExtensions
    {
        public static RentHouseListDto ToPageListDto(this RentHouse rentHouse)
        {
            return new RentHouseListDto
            {
                Id = rentHouse.Id,
                Title = rentHouse.Title,
                Address = rentHouse.Address,
                Description = rentHouse.Description,
                Phone = rentHouse.Phone,
                Type = rentHouse.Type,
                MonthlyRent = rentHouse.MonthlyRent,
                Deposit = rentHouse.Deposit,
                CreateTime = rentHouse.CreateTime,
                CreateUserId = rentHouse.CreateUserId,
                ImageSrc = rentHouse.ImageSrc
            };
        }
    }
}
