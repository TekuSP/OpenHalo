using System;
using System.Text;

using nanoFramework.Presentation;
using nanoFramework.UI.Input;
using nanoFramework.UI;
using nanoFramework.Presentation.Controls;
using OpenHalo.Resources;

namespace OpenHalo.Windows
{
    public class ConnectingWIFI : Window
    {
        private OpenHaloApplication App
        {
            get; set;
        }
        public ConnectingWIFI(OpenHaloApplication application)
        {
            App = application;
            Visibility = Visibility.Visible;
            Width = DisplayControl.ScreenWidth;
            Height = DisplayControl.ScreenHeight;
            Buttons.Focus(this);
            StackPanel panel = new StackPanel();
            Child = panel;

            Background = new nanoFramework.Presentation.Media.SolidColorBrush(System.Drawing.Color.Black);
            Foreground = new nanoFramework.Presentation.Media.SolidColorBrush(System.Drawing.Color.White);
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

            Image logo = new Image(ResourceDictionary.GetBitmap(ResourceDictionary.BitmapResources.wifi));
            logo.HorizontalAlignment = HorizontalAlignment.Center;
            logo.Visibility = Visibility.Visible;
            stackPanel.Children.Add(logo);

            Text settText = new Text(OpenHaloApplication.SmallFont, "To interrupt startup and enter settings,");
            settText.VerticalAlignment = VerticalAlignment.Center;
            settText.HorizontalAlignment = HorizontalAlignment.Center;
            settText.ForeColor = System.Drawing.Color.White;
            settText.SetMargin(0, 10, 0, 0);
            stackPanel.Children.Add(settText);

            Text touchText = new Text(OpenHaloApplication.SmallFont, "touch the screen at any point.");
            touchText.VerticalAlignment = VerticalAlignment.Center;
            touchText.HorizontalAlignment = HorizontalAlignment.Center;
            touchText.ForeColor = System.Drawing.Color.White;
            stackPanel.Children.Add(touchText);

            Text connnectingText = new Text(OpenHaloApplication.SmallFont, "Connecting to WiFi:");
            connnectingText.VerticalAlignment = VerticalAlignment.Center;
            connnectingText.HorizontalAlignment = HorizontalAlignment.Center;
            connnectingText.ForeColor = System.Drawing.Color.White;
            connnectingText.SetMargin(0, 10, 0, 0);
            stackPanel.Children.Add(connnectingText);

            Text wifiText = new Text(OpenHaloApplication.SmallFont, "");
            wifiText.VerticalAlignment = VerticalAlignment.Center;
            wifiText.HorizontalAlignment = HorizontalAlignment.Center;
            wifiText.ForeColor = System.Drawing.Color.White;
            stackPanel.Children.Add(wifiText);

            Text atttemptText = new Text(OpenHaloApplication.SmallFont, "");
            atttemptText.VerticalAlignment = VerticalAlignment.Center;
            atttemptText.HorizontalAlignment = HorizontalAlignment.Center;
            atttemptText.ForeColor = System.Drawing.Color.White;
            stackPanel.Children.Add(atttemptText);

            Console.WriteLine("Wifi Connections init....");
            Configs.Wifi pernament = null;
            while (WifiNetworkHelper.Status != NetworkHelperStatus.NetworkIsReady)
            {
                foreach (var item in OpenHaloApplication.config.Wifis)
                {
                    int tries = 5;
                    wifiText.Dispatcher.Invoke(TimeSpan.MaxValue, (args) => { wifiText.TextContent = item.SSID; atttemptText.TextContent = "Attempt: " + (tries - 5 + 1); return null; }, null);
                    pernament = item;
                    Console.WriteLine("Trying: " + item.SSID);
                    WifiNetworkHelper.ScanAndConnectDhcp(item.SSID, item.Password, System.Device.Wifi.WifiReconnectionKind.Manual);
                    while (WifiNetworkHelper.Status != NetworkHelperStatus.NetworkIsReady)
                    {
                        tries--;
                        Console.WriteLine($"Network not ready, status: {Diagnostics.GetNetworkStatus(WifiNetworkHelper.Status)}");
                        WifiNetworkHelper.ScanAndConnectDhcp(item.SSID, item.Password, System.Device.Wifi.WifiReconnectionKind.Manual);
                        Thread.Sleep(1500);
                        if (tries <= 0)
                        {
                            break;
                        }
                    }
                }
            }
            WifiNetworkHelper.ScanAndConnectDhcp(pernament.SSID, pernament.Password, System.Device.Wifi.WifiReconnectionKind.Automatic);
            Console.WriteLine("WIFI Connected");
            var networkInterface = NetworkInterface.GetAllNetworkInterfaces()[WifiNetworkHelper.WifiAdapter.NetworkInterface];
            Console.WriteLine("IP: " + networkInterface.IPv4Address);
            Console.WriteLine("DNS: " + networkInterface.IPv4DnsAddresses[0]);
            Console.WriteLine("Gateway: " + networkInterface.IPv4GatewayAddress);
            Console.WriteLine("Networking started!");
            Console.WriteLine("Initializing SNTP...");
            Sntp.Server1 = "tik.cesnet.cz";
            Sntp.Server2 = "pool.ntp.org";
            Sntp.Start();
            Console.WriteLine("SNTP Initialized!");
            Console.WriteLine("UTC NOW: " + DateTime.UtcNow.ToString());
            while (DateTime.UtcNow.Year == 1970)
            {
                Console.WriteLine("Waiting for SNTP....");
                Console.WriteLine("UTC NOW: " + DateTime.UtcNow.ToString());
                Sntp.UpdateNow();
                Thread.Sleep(1250);
            }
            Console.WriteLine("SNTP fully ready!");
            App.MainWindow = new ConnectingMoonraker(App);
            Close();
        }
    }
}
