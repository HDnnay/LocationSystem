using LocationSystem.Application.Features.RentHousies.Queries.QueryRentHouseList;
using LocationSystem.Application.Utilities.Common;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IRentHouseRepository: IRepository<RentHouse>
    {
        public Task<Dictionary<int, IEnumerable<RentHouse>>> GetRentHousePage(GetRentHouseListFilter filter);
        public Task<(int, IEnumerable<RentHouse>)> GetRentHouseTuplePage(GetRentHouseListFilter filter);
    }
}
