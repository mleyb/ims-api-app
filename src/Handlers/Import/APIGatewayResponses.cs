using Amazon.Lambda.APIGatewayEvents;
using System.Net;

namespace Import
{
    internal static class APIGatewayProxyResponses
    {
        public static APIGatewayProxyResponse OK()
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }
    }
}
