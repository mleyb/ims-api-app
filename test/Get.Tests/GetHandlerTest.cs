using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Get.Tests
{
    public class GetHandlerTest
    {
        [Fact]
        public async Task HandleRequestAsync_ValidRequest_VehicleDataFoundReturnsOKResponse()
        {
            // Arrange

            const string vin = "VIN";

            var data = new VehicleData
            {
                CustomerId = 1,
                VIN = "VIN",
                Make = "BMW",
                Model = "118d",
                ModelYear = 2016,
                ImportedDate = DateTime.UtcNow
            };

            var fakeValidateVIN = A.Fake<IValidateVIN>();
            A.CallTo(() => fakeValidateVIN.IsValid(vin)).Returns(true);

            var fakeVehicleDataService = A.Fake<IVehicleDataService>();
            A.CallTo(() => fakeVehicleDataService.GetVehicleDataAsync(vin)).Returns(data);

            IServiceProvider sp = new ServiceCollection()
                .AddSingleton(fakeValidateVIN)
                .AddSingleton(fakeVehicleDataService)
                .BuildServiceProvider();

            var context = A.Fake<ILambdaContext>();
            A.CallTo(() => context.Logger).Returns(A.Fake<ILambdaLogger>());

            APIGatewayProxyRequest dummyRequestEvent = new APIGatewayProxyRequest
            {
                PathParameters = new Dictionary<string, string> { { "id", vin } }
            };

            var sut = new GetHandler(sp);

            // Act

            APIGatewayProxyResponse result = await sut.HandleRequestAsync(dummyRequestEvent, context);

            // Assert

            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task HandleRequestAsync_ValidRequest_VehicleDataFoundReturnsResponseWithExpectedContent()
        {
            // Arrange

            const string vin = "VIN";

            var data = new VehicleData
            {
                CustomerId = 1,
                VIN = "VIN",
                Make = "BMW",
                Model = "118d",
                ModelYear = 2016,
                ImportedDate = DateTime.UtcNow
            };

            var fakeValidateVIN = A.Fake<IValidateVIN>();
            A.CallTo(() => fakeValidateVIN.IsValid(vin)).Returns(true);

            var fakeVehicleDataService = A.Fake<IVehicleDataService>();
            A.CallTo(() => fakeVehicleDataService.GetVehicleDataAsync(vin)).Returns(data);

            IServiceProvider sp = new ServiceCollection()
                .AddSingleton(fakeValidateVIN)
                .AddSingleton(fakeVehicleDataService)
                .BuildServiceProvider();

            var context = A.Fake<ILambdaContext>();
            A.CallTo(() => context.Logger).Returns(A.Fake<ILambdaLogger>());

            APIGatewayProxyRequest dummyRequestEvent = new APIGatewayProxyRequest
            {
                PathParameters = new Dictionary<string, string> { { "id", vin } }
            };

            var sut = new GetHandler(sp);

            // Act

            APIGatewayProxyResponse result = await sut.HandleRequestAsync(dummyRequestEvent, context);

            ResponseBody responseBody = JsonConvert.DeserializeObject<ResponseBody>(result.Body);

            // Assert

            Assert.Equal(data.CustomerId, responseBody.CustomerId);
            Assert.Equal(data.VIN, responseBody.VIN);
            Assert.Equal(data.Make, responseBody.Make);
            Assert.Equal(data.Model, responseBody.Model);
            Assert.Equal(data.ModelYear, responseBody.ModelYear);
            Assert.Equal(data.ImportedDate, responseBody.ImportedDate);
        }

        [Fact]
        public async Task HandleRequestAsync_ValidRequest_NoVehicleDataFoundReturnsNotFoundResponse()
        {
            // Arrange

            const string vin = "VIN";

            var fakeValidateVIN = A.Fake<IValidateVIN>();
            A.CallTo(() => fakeValidateVIN.IsValid(vin)).Returns(true);

            var fakeVehicleDataService = A.Fake<IVehicleDataService>();
            A.CallTo(() => fakeVehicleDataService.GetVehicleDataAsync(vin)).Returns<VehicleData>(null);

            IServiceProvider sp = new ServiceCollection()
                .AddSingleton(fakeValidateVIN)
                .AddSingleton(fakeVehicleDataService)
                .BuildServiceProvider();

            var context = A.Fake<ILambdaContext>();
            A.CallTo(() => context.Logger).Returns(A.Fake<ILambdaLogger>());

            APIGatewayProxyRequest dummyRequestEvent = new APIGatewayProxyRequest
            {
                PathParameters = new Dictionary<string, string> { { "id", vin } }
            };

            var sut = new GetHandler(sp);

            // Act

            APIGatewayProxyResponse result = await sut.HandleRequestAsync(dummyRequestEvent, context);

            // Assert

            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
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

            APIGatewayProxyRequest dummyRequestEvent = new APIGatewayProxyRequest
            {
                PathParameters = new Dictionary<string, string> { { "id", vin } }
            };

            var sut = new GetHandler(sp);

            // Act

            APIGatewayProxyResponse result = await sut.HandleRequestAsync(dummyRequestEvent, context);

            // Assert

            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
