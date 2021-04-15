using FakeItEasy;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Get.Tests
{
    public class VehicleDataServiceTest
    {
        [Fact]
        public async Task GetVehicleDataAsync_ValidInput_ReturnsExpectedVehicleData()
        {
            const string vin = "VIN";

            VehicleDataTableItem item = new VehicleDataTableItem
            {
                CustomerId = 1,
                VIN = vin,
                Make = "BMW",
                Model = "118d",
                ModelYear = 2016,
                ImportedDate = DateTime.UtcNow
            };

            var fakeVehicleDataTable = A.Fake<IVehicleDataTable>();
            A.CallTo(() => fakeVehicleDataTable.LoadAsync(vin)).Returns(item);

            var sut = new VehicleDataService(fakeVehicleDataTable);

            // Act

            VehicleData data = await sut.GetVehicleDataAsync(vin);

            // Assert

            Assert.Equal(item.CustomerId, data.CustomerId);
            Assert.Equal(vin, data.VIN);
            Assert.Equal(item.Make, data.Make);
            Assert.Equal(item.Model, data.Model);
            Assert.Equal(item.ModelYear, data.ModelYear);
            Assert.Equal(item.ImportedDate, data.ImportedDate);
        }
    }
}
