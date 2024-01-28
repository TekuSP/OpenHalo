using System;
using System.Text;
using nanoFramework.WebServer;

using OpenHalo.Helpers;
using OpenHalo.Windows;


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
        [Route("/LoadData")]
        [Authentication("None")]
        [Method("GET")]
        public void LoadData(WebServerEventArgs e)
        {
            WebServer.SendFileOverHTTP(e.Context.Response, "data.json", Encoding.UTF8.GetBytes(ConfigHelper.LoadConfigJSON() ?? ""), "application/json");
        }
        [Route("/NearbyNetworks")]
        [Authentication("None")]
        [Method("GET")]
        public void NearbyNetworks(WebServerEventArgs e)
        {
            WebServer.SendFileOverHTTP(e.Context.Response, "networks.json", Encoding.UTF8.GetBytes(Setup.NearbyAPs ?? ""), "application/json");
        }
        [Route("/SaveData")]
        [Authentication("None")]
        [Method("POST")]
        public void SaveData(WebServerEventArgs e)
        {
            try
            {
                ConfigHelper.SaveConfigJSON(e.Context.Request.Headers["data"]);
                WebServer.OutputHttpCode(e.Context.Response, System.Net.HttpStatusCode.OK);
                Console.WriteLine("Requested reboot to normal operation mode...");
                Networking.EnableClientMode(); //Enables client mode and reboots
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Exception happened when saving new config!");
                Console.WriteLine(ex.ToString());
                WebServer.OutputHttpCode(e.Context.Response, System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
