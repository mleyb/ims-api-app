using FakeItEasy;
using System.Threading.Tasks;
using Xunit;

namespace Import.Tests
{
    public class VehicleDataServiceTest
    {
        [Fact]
        public async Task ImportVehicleDataAsync_ValidArgs_MustLookupVehicleData()
        {
            // Arrange

            const int customerId = 1;
            const string vin = "VIN";

            var lookupData = new VehicleLookupData
            {
                Make = "IGNORED",
                Model = "IGNORED",
                ModelYear = 0
            };

            var fakeVehicleDataLookup = A.Fake<IVehicleDataLookup>();
            A.CallTo(() => fakeVehicleDataLookup.GetVehicleDataByVINAsync(vin)).Returns(lookupData);

            var fakeVehicleDataTable = A.Fake<IVehicleDataTable>();

            var sut = new VehicleDataService(fakeVehicleDataLookup, fakeVehicleDataTable);

            // Act

            await sut.ImportVehicleDataAsync(customerId, vin);

            // Assert

            A.CallTo(() => fakeVehicleDataTable.SaveAsync(A<VehicleDataTableItem>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ImportVehicleDataAsync_ValidArgs_MustSaveVehicleDataToTable()
        {
            // Arrange

            const int customerId = 1;
            const string vin = "VIN";

            var lookupData = new VehicleLookupData
            {
                Make = "IGNORED",
                Model = "IGNORED",
                ModelYear = 0
            };

            var fakeVehicleDataLookup = A.Fake<IVehicleDataLookup>();
            A.CallTo(() => fakeVehicleDataLookup.GetVehicleDataByVINAsync(vin)).Returns(lookupData);

            var fakeVehicleDataTable = A.Fake<IVehicleDataTable>();

            var sut = new VehicleDataService(fakeVehicleDataLookup, fakeVehicleDataTable);

            // Act

            await sut.ImportVehicleDataAsync(customerId, vin);

            // Assert

            A.CallTo(() => fakeVehicleDataTable.SaveAsync(A<VehicleDataTableItem>.Ignored)).MustHaveHappenedOnceExactly();
        }
    }
}
