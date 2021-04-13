using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Import
{
    public class VehicleLookupData
    {
        public static class VariableNames
        {
            public const string Make = "Make";
            public const string Model = "Model";
            public const string ModelYear = "Model Year";
        }

        public string Make { get; set; }

        public string Model { get; set; }

        public int ModelYear { get; set; }
    }

    public interface IVehicleDataLookup
    {
        Task<VehicleLookupData> GetVehicleDataByVINAsync(string vin);
    }

    public class VehicleDataLookup : IVehicleDataLookup
    {
        private readonly IVPICHttpClient _vpicHttpClient;
        private readonly IVehicleDataLookupResponseParser _responseParser;

        public VehicleDataLookup(IVPICHttpClient vpicHttpClient, IVehicleDataLookupResponseParser responseParser)
        {
            _vpicHttpClient = vpicHttpClient;
            _responseParser = responseParser;
        }

        public async Task<VehicleLookupData> GetVehicleDataByVINAsync(string vin)
        {
            HttpResponseMessage response = await _vpicHttpClient.DecodeVINAsync(vin);

            response.EnsureSuccessStatusCode();

            VehicleAPIData data = _responseParser.ParseResponse(await response.Content.ReadAsStringAsync());

            string make = data.Results.Single(r => r.Variable == VehicleLookupData.VariableNames.Make).Value;
            string model = data.Results.Single(r => r.Variable == VehicleLookupData.VariableNames.Model).Value;
            string modelYear = data.Results.Single(r => r.Variable == VehicleLookupData.VariableNames.ModelYear).Value;

            var lookupData = new VehicleLookupData
            {
                Make = make,
                Model = model,
                ModelYear = Int32.Parse(modelYear)
            };

            return lookupData;
        }
    }
}
