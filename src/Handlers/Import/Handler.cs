using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace Import
{
    public class ImportHandler
    {
        private readonly IServiceProvider _sp;

        public ImportHandler()
        {
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            _sp = serviceCollection.BuildServiceProvider();
        }

        public ImportHandler(IServiceProvider sp)
        {
            _sp = sp;
        }

        public async Task<APIGatewayProxyResponse> HandleRequestAsync(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine($"APIGateway request: {JsonConvert.SerializeObject(request)}");

            ImportRequest requestBody = JsonConvert.DeserializeObject<ImportRequest>(request.Body);

            IValidateVIN validateVIN = _sp.GetRequiredService<IValidateVIN>();

            if (!validateVIN.IsValid(requestBody.VIN))
            {
                return APIGatewayProxyResponses.BadRequest();
            }

            IVehicleDataService dataService = _sp.GetRequiredService<IVehicleDataService>();

            await dataService.ImportVehicleDataAsync(requestBody.CustomerId, requestBody.VIN);

            return APIGatewayProxyResponses.OK();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IVehicleDataService, VehicleDataService>();
            services.AddTransient<IVehicleDataLookup, VehicleDataLookup>();
            services.AddTransient<IVPICHttpClient, VPICHttpClient>();
            services.AddTransient<IVehicleDataLookupResponseParser, VehicleDataLookupResponseParser>();
            services.AddTransient<IVehicleDataTable, VehicleDataTable>();
            services.AddTransient<IValidateVIN, VINValidator>();

            services.AddSingleton<IDynamoDBContext, DynamoDBContext>(sp => new DynamoDBContext(new AmazonDynamoDBClient()));
        }
    }

    public class ImportRequest
    {
        public int CustomerId { get; set; }

        public string VIN { get; set; }
    }
}
