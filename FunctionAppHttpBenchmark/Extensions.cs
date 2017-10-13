using System;
using System.Linq;
using System.Net.Http;

namespace FunctionAppHttpBenchmark
{
    static class Extensions
    {
        internal static long GetLongQueryStringParam(this HttpRequestMessage req, string paramName, int defaultValue)
        {
            string paramValue = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, paramName, true) == 0)
                .Value;

            if (paramValue == null)
            {
                return defaultValue;
            }

            return long.Parse(paramValue);
        }
    }
}
