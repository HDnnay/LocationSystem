using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos.RentHouses;
using LocationSystem.Application.Features.RentHousies.Queries.QueryRentHouseList;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using Mapster;

namespace LocationSystem.Application.Features.RentHousies.Queries.GetRentHouseList
{
    public class GetRentHouseListQueryHandler : IRequestHandler<GetRentHouseListQuery, PageResult<RentHouseListDto>>
    {
        private readonly IRentHouseRepository _repository;
        public GetRentHouseListQueryHandler(IRentHouseRepository repository)
        {
            _repository=repository;
        }
        public async Task<PageResult<RentHouseListDto>> Handle(GetRentHouseListQuery request)
        {
            var (total, result) = await _repository.GetRentHouseTuplePage(request);
            var pageResult = new PageResult<RentHouseListDto>();
            pageResult.Total = total;
            pageResult.CurrentPage = request.Page;
            pageResult.Data = result.Any() ? result.Select(t => t.Adapt<RentHouseListDto>()).ToList() : [];
            return pageResult;
        }
    }
}
