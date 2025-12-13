using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Appointments.Mapper;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Queries.GetAppointmentDetail
{
    public class GetAppointmentDetailQueryHandler : IRequestHandler<GetAppointmentDetailQuery, AppointmentDetailDto>
    {
        private readonly IAppointmentRepository _repository;
        public GetAppointmentDetailQueryHandler(IAppointmentRepository repository) 
        {
            _repository = repository;
        }
        public async Task<AppointmentDetailDto> Handle(GetAppointmentDetailQuery request)
        {
            var appointment = await _repository.GetByIdAsync(request.Id);
            if (appointment == null)
                throw new BussinessRuleException("该预约不存在");
            var model = appointment.MapToDto();
            return model;
        }
    }
}
