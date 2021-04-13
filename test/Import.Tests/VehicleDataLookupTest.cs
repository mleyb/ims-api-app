using FakeItEasy;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Import.Tests
{
    public class VehicleDataLookupTest
    {
        [Fact]
        public async Task GetVehicleDataByVINAsync_NotFoundResponse_ThrowsHttpRequestException()
        {
            // Arrange

            const string testVIN = "VIN";

            var notFoundResponse = new HttpResponseMessage(HttpStatusCode.NotFound);

            var fakeVPICHttpClient = A.Fake<IVPICHttpClient>();
            A.CallTo(() => fakeVPICHttpClient.DecodeVINAsync(testVIN)).Returns(notFoundResponse);

            var fakeVehicleDataResponseParser = A.Fake<IVehicleDataLookupResponseParser>();

            var sut = new VehicleDataLookup(fakeVPICHttpClient, fakeVehicleDataResponseParser);

            // Act/Assert

            await Assert.ThrowsAsync<HttpRequestException>(async () => await sut.GetVehicleDataByVINAsync(testVIN));
        }

        [Fact]
        public async Task GetVehicleDataByVINAsync_BadRequestResponse_ThrowsHttpRequestException()
        {
            // Arrange

            const string testVIN = "VIN";

            var badRequestResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var fakeVPICHttpClient = A.Fake<IVPICHttpClient>();
            A.CallTo(() => fakeVPICHttpClient.DecodeVINAsync(testVIN)).Returns(badRequestResponse);

            var fakeVehicleDataResponseParser = A.Fake<IVehicleDataLookupResponseParser>();

            var sut = new VehicleDataLookup(fakeVPICHttpClient, fakeVehicleDataResponseParser);

            // Act/Assert

            await Assert.ThrowsAsync<HttpRequestException>(async () => await sut.GetVehicleDataByVINAsync(testVIN));
        }

        [Fact]
        public async Task GetVehicleDataByVINAsync_ValidResponse_ReturnsNonNullResult()
        {
            // Arrange

            const string testVIN = "VIN";

            var okResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("") };

            var resultData = new VehicleAPIData
            {
                Results = new List<VehicleAPIDataResult>
                {
                    new VehicleAPIDataResult { Variable = VehicleLookupData.VariableNames.Make, Value = "IGNORED" },
                    new VehicleAPIDataResult { Variable = VehicleLookupData.VariableNames.Model, Value = "IGNORED" },
                    new VehicleAPIDataResult { Variable = VehicleLookupData.VariableNames.ModelYear, Value = "0" }
                }
            };

            var fakeVPICHttpClient = A.Fake<IVPICHttpClient>();
            A.CallTo(() => fakeVPICHttpClient.DecodeVINAsync(testVIN)).Returns(okResponse);

            var fakeVehicleDataResponseParser = A.Fake<IVehicleDataLookupResponseParser>();
            A.CallTo(() => fakeVehicleDataResponseParser.ParseResponse(A<string>.Ignored)).Returns(resultData);

            var sut = new VehicleDataLookup(fakeVPICHttpClient, fakeVehicleDataResponseParser);

            // Act

            var data = await sut.GetVehicleDataByVINAsync(testVIN);

            // Assert

            A.CallTo(() => fakeVPICHttpClient.DecodeVINAsync(testVIN)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeVehicleDataResponseParser.ParseResponse(A<string>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.NotNull(data);
        }

        [Fact]
        public async Task GetVehicleDataByVINAsync_ValidResponse_ReturnsDataWithExpectedMake()
        {
            // Arrange

            const string testVIN = "VIN";

            const string expectedMake = "BMW";

            var okResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("") };

            var resultData = new VehicleAPIData
            {
                Results = new List<VehicleAPIDataResult>
                {
                    new VehicleAPIDataResult { Variable = VehicleLookupData.VariableNames.Make, Value = expectedMake },
                    new VehicleAPIDataResult { Variable = VehicleLookupData.VariableNames.Model, Value = "IGNORED" },
                    new VehicleAPIDataResult { Variable = VehicleLookupData.VariableNames.ModelYear, Value = "0" }
                }
            };

            var fakeVPICHttpClient = A.Fake<IVPICHttpClient>();
            A.CallTo(() => fakeVPICHttpClient.DecodeVINAsync(testVIN)).Returns(okResponse);

            var fakeVehicleDataResponseParser = A.Fake<IVehicleDataLookupResponseParser>();
            A.CallTo(() => fakeVehicleDataResponseParser.ParseResponse(A<string>.Ignored)).Returns(resultData);

            var sut = new VehicleDataLookup(fakeVPICHttpClient, fakeVehicleDataResponseParser);

            // Act

            var data = await sut.GetVehicleDataByVINAsync(testVIN);

            // Assert

            A.CallTo(() => fakeVPICHttpClient.DecodeVINAsync(testVIN)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeVehicleDataResponseParser.ParseResponse(A<string>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedMake, data.Make);
        }

        [Fact]
        public async Task GetVehicleDataByVINAsync_ValidResponse_ReturnsDataWithExpectedModel()
        {
            // Arrange

            const string testVIN = "VIN";

            const string expectedModel = "118d";

            var okResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("") };

            var resultData = new VehicleAPIData
            {
                Results = new List<VehicleAPIDataResult>
                {
                    new VehicleAPIDataResult { Variable = VehicleLookupData.VariableNames.Make, Value = "IGNORED" },
                    new VehicleAPIDataResult { Variable = VehicleLookupData.VariableNames.Model, Value = expectedModel },
                    new VehicleAPIDataResult { Variable = VehicleLookupData.VariableNames.ModelYear, Value = "0" }
                }
            };

            var fakeVPICHttpClient = A.Fake<IVPICHttpClient>();
            A.CallTo(() => fakeVPICHttpClient.DecodeVINAsync(testVIN)).Returns(okResponse);

            var fakeVehicleDataResponseParser = A.Fake<IVehicleDataLookupResponseParser>();
            A.CallTo(() => fakeVehicleDataResponseParser.ParseResponse(A<string>.Ignored)).Returns(resultData);

            var sut = new VehicleDataLookup(fakeVPICHttpClient, fakeVehicleDataResponseParser);

            // Act

            var data = await sut.GetVehicleDataByVINAsync(testVIN);

            // Assert

            A.CallTo(() => fakeVPICHttpClient.DecodeVINAsync(testVIN)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeVehicleDataResponseParser.ParseResponse(A<string>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedModel, data.Model);
        }

        [Fact]
        public async Task GetVehicleDataByVINAsync_ValidResponse_ReturnsDataWithExpectedModelYear()
        {
            // Arrange

            const string testVIN = "VIN";

            const int expectedModelYear = 2016;

            var okResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("") };

            var resultData = new VehicleAPIData
            {
                Results = new List<VehicleAPIDataResult>
                {
                    new VehicleAPIDataResult { Variable = VehicleLookupData.VariableNames.Make, Value = "IGNORED" },
                    new VehicleAPIDataResult { Variable = VehicleLookupData.VariableNames.Model, Value = "IGNORED" },
                    new VehicleAPIDataResult { Variable = VehicleLookupData.VariableNames.ModelYear, Value = expectedModelYear.ToString() }
                }
            };

            var fakeVPICHttpClient = A.Fake<IVPICHttpClient>();
            A.CallTo(() => fakeVPICHttpClient.DecodeVINAsync(testVIN)).Returns(okResponse);

            var fakeVehicleDataResponseParser = A.Fake<IVehicleDataLookupResponseParser>();
            A.CallTo(() => fakeVehicleDataResponseParser.ParseResponse(A<string>.Ignored)).Returns(resultData);

            var sut = new VehicleDataLookup(fakeVPICHttpClient, fakeVehicleDataResponseParser);

            // Act

            var data = await sut.GetVehicleDataByVINAsync(testVIN);

            // Assert

            A.CallTo(() => fakeVPICHttpClient.DecodeVINAsync(testVIN)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeVehicleDataResponseParser.ParseResponse(A<string>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedModelYear, data.ModelYear);
        }
    }
}
