using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.RentHousies.Queries.ShareDtos;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace LocationSystem.Application.Features.RentHousies.Queries.GetRentHouseDetail
{
    public class GetRentHouseDetailQueryHandler : IRequsetHandler<GetRentHouseDetailQuery, RentHouseDto>
    {
        private readonly IRentHouseRepository _repository;
        public GetRentHouseDetailQueryHandler(IRentHouseRepository repository) 
        {
            _repository = repository;
        }
        public async Task<RentHouseDto> Handle(GetRentHouseDetailQuery request)
        {
            var result = await _repository.GetByIdAsync(request.Id);
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            var model = new RentHouseDto()
            {
                Id=result.Id,
                Address=result.Address,
                Title=result.Title,
                Phone=result.Phone,
                Description=result.Description,
                Type=result.Type,
                Deposit=result.Deposit,
                MonthlyRent=result.MonthlyRent,
                CreateUserId =result.CreateUserId,
                CreateTime=result.CreateTime,
                ImageSrc=result.ImageSrc
            };
            return model;
        }
    }
}
