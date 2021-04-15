using System;
using System.Threading.Tasks;

namespace Get
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
        Task<VehicleData> GetVehicleDataAsync(string vin);
    }

    public class VehicleDataService : IVehicleDataService
    {
        private readonly IVehicleDataTable _table;

        public VehicleDataService(IVehicleDataTable table)
        {
            _table = table;
        }

        public async Task<VehicleData> GetVehicleDataAsync(string vin)
        {
            VehicleDataTableItem item = await _table.LoadAsync(vin);

            if (item == null)
            {
                return null;
            }

            var data = new VehicleData
            {
                VIN = item.VIN,
                CustomerId = item.CustomerId,
                Make = item.Make,
                Model = item.Model,
                ModelYear = item.ModelYear,
                ImportedDate = item.ImportedDate
            };

            return data;
        }
    }
}
