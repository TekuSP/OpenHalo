using System;
using System.Text;

using nanoFramework.Presentation;
using nanoFramework.Presentation.Controls;
using nanoFramework.UI.Input;
using nanoFramework.UI;

using OpenHalo.Resources;
using System.Threading;

namespace OpenHalo.Windows
{
    public class Reboot : Window
    {
        private OpenHaloApplication App
        {
            get; set;
        }
        public Reboot(OpenHaloApplication application)
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

            Image logo = new Image(ResourceDictionary.GetBitmap(ResourceDictionary.BitmapResources.reboot));
            logo.HorizontalAlignment = HorizontalAlignment.Center;
            logo.Visibility = Visibility.Visible;
            stackPanel.Children.Add(logo);

            Text connnectingText = new Text(OpenHaloApplication.SmallFont, "System is ready to reboot!");
            connnectingText.VerticalAlignment = VerticalAlignment.Center;
            connnectingText.HorizontalAlignment = HorizontalAlignment.Center;
            connnectingText.ForeColor = System.Drawing.Color.White;
            connnectingText.SetMargin(0, 10, 0, 0);
            stackPanel.Children.Add(connnectingText);

            var reboot = new Thread(() =>
            {
                Thread.Sleep(2500);
                nanoFramework.Runtime.Native.Power.RebootDevice();
            });
            reboot.Start();
        }
    }
}
