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
    public class GetRentHouseListQueryHandler : IRequestHandler<GetRentHouseListQuery, PageResult<RentHouseListDto>>
    {
        private readonly IRentHouseRepository _repository;
        public GetRentHouseListQueryHandler(IRentHouseRepository repository)
        {
            _repository=repository;
        }
        public async Task<PageResult<RentHouseListDto>> Handle(GetRentHouseListQuery request)
        {
            var (total,result) = await _repository.GetRentHouseTuplePage(request);
            var pageResult = new PageResult<RentHouseListDto>();
            pageResult.Total = total;
            pageResult.CurrentPage = request.Page;
            pageResult.Data = result.Any() ? result.Select(t => t.ToPageListDto()).ToList() : [];
            return pageResult;
        }
    }
}
