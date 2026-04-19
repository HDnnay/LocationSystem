using LocationSystem.Application.Dtos.Interfaces;

namespace LocationSystem.Application.Dtos.Provinces
{
    public class ProvinceDto : ICompanyEntity
    {
        public string? Address { get; set; }
        public string? Province { get; set; }
    }
}
