using System;
using System.Net.Http;
using System.Text;

namespace OpenHalo.Helpers
{
    /// <summary>
    /// HTTP Helpers
    /// </summary>
    public static class HttpHelpers
    {
        /// <summary>
        /// HttpClient which contains Api Key for Moonraker
        /// </summary>
        /// <returns>Moonraker valid Http Client</returns>
        public static HttpClient HttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Api-Key", OpenHaloApplication.config.MoonrakerApiKey);
            return client;
        }
    }
}
