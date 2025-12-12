using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Features.DentalOffices.Commands.UpDentalOffice;
using LocationSystem.Domain.Entities;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace LocationSystem.Tests.Applicstion.Features.DetalOffice
{
    [TestClass]
    public class UpdateDetalOfficeTests
    {
        private IDentalOfficeRepositoty repositoty;
        private IUnitOfWork unitOfWork;
        private UpdateDnetalOfficeCommandHandler handler;
        [TestInitialize]
        public void SetUp() 
        {
            repositoty = Substitute.For<IDentalOfficeRepositoty>();
            unitOfWork = Substitute.For<IUnitOfWork>();
            handler =   new UpdateDnetalOfficeCommandHandler(repositoty,unitOfWork);

        }
        [TestMethod]
        public async Task  Handler_UpdateDetalOfficeTest() 
        {
            var dentalOffice= new DentalOffice("DentalOffice A");
            var id = dentalOffice.Id;
            var command = new UpdateDetalOfficeCommand() { Id = id ,Name = "new name"};
            repositoty.GetByIdAsync(id).Returns(dentalOffice);
            await handler.Handle(command);
            await repositoty.Received(1).UpdateAsync(dentalOffice);
            await unitOfWork.Received(1).Commit();
             var result = await repositoty.GetByIdAsync(id);
            Assert.IsTrue(result.Name == "new name");


        }
        [TestMethod]
        public async Task Handler_UpdateDetalOfficeTest_Throw()
        {
        
            var command = new UpdateDetalOfficeCommand() { Id = Guid.NewGuid(), Name = "new name" };
            repositoty.GetByIdAsync(command.Id).ReturnsNull();
            await handler.Handle(command);
           
          


        }
    }
}
