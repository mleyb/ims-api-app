using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace Get
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

        public static APIGatewayProxyResponse OKContent(object body)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = JsonConvert.SerializeObject(body),
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                }
            };
        }

        public static APIGatewayProxyResponse BadRequest()
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }

        public static APIGatewayProxyResponse NotFound(string id = null)
        {
            return new APIGatewayProxyResponse
            {
                Body = id,
                StatusCode = (int)HttpStatusCode.NotFound
            };
        }
    }
}
