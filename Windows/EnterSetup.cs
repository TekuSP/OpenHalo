using System;
using System.Text;

using nanoFramework.Presentation;
using nanoFramework.UI.Input;
using nanoFramework.UI;
using nanoFramework.Presentation.Controls;
using OpenHalo.Resources;
using System.Threading;

namespace OpenHalo.Windows
{
    public class EnterSetup : Window
    {
        private Text countdownText;
        private Timer timer;
        private OpenHaloApplication App
        {
            get; set;
        }
        public EnterSetup(OpenHaloApplication application)
        {
            App = application;
            Visibility = Visibility.Visible;
            Width = DisplayControl.ScreenWidth;
            Height = DisplayControl.ScreenHeight;
            Buttons.Focus(this);

            Background = new nanoFramework.Presentation.Media.SolidColorBrush(System.Drawing.Color.Black);
            Foreground = new nanoFramework.Presentation.Media.SolidColorBrush(System.Drawing.Color.White);
            StackPanel stackPanel = new StackPanel();
            stackPanel.SetMargin(0,30,0,0);
            Child = stackPanel;
            stackPanel.Visibility = Visibility.Visible;

            Text logoText = new Text(OpenHaloApplication.NinaBFont,"OpenHalo");
            logoText.VerticalAlignment = VerticalAlignment.Center;
            logoText.HorizontalAlignment = HorizontalAlignment.Center;
            logoText.ForeColor = System.Drawing.Color.White;
            logoText.Visibility = Visibility.Visible;
            logoText.SetMargin(0, 0, 0, 10);
            stackPanel.Children.Add(logoText);

            Image logo = new Image(ResourceDictionary.GetBitmap(ResourceDictionary.BitmapResources.settings));
            logo.HorizontalAlignment = HorizontalAlignment.Center;
            logo.Visibility = Visibility.Visible;
            stackPanel.Children.Add(logo);

            Text settText = new Text(OpenHaloApplication.SmallFont,"To interrupt startup and enter settings,");
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

            countdownText = new Text(OpenHaloApplication.SmallFont, "Continues boot in 10 seconds.");
            countdownText.ForeColor = System.Drawing.Color.White;
            countdownText.VerticalAlignment = VerticalAlignment.Center;
            countdownText.HorizontalAlignment = HorizontalAlignment.Center;
            countdownText.SetMargin(0, 20, 0, 0);
            stackPanel.Children.Add(countdownText);
            int tries = 10;
            timer = new Timer((args) =>
            {
                tries--;
                countdownText.Dispatcher.Invoke(TimeSpan.MaxValue, (args) =>
                {
                    countdownText.TextContent = $"Continues boot in {tries} seconds.";
                    return null;
                }, null);
                if (tries <= 0)
                {
                    App.MainWindow.Dispatcher.Invoke(TimeSpan.MaxValue, (args) =>
                    {
                        timer.Dispose();
                        timer = null;
                        App.MainWindow = new ConnectingWIFI(App); //Open Connecting WIFI Dialog
                        Close();
                        return null;
                    }, null);
                }
            }, null, 0, 1000);
        }
        protected override void OnButtonDown(ButtonEventArgs e)
        {
            App.MainWindow = new Setup(App); //Open Setup
            Close();
        }
    }
}
