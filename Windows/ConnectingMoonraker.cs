using System;
using System.Text;

using nanoFramework.Presentation;
using nanoFramework.Presentation.Controls;
using nanoFramework.UI.Input;
using nanoFramework.UI;
using OpenHalo.Resources;
using System.Net.Http;
using System.Threading;
using OpenHalo.Helpers;

namespace OpenHalo.Windows
{
    public class ConnectingMoonraker : Window
    {
        private OpenHaloApplication App
        {
            get; set;
        }
        public ConnectingMoonraker(OpenHaloApplication application)
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

            Image logo = new Image(ResourceDictionary.GetBitmap(ResourceDictionary.BitmapResources.moonraker));
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

            Text connnectingText = new Text(OpenHaloApplication.SmallFont, "Connecting to Moonraker:");
            connnectingText.VerticalAlignment = VerticalAlignment.Center;
            connnectingText.HorizontalAlignment = HorizontalAlignment.Center;
            connnectingText.ForeColor = System.Drawing.Color.White;
            connnectingText.SetMargin(0, 10, 0, 0);
            stackPanel.Children.Add(connnectingText);

            Text wifiText = new Text(OpenHaloApplication.SmallFont, OpenHaloApplication.config.MoonrakerUri);
            wifiText.VerticalAlignment = VerticalAlignment.Center;
            wifiText.HorizontalAlignment = HorizontalAlignment.Center;
            wifiText.ForeColor = System.Drawing.Color.White;
            stackPanel.Children.Add(wifiText);

            Thread th = new Thread(() =>
            {
                var client = HttpClientHelper.HttpClient();
                var str = client.GetString($"http://{OpenHaloApplication.config.MoonrakerUri}/printer/objects/query?webhooks&virtual_sdcard&print_stats");
                Console.WriteLine(str);
            });
            th.Start();
        }
    }
}
