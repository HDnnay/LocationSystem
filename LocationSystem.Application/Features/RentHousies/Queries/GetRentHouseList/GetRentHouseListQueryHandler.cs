using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.RentHousies.Queries.QueryRentHouseList;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.RentHousies.Queries.GetRentHouseList
{
    public class GetRentHouseListQueryHandler : IRequestHandler<GetRentHouseListFilter, PageResult<RentHouseListDto>>
    {
        private readonly IRentHouseRepository _repository;
        public GetRentHouseListQueryHandler(IRentHouseRepository repository)
        {
            _repository=repository;
        }
        public async Task<PageResult<RentHouseListDto>> Handle(GetRentHouseListFilter request)
        {
            Dictionary<int,IEnumerable<RentHouse>> model = await _repository.GetRentHousePage(request);
            var pageResult = new PageResult<RentHouseListDto>();
            foreach (var item in model)
            {
                pageResult.Total = item.Key;
                pageResult.CurrentPage = request.Page;
                pageResult.Data = item.Value.Any() ? item.Value.Select(t=>t.ToPageListDto()).ToList(): [];
                break;
            }
            return pageResult;
        }
    }
}
