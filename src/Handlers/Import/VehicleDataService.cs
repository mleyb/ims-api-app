using System;
using System.Threading.Tasks;

namespace Import
{
    public class VehicleData
    {
        public int CustomerId { get; set; }

        public string VIN { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public int ModelYear { get; set; }

        public DateTime ImportedDate { get; set; }
    }

    public interface IVehicleDataService
    {
        Task ImportVehicleDataAsync(int customerId, string vin);
    }

    public class VehicleDataService : IVehicleDataService
    {
        private readonly IVehicleDataLookup _lookup;
        private readonly IVehicleDataTable _table;

        public VehicleDataService(IVehicleDataLookup lookup, IVehicleDataTable table)
        {
            _lookup = lookup;
            _table = table;
        }

        public async Task ImportVehicleDataAsync(int customerId, string vin)
        {
            VehicleLookupData lookupData = await _lookup.GetVehicleDataByVINAsync(vin);

            var item = new VehicleDataTableItem
            {
                VIN = vin,
                CustomerId = customerId,
                Make = lookupData.Make,
                Model = lookupData.Model,
                ModelYear = lookupData.ModelYear,
                ImportedDate = DateTime.UtcNow
            };

            await _table.SaveAsync(item);
        }
    }
}
