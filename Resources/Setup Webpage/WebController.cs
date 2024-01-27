using System;

using nanoFramework.Json;
using nanoFramework.WebServer;


namespace OpenHalo.Resources.Setup_Webpage
{
    public class WebController
    {
        [Route("/")]
        [Authentication("None")]
        [Method("GET")]
        public void Index(WebServerEventArgs e)
        {
            WebServer.SendFileOverHTTP(e.Context.Response, "index.html", Resources.ResourceDictionary.GetBytes(ResourceDictionary.BinaryResources.index), "text/html");
        }
    }
}
