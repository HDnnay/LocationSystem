using System.ComponentModel.DataAnnotations;

namespace LocationSystem.Domain.Entities
{
    public enum UserType
    {
        [Display(Name = "默认用户")]
        Default = 0,
        [Display(Name = "管理员")]
        Admin = 1,
        [Display(Name = "普通用户")]
        User = 2,
    }
}