using System;
using System.Text;

using nanoFramework.Presentation;
using nanoFramework.UI.Input;
using nanoFramework.UI;
using nanoFramework.Presentation.Controls;
using OpenHalo.Resources;
using nanoFramework.Networking;
using System.Net.NetworkInformation;
using Iot.Device.DhcpServer;
using System.Threading;
using System.Net;
using nanoFramework.Runtime.Native;
using OpenHalo.Helpers;
using nanoFramework.WebServer;
using OpenHalo.Resources.Setup_Webpage;
using nanoFramework.Json;

namespace OpenHalo.Windows
{
    public class Setup : HaloWindow
    {
        public const string SoftApIP = "192.168.4.1";
        public static WebServer WebServer;
        public static string NearbyAPs;
        public Setup(OpenHaloApplication application) : base(application)
        {

        }

        public override void OnLoaded()
        {
            Console.WriteLine($"Starting SSID OpenHalo Setup on {SoftApIP}");

            if (!Networking.IsAPModeEnabled() || !Networking.IsModeValid())
                Networking.EnableAPMode(this, SoftApIP);

            Console.WriteLine("SSID Running!");

            Console.WriteLine("Starting DHCP Server....");
            DhcpServer dhcpServer = new DhcpServer();
            dhcpServer.CaptivePortalUrl = $"http://{SoftApIP}";
            var result = dhcpServer.Start(IPAddress.Parse(SoftApIP), new IPAddress(new byte[] { 255, 255, 255, 0 }), 1200);
            if (result)
                Console.WriteLine("DHCP Server running!");
            else
                Console.WriteLine("Error, DHCP Server not started!");
            Console.WriteLine("Starting Web Server");
            if (WebServer == null)
                WebServer = new WebServer(80, HttpProtocol.Http, new Type[] { typeof(WebController)} );
            WebServer.Start();

            var adapter = Networking.GetAdapter();
            adapter.AvailableNetworksChanged += Adapter_AvailableNetworksChanged;
            while (true)
            {
                try
                {
                    Console.WriteLine("Trying scan....");
                    adapter.ScanAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception scanning...");
                    Console.WriteLine(ex.ToString());
                }
                finally { Thread.Sleep(10000); }
            }
        }

        private void Adapter_AvailableNetworksChanged(System.Device.Wifi.WifiAdapter sender, object e)
        {
            NearbyAPs = JsonConvert.SerializeObject(sender.NetworkReport.AvailableNetworks);
        }

        public override void RenderElements()
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.SetMargin(0, 30, 0, 0);
            Child = stackPanel;
            stackPanel.Visibility = Visibility.Visible;

            Text logoText = new Text(OpenHaloApplication.NinaBFont, "OpenHalo");
            logoText.VerticalAlignment = VerticalAlignment.Center;
            logoText.HorizontalAlignment = HorizontalAlignment.Center;
            logoText.ForeColor = System.Drawing.Color.White;
            logoText.Visibility = Visibility.Visible;
            logoText.SetMargin(0, 0, 0, 10);
            stackPanel.Children.Add(logoText);

            Image logo = new Image(ResourceDictionary.GetBitmap(ResourceDictionary.BitmapResources.wifiConnect));
            logo.HorizontalAlignment = HorizontalAlignment.Center;
            logo.Visibility = Visibility.Visible;
            stackPanel.Children.Add(logo);

            Text settText = new Text(OpenHaloApplication.NinaBFont, "SSID: OpenHalo Setup");
            settText.VerticalAlignment = VerticalAlignment.Center;
            settText.HorizontalAlignment = HorizontalAlignment.Center;
            settText.ForeColor = System.Drawing.Color.White;
            settText.SetMargin(5, 15, 5, 5);
            stackPanel.Children.Add(settText);


            Text touchText = new Text(OpenHaloApplication.NinaBFont, "PSWD: OpenHalo");
            touchText.VerticalAlignment = VerticalAlignment.Center;
            touchText.HorizontalAlignment = HorizontalAlignment.Center;
            touchText.ForeColor = System.Drawing.Color.White;
            touchText.SetMargin(5);
            stackPanel.Children.Add(touchText);

            Text urlText = new Text(OpenHaloApplication.NinaBFont, "URL: " + SoftApIP);
            urlText.VerticalAlignment = VerticalAlignment.Center;
            urlText.HorizontalAlignment = HorizontalAlignment.Center;
            urlText.ForeColor = System.Drawing.Color.White;
            urlText.SetMargin(5);
            stackPanel.Children.Add(urlText);
        }

        private void Reboot()
        {
            Console.WriteLine("Requested reboot to normal operation mode...");
            Networking.EnableClientMode(this); //Enables client mode and reboots
        }
        protected override void OnButtonDown(ButtonEventArgs e)
        {
            if (e.Button == Button.VK_BACK)
            {
                Reboot();
            }
        }
    }
}
