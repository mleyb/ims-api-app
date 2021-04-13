using FakeItEasy;
using System;
using Xunit;

namespace Get.Tests
{
    public class VehicleDataServiceTest
    {
        [Fact]
        public void GetVehicleDataAsync_ValidArg_ReturnsExpectedData()
        {
            var fakeVehicleDataTable = A.Fake<IVehicleDataTable>();

            VehicleDataTableItem item = new VehicleDataTableItem
            {
                CustomerId = 1,
                VIN = "VIN",
                Make = "BMW",
                Model = "118d",
                ModelYear = 2016,
                ImportedDate = DateTime.UtcNow
            };


            // Act

            var sut = new VehicleDataService(fakeVehicleDataTable);

            //Assert


        }
    }
}
