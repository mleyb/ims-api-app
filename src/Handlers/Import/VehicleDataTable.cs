using Amazon.DynamoDBv2.DataModel;
using System;
using System.Threading.Tasks;

namespace Import
{
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

    public interface IVehicleDataTable
    {
        Task SaveAsync(VehicleDataTableItem item);
    }

    public class VehicleDataTable : IVehicleDataTable
    {
        private const string TableName = "IMS.VehicleData"; // TODO - get from env

        private readonly IDynamoDBContext _db;

        public VehicleDataTable(IDynamoDBContext db)
        {
            _db = db;
        }

        public async Task SaveAsync(VehicleDataTableItem item)
        {
            var operationConfig = new DynamoDBOperationConfig
            {
                OverrideTableName = TableName
            };

            // ensure case consistency on key property
            item.VIN = item.VIN.ToUpper();

            await _db.SaveAsync(item, operationConfig);
        }
    }
}
