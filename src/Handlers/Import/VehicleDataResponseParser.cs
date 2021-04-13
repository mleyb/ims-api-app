using Newtonsoft.Json;
using System.Collections.Generic;

namespace Import
{
    public class VehicleAPIData
    {
        [JsonProperty("Count")]
        public int Count { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("SearchCriteria")]
        public string SearchCriteria { get; set; }

        [JsonProperty("Results")]
        public List<VehicleAPIDataResult> Results { get; set; }
    }

    public class VehicleAPIDataResult
    {
        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("ValueId")]
        public string ValueId { get; set; }

        [JsonProperty("Variable")]
        public string Variable { get; set; }

        [JsonProperty("VariableId")]
        public int VariableId { get; set; }
    }

    public interface IVehicleDataLookupResponseParser
    {
        VehicleAPIData ParseResponse(string json);
    }

    public class VehicleDataLookupResponseParser : IVehicleDataLookupResponseParser
    {
        public VehicleAPIData ParseResponse(string json)
        {
            VehicleAPIData data = JsonConvert.DeserializeObject<VehicleAPIData>(json);

            return data;
        }
    }
}
