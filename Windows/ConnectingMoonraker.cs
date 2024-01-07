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
using nanoFramework.Json;
using OpenHalo.Moonraker;
using TekuSP.Drivers.DriverBase.Interfaces;
using OpenHalo.Windows.PrintingStates;
using OpenHalo.Windows.PrintingStates.Virtual;

namespace OpenHalo.Windows
{
    public class ConnectingMoonraker : HaloWindow
    {
        public ConnectingMoonraker(OpenHaloApplication application) : base(application)
        {
        }

        public override void OnLoaded()
        {
            if (!Query.IsQueryRunning())
                Query.StartQuery();
            while (true)
            {
                if (Query.data != null)
                {
                    switch (Query.data.status.print_stats.state)
                    {
                        case "standby":
                            Dispatcher.Invoke(TimeSpan.MaxValue, (args) =>
                            {
                                App.MainWindow = new StandbyState(App);
                                Close();
                                return null;
                            }, null);
                            return;
                        case "printing":
                            Dispatcher.Invoke(TimeSpan.MaxValue, (args) =>
                            {
                                App.MainWindow = new PrintingState(App);
                                Close();
                                return null;
                            }, null);
                            return;
                        case "paused":
                            Dispatcher.Invoke(TimeSpan.MaxValue, (args) =>
                            {
                                App.MainWindow = new PausedState(App);
                                Close();
                                return null;
                            }, null);
                            return;
                        case "complete":
                            Dispatcher.Invoke(TimeSpan.MaxValue, (args) =>
                            {
                                if (Query.data.status.heater_bed.temperature >= 30 && Query.data.status.heater_bed.power == 0)
                                    App.MainWindow = new CoolingDown(App);
                                else
                                    App.MainWindow = new CompleteState(App);
                                Close();
                                return null;
                            }, null);
                            return;
                        case "cancelled":
                            Dispatcher.Invoke(TimeSpan.MaxValue, (args) =>
                            {
                                App.MainWindow = new CancelledState(App);
                                Close();
                                return null;
                            }, null);
                            return;
                        case "error":
                            Dispatcher.Invoke(TimeSpan.MaxValue, (args) =>
                            {
                                App.MainWindow = new ErrorState(App);
                                Close();
                                return null;
                            }, null);
                            return;
                        default:
                            Console.WriteLine("Unknown Klipper state!");
                            return;
                    }
                }
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

        }
        protected override void OnButtonDown(ButtonEventArgs e)
        {
            if (e.Button == Button.VK_SELECT)
            {
                App.MainWindow.Dispatcher.Invoke(TimeSpan.MaxValue, (args) =>
                {
                    App.MainWindow = new Setup(App); //Open Setup
                    Close();
                    return null;
                }, null);
            }
        }
    }
}
