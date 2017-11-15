using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;

namespace FunctionAppHttpBenchmark
{
    public static class HttpTest
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("HttpTest")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            long memory = req.GetLongQueryStringParam("AllocInMB", 0);
            int sleep = (int)req.GetLongQueryStringParam("SleepInMS", 0);
            long iterations = req.GetLongQueryStringParam("LoopSpins", 0);
            int matrixSize = (int)req.GetLongQueryStringParam("MatrixSize", 0);
            int requests = (int)req.GetLongQueryStringParam("Requests", 0);
            var rnd = new Random();

            // Allocate memory and fill it with random bytes
            byte[] bytes = new byte[memory * 1024 * 1024];
            rnd.NextBytes(bytes);

            // Do an async sleep
            await Task.Delay(sleep);

            // Spin the CPU
            for (long i = 0; i < iterations; i++) { }

            // Do matrix multiplications
            if (matrixSize > 0)
            {
                Matrix.DoMatrixMultiplication(matrixSize);
            }

            // Make some outbound http requests in parallel
            if (requests > 0)
            {
                var tasks = new List<Task>();
                for (int i = 0; i < requests; i++)
                {
                    tasks.Add(client.GetAsync("http://microsoft.com"));
                }
                await Task.WhenAll(tasks);
            }

            var response = req.CreateResponse(HttpStatusCode.OK, "Done");
            response.Headers.Add("X-server", Environment.GetEnvironmentVariable("COMPUTERNAME"));
            return response;
        }
    }
}
