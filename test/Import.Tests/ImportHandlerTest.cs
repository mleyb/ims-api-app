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

            const string vin = "VIN";

            var fakeValidateVIN = A.Fake<IValidateVIN>();
            A.CallTo(() => fakeValidateVIN.IsValid(vin)).Returns(true);

            var fakeVehicleDataService = A.Fake<IVehicleDataService>();

            IServiceProvider sp = new ServiceCollection()
                .AddSingleton(fakeValidateVIN)
                .AddSingleton(fakeVehicleDataService)
                .BuildServiceProvider();

            var context = A.Fake<ILambdaContext>();
            A.CallTo(() => context.Logger).Returns(A.Fake<ILambdaLogger>());

            APIGatewayProxyRequest dummyRequestEvent = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(new ImportRequest { CustomerId = 1, VIN = vin })
            };

            var sut = new ImportHandler(sp);

            // Act

            APIGatewayProxyResponse response = await sut.HandleRequestAsync(dummyRequestEvent, context);

            // Assert

            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task HandleRequestAsync_ValidRequest_InvokesVehicleDataServiceImportVehicleDataAsyncWithExpectedArgs()
        {
            // Arrange

            const int expectedCustomerId = 1;
            const string expectedVIN = "VIN";

            var fakeValidateVIN = A.Fake<IValidateVIN>();
            A.CallTo(() => fakeValidateVIN.IsValid(expectedVIN)).Returns(true);

            var fakeVehicleDataService = A.Fake<IVehicleDataService>();

            IServiceProvider sp = new ServiceCollection()
                .AddSingleton(fakeValidateVIN)
                .AddSingleton(fakeVehicleDataService)
                .BuildServiceProvider();

            var context = A.Fake<ILambdaContext>();
            A.CallTo(() => context.Logger).Returns(A.Fake<ILambdaLogger>());

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

        [Fact]
        public async Task HandleRequestAsync_VINValidationFails_ReturnsBadRequestResponse()
        {
            // Arrange

            const string vin = "VIN";

            var fakeValidateVIN = A.Fake<IValidateVIN>();
            A.CallTo(() => fakeValidateVIN.IsValid(vin)).Returns(false);

            var fakeVehicleDataService = A.Fake<IVehicleDataService>();

            IServiceProvider sp = new ServiceCollection()
                .AddSingleton(fakeValidateVIN)
                .AddSingleton(fakeVehicleDataService)
                .BuildServiceProvider();

            var context = A.Fake<ILambdaContext>();
            A.CallTo(() => context.Logger).Returns(A.Fake<ILambdaLogger>());

            var importRequest = new ImportRequest { CustomerId = 1, VIN = vin };

            APIGatewayProxyRequest dummyRequestEvent = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(importRequest)
            };

            var sut = new ImportHandler(sp);

            // Act

            APIGatewayProxyResponse response = await sut.HandleRequestAsync(dummyRequestEvent, context);

            // Assert

            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
