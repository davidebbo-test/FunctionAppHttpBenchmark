using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;

namespace FunctionAppHttpBenchmark
{
    public static class HttpTest
    {
        [FunctionName("HttpTest")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            long memory = req.GetLongQueryStringParam("AllocInMB", 0);
            int sleep = (int)req.GetLongQueryStringParam("SleepInMS", 0);
            long iterations = req.GetLongQueryStringParam("LoopSpins", 0);
            var rnd = new Random();

            // Allocate memory and fill it with random bytes
            byte[] bytes = new byte[memory * 1024 * 1024];
            rnd.NextBytes(bytes);

            // Do an async sleep
            await Task.Delay(sleep);

            // Spin the CPU
            for (long i = 0; i < iterations; i++) { }

            var response = req.CreateResponse(HttpStatusCode.OK, "Done");
            response.Headers.Add("X-server", "aaa");
            return response;
        }
    }
}
