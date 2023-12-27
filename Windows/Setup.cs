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

namespace OpenHalo.Windows
{
    public class Setup : Window
    {
        public const string SoftApIP = "192.168.4.1";
        private OpenHaloApplication App
        {
            get; set;
        }
        public Setup(OpenHaloApplication application)
        {
            App = application;
            Visibility = Visibility.Visible;
            Width = DisplayControl.ScreenWidth;
            Height = DisplayControl.ScreenHeight;
            Buttons.Focus(this);

            Background = new nanoFramework.Presentation.Media.SolidColorBrush(System.Drawing.Color.Black);
            Foreground = new nanoFramework.Presentation.Media.SolidColorBrush(System.Drawing.Color.White);

            Console.WriteLine($"Starting SSID OpenHalo Setup on {SoftApIP}");

            if (!Networking.IsAPModeEnabled() || !Networking.IsModeValid())
                Networking.EnableAPMode(this, App, SoftApIP);

            Console.WriteLine("SSID Running!");

            Console.WriteLine("Starting DHCP Server....");
            DhcpServer dhcpServer = new DhcpServer();
            dhcpServer.CaptivePortalUrl = $"http://{SoftApIP}";
            dhcpServer.Start(IPAddress.Parse(SoftApIP), new IPAddress(new byte[] { 255, 255, 255, 0 }), 1200);
            Console.WriteLine("DHCP Server running!");

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
    }
}
