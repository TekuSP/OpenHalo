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
    public class Reboot : HaloWindow
    {
        public Reboot(OpenHaloApplication application) : base(application)
        {
        }

        public override void OnLoaded()
        {
            Thread.Sleep(5000);
            OpenHaloApplication.SetBrightness(0); //Shutdown screen
            nanoFramework.Runtime.Native.Power.RebootDevice();
        }
        public override void RenderElements()
        {
            StackPanel panel = new StackPanel();
            Child = panel;

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
        }
    }
}
