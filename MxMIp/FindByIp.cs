using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using MaxMind.Db;
using MaxMind.GeoIP2;
using System;

namespace MxMIp
{
    public static class FindByIp
    {
        [FunctionName("FindByIp")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log, ExecutionContext context)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string ip = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "ip", true) == 0)
                .Value;
            string mmdbPath = System.IO.Path.Combine(context.FunctionAppDirectory , Environment.GetEnvironmentVariable("mmdbFilePath"));
            log.Info(mmdbPath);
            var reader = new DatabaseReader(mmdbPath);
            var response = reader.City(ip);
            return ip == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass IP Address 81.2.69.160")
                : req.CreateResponse(HttpStatusCode.OK, "Response Is" + response);
        
        }
    }
}
