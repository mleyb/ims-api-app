using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace Get
{
    public class GetHandler
    {
        private readonly IServiceProvider _sp;

        public GetHandler()
        {
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            _sp = serviceCollection.BuildServiceProvider();
        }

        public GetHandler(IServiceProvider sp)
        {
            _sp = sp;
        }

        public async Task<APIGatewayProxyResponse> HandleRequestAsync(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine($"APIGateway request: {JsonConvert.SerializeObject(request)}");

            string vin = WebUtility.UrlDecode(request.PathParameters["id"]);

            IVehicleDataService dataService = _sp.GetRequiredService<IVehicleDataService>();

            VehicleData data = await dataService.GetVehicleDataAsync(vin);

            if (data == null)
            {
                return APIGatewayProxyResponses.NotFound();
            }

            var body = new ResponseBody
            {
                VIN = data.VIN,
                CustomerId = data.CustomerId,
                Make = data.Make,
                Model = data.Model,
                ModelYear = data.ModelYear,
                ImportedDate = data.ImportedDate
            };

            return APIGatewayProxyResponses.OKContent(body);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IVehicleDataService, VehicleDataService>();
            services.AddTransient<IVehicleDataTable, VehicleDataTable>();

            services.AddSingleton<IDynamoDBContext, DynamoDBContext>(sp => new DynamoDBContext(new AmazonDynamoDBClient()));
        }
    }

    public class ResponseBody
    {
        public int CustomerId { get; set; }

        public string VIN { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public int ModelYear { get; set; }

        public DateTime ImportedDate { get; set; }
    }
}