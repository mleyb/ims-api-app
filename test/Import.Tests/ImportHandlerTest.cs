using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Import.Tests
{
    public class ImportHandlerTest
    {
        [Fact]
        public async Task HandleRequestAsync_ValidRequest_ReturnsOKResponse()
        {
            // Arrange

            var fakeVehicleDataService = A.Fake<IVehicleDataService>();

            IServiceProvider sp = new ServiceCollection()
                .AddSingleton(fakeVehicleDataService)
                .BuildServiceProvider();

            var context = A.Fake<ILambdaContext>();
            A.CallTo(() => context.Logger).Returns(A.Fake<ILambdaLogger>());

            APIGatewayProxyRequest dummyRequestEvent = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(new ImportRequest { CustomerId = 1, VIN = "ABCD" })
            };

            var sut = new ImportHandler(sp);

            // Act

            APIGatewayProxyResponse result = await sut.HandleRequestAsync(dummyRequestEvent, context);

            // Assert

            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task HandleRequestAsync_ValidRequest_InvokesVehicleDataServiceImportVehicleDataAsyncWithExpectedArgs()
        {
            // Arrange

            var fakeVehicleDataService = A.Fake<IVehicleDataService>();

            IServiceProvider sp = new ServiceCollection()
                .AddSingleton(fakeVehicleDataService)
                .BuildServiceProvider();

            var context = A.Fake<ILambdaContext>();
            A.CallTo(() => context.Logger).Returns(A.Fake<ILambdaLogger>());

            const int expectedCustomerId = 1;
            const string expectedVIN = "VIN";

            var importRequest = new ImportRequest { CustomerId = expectedCustomerId, VIN = expectedVIN };

            APIGatewayProxyRequest dummyRequestEvent = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(importRequest)
            };

            var sut = new ImportHandler(sp);

            // Act

            await sut.HandleRequestAsync(dummyRequestEvent, context);

            // Assert

            A.CallTo(() => fakeVehicleDataService.ImportVehicleDataAsync(expectedCustomerId, expectedVIN)).MustHaveHappenedOnceExactly();
        }
    }
}
