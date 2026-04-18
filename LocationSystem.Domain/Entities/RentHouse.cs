using LocationSystem.Domain.Entities.Interfacies;
using LocationSystem.Domain.Entities.UserRolePermissions;
using LocationSystem.Domain.Enums;
using LocationSystem.Domain.Exceptions;
using System.ComponentModel;

namespace LocationSystem.Domain.Entities
{
    public class RentHouse : ISoftDeleteEntity
    {
        private RentHouse()
        {

        }
        public RentHouse(string title, string address, string descrption,
            decimal monthlyRent, decimal deposit, HouserType type, Guid createUserId, string phone)
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
        [Description("标题")]

        public string Title { get; set; }
        [Description("地址")]

        public string Address { get; set; }
        [Description("描述")]

        public string Description { get; set; }
        [Description("租房类型")]

        public HouserType Type { get; set; }
        [Description("创建者id")]

        public Guid CreateUserId { get; set; }
        [Description("手机号")]

        public string Phone { get; set; }
        /// <summary>
        /// 月租
        /// </summary>
        [Description("月租")]
        public decimal MonthlyRent { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        [Description("押金")]
        public decimal Deposit { get; set; }
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }
        public string ImageSrc { get; set; }
        [Description("可见等级")]
        public LevelType LevelType { get; set; }
        [Description("开始时间")]
        public DateTime? StartTime { get; set; }
        [Description("结束时间")]
        public DateTime? EndTime { get; set; }
        [Description("创建者")]
        public virtual User? User { get; set; }
        public bool IsDelete { get; set; }
        public Guid DeleteUserId { get; set; }
        public DateTime DeleteTime { get; set; }
        public bool IsVisiable { get; set; }

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
            if (newValue==Deposit) return;
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
