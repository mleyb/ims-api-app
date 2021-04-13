using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Import
{
    public interface IVPICHttpClient
    {
        Task<HttpResponseMessage> DecodeVINAsync(string vin);
    }

    public class VPICHttpClient : IVPICHttpClient
    {
        private const string RequestUriTemplate = "https://vpic.nhtsa.dot.gov/api/vehicles/DecodeVIN/{0}?format=json";

        private static readonly HttpClient _httpClient = new HttpClient();

        public async Task<HttpResponseMessage> DecodeVINAsync(string vin)
        {
            string requestUri = String.Format(RequestUriTemplate, vin);

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            return response;
        }
    }
}
