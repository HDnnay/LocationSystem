using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Appointments.Mapper;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Queries.GetAppointmentList
{
    public class GetAppointmentListQueryHandler : IRequsetHandler<GetAppointmentListQuery, PageResult<AppointmentListDto>>
    {
        private readonly IAppointmentRepository _repository;

        public GetAppointmentListQueryHandler(IAppointmentRepository repository) 
        {
            _repository = repository;
        }
        public async Task<PageResult<AppointmentListDto>> Handle(GetAppointmentListQuery request)
        {
            var appointments =await _repository.GetAppointmentPage(request);
            var total = await _repository.GetTotalCount();
            return new PageResult<AppointmentListDto>()
            {
                Data = appointments.Select(t => t.MapToListDto()).ToList(),
                Total = total
            };
        }
    }
}
