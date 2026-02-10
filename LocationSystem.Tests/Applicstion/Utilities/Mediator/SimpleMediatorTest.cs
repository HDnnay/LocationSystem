using LocationSystem.Application.Exceptions;
using LocationSystem.Application.Utilities;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace LocationSystem.Tests.Applicstion.Utilities.Mediator
{
    [TestClass]
    public class SimpleMediatorTest
    {
        public class FalseRequest : IRequest<string> { }
        [TestMethod]
        public async Task Send_WithRegisterHandler_HandlerExecuted()
        {
            var request = new FalseRequest();
            var handlerMock = Substitute.For<IRequestHandler<FalseRequest, string>>();
            var serviceProviderMock = Substitute.For<IServiceProvider>();
            serviceProviderMock.GetService(typeof(IRequestHandler<FalseRequest, string>))
                .Returns(handlerMock);
            var mediator = new SimpleMediator(serviceProviderMock);
            var result = await mediator.Send(request);
            await handlerMock.Received(1).Handle(request);
        }
        [TestMethod]
        public async Task Send_WithoutRegisterHandler_ThrowMediatorException()
        {
            var request = new FalseRequest();
            var serviceProviderMock = Substitute.For<IServiceProvider>();
            serviceProviderMock.GetService(typeof(IRequestHandler<FalseRequest, string>))
                .ReturnsNull();
            var mediator = new SimpleMediator(serviceProviderMock);

            await Assert.ThrowsExactlyAsync<MediatorExpcetion>(async () =>
             {
                 var result = await mediator.Send(request);
             });
        }

    }
}
