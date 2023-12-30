using System;
using System.Text;
using System.Threading;

using nanoFramework.Presentation;
using nanoFramework.Presentation.Controls;

using OpenHalo.Moonraker;
using OpenHalo.Resources;

namespace OpenHalo.Windows.PrintingStates
{
    public class PrintingState : HaloWindow
    {
        private Text hotend;
        private Text heatbed;
        public PrintingState(OpenHaloApplication app) : base(app)
        {
        }

        public override void OnLoaded()
        {
            while (true)
            {
                if (hotend == null || heatbed == null)
                {
                    Thread.Sleep(100);
                    continue;
                }
                Dispatcher.Invoke(TimeSpan.MaxValue, (args) =>
                {
                    hotend.TextContent = string.Format("{0:000.00}", Query.data.status.extruder.temperature);
                    heatbed.TextContent = string.Format("{0:000.00}", Query.data.status.heater_bed.temperature);
                    return null;
                }, null);
                Thread.Sleep(500);
            }
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

            StackPanel temps = new StackPanel(Orientation.Horizontal);
            temps.Visibility = Visibility.Visible;

            hotend = new Text(OpenHaloApplication.NinaBFont, string.Format("{0:000.00}", Query.data.status.extruder.temperature));
            heatbed = new Text(OpenHaloApplication.NinaBFont, string.Format("{0:000.00}", Query.data.status.heater_bed.temperature));
            hotend.VerticalAlignment = VerticalAlignment.Center;
            hotend.HorizontalAlignment = HorizontalAlignment.Left;
            hotend.ForeColor = System.Drawing.Color.White;
            hotend.SetMargin(20, 10, 0, 0);

            heatbed.VerticalAlignment = VerticalAlignment.Center;
            heatbed.HorizontalAlignment = HorizontalAlignment.Left;
            heatbed.ForeColor = System.Drawing.Color.White;
            heatbed.SetMargin(120, 10, 0, 0);

            temps.Children.Add(hotend);
            temps.Children.Add(heatbed);

            stackPanel.Children.Add(temps);

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
        }
    }
}
