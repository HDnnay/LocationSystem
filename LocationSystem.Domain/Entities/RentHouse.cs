using LocationSystem.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Domain.Entities
{
    public class RentHouse
    {
        private RentHouse() 
        {
            
        }
        public RentHouse(string title,string address, string descrption,
            decimal monthlyRent,decimal deposit, HouserType type,Guid createUserId,string phone)
        {
            Id= Guid.NewGuid();
            Title = title;
            Address = address;
            Description = descrption;
            MonthlyRent = monthlyRent;
            Deposit = deposit;
            Type = type;
            CreateUserId=createUserId;
            Phone = phone;
            CreateTime = DateTime.Now;
        }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public HouserType Type { get; set; }
        public Guid CreateUserId { get; set; }
        public string Phone { get; set; }
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
        public void SetOrUpdateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new BussinessRuleException("租房标题不能为空");
            Title = title;
        }
        public void SetOrUpdateMonthlyRent(decimal newValue)
        {
            if (newValue<=0)
                throw new BussinessRuleException("月租不能小于等于0");
            if (newValue==MonthlyRent)
                return;
            else
                MonthlyRent = newValue;
        }
        public void SetOrUpdateDeposit(decimal newValue)
        {
            if (newValue<0)
                throw new BussinessRuleException("押金不能小于0");
            if(newValue==Deposit) return;
            Deposit = newValue;
        }

    }
    public enum HouserType
    {
        OneRoom,
        TwoRoom,
        ThreeRoom,
    }
}
