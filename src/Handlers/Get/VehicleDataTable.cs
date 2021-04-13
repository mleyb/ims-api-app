using Amazon.DynamoDBv2.DataModel;
using System;
using System.Threading.Tasks;

namespace Get
{
    public interface IVehicleDataTable
    {
        Task<VehicleDataTableItem> LoadAsync(string vin);
    }

    public class VehicleDataTable : IVehicleDataTable
    {
        private const string TableName = "IMS.VehicleData";

        private readonly IDynamoDBContext _db;

        public VehicleDataTable(IDynamoDBContext db)
        {
            _db = db;
        }

        public async Task<VehicleDataTableItem> LoadAsync(string vin)
        {
            var operationConfig = new DynamoDBOperationConfig
            {
                OverrideTableName = TableName
            };

            // ensure case consistency on key property
            vin = vin.ToUpper();

            VehicleDataTableItem item = await _db.LoadAsync<VehicleDataTableItem>(vin, operationConfig);

            return item;
        }
    }

    public class VehicleDataTableItem
    {
        [DynamoDBHashKey("Id")]
        public string VIN { get; set; }

        [DynamoDBProperty("CustomerId")]
        public int CustomerId { get; set; }

        [DynamoDBProperty("Make")]
        public string Make { get; set; }

        [DynamoDBProperty("Model")]
        public string Model { get; set; }

        [DynamoDBProperty("ModelYear")]
        public int ModelYear { get; set; }

        [DynamoDBProperty("ImportedDate")]
        public DateTime ImportedDate { get; set; }
    }
}
