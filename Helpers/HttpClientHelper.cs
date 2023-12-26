using System;
using System.Net.Http;
using System.Text;

namespace OpenHalo.Helpers
{
    public static class HttpClientHelper
    {
        public static HttpClient HttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Api-Key", OpenHaloApplication.config.MoonrakerApiKey);
            return client;
        }
    }
}
