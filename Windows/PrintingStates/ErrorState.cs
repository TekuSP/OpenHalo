using System;
using System.Text;

using nanoFramework.Presentation.Controls;
using nanoFramework.Presentation.Media;
using nanoFramework.Presentation.Shapes;
using nanoFramework.Presentation;
using OpenHalo.Moonraker;
using OpenHalo.Resources;
using OpenHalo.Windows.PrintingStates.Virtual;

namespace OpenHalo.Windows.PrintingStates
{
    public class ErrorState : HaloWindow
    {
        private string currentState;
        public ErrorState(OpenHaloApplication app) : base(app)
        {
        }
        private void Query_dataChanged(object sender, EventArgs e)
        {
            if (currentState != Query.data.status.print_stats.state)
            {
                Query.dataChanged -= Query_dataChanged;
                //Change of state!
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
                                if (Query.data.status.heater_bed.temperature > 30)
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
                            currentState = Query.data.status.print_stats.state;
                            Query.dataChanged += Query_dataChanged; //Reregister
                            break;
                        default:
                            Console.WriteLine("Unknown Klipper state!");
                            return;
                    }
                }
                else
                {
                }
            }
        }
        public override void OnLoaded()
        {
            currentState = Query.data.status.print_stats.state;
            Query.dataChanged += Query_dataChanged;
        }
        public override void RenderElements()
        {
            Panel mainPanel = new Panel();
            mainPanel.Visibility = Visibility.Visible;
            StackPanel stackPanel = new StackPanel();
            stackPanel.SetMargin(0, 30, 0, 0);
            Child = mainPanel;
            stackPanel.Visibility = Visibility.Visible;

            Text logoText = new Text(OpenHaloApplication.NinaBFont, "OpenHalo");
            logoText.VerticalAlignment = VerticalAlignment.Center;
            logoText.HorizontalAlignment = HorizontalAlignment.Center;
            logoText.ForeColor = System.Drawing.Color.White;
            logoText.Visibility = Visibility.Visible;
            logoText.SetMargin(0, 0, 0, 0);
            stackPanel.Children.Add(logoText);

            Text M117 = new Text(OpenHaloApplication.NinaBFont, "Error in the printer");
            M117.HorizontalAlignment = HorizontalAlignment.Center;
            M117.VerticalAlignment = VerticalAlignment.Center;
            M117.Visibility = Visibility.Visible;
            M117.SetMargin(25, 0, 25, 0);
            M117.ForeColor = System.Drawing.Color.Gray;
            stackPanel.Children.Add(M117);


            Image image = new Image(Resources.ResourceDictionary.GetBitmap(ResourceDictionary.BitmapResources.alert));
            image.HorizontalAlignment = HorizontalAlignment.Center;
            image.VerticalAlignment = VerticalAlignment.Center;
            image.Visibility = Visibility.Visible;

            stackPanel.Children.Add(image);

            Text printFinished = new Text(OpenHaloApplication.SegoeUI24, "Shutdown, error");
            printFinished.HorizontalAlignment = HorizontalAlignment.Center;
            printFinished.VerticalAlignment = VerticalAlignment.Center;
            printFinished.Visibility = Visibility.Visible;
            printFinished.SetMargin(25, 0, 25, 0);
            printFinished.ForeColor = System.Drawing.Color.LightGray;
            printFinished.TextAlignment = nanoFramework.Presentation.Media.TextAlignment.Center;

            stackPanel.Children.Add(printFinished);

            Text percentage = new Text(OpenHaloApplication.SegoeUI24, PrintingState.ToStringFormatDouble(Query.data.status.display_status.progress * 100) + " %");
            percentage.HorizontalAlignment = HorizontalAlignment.Center;
            percentage.VerticalAlignment = VerticalAlignment.Center;
            percentage.Visibility = Visibility.Visible;
            percentage.ForeColor = System.Drawing.Color.WhiteSmoke;
            percentage.TextAlignment = nanoFramework.Presentation.Media.TextAlignment.Center;
            stackPanel.Children.Add(percentage);

            int progressSize = 240 - (int)Math.Round(Query.data.status.display_status.progress * 10);
            int marginSize = (240 - progressSize) / 2;
            if (progressSize % 2 != 0)
                progressSize += 1;

            Ellipse ellipse = new Ellipse(240, 240);
            ellipse.Fill = new SolidColorBrush(System.Drawing.Color.Red);
            ellipse.Visibility = Visibility.Visible;
            ellipse.Stroke = new Pen(System.Drawing.Color.Red);

            Ellipse backgroundEllipse = new Ellipse(progressSize, progressSize);
            backgroundEllipse.Fill = new SolidColorBrush(System.Drawing.Color.Black);
            backgroundEllipse.Visibility = Visibility.Visible;
            backgroundEllipse.Stroke = new Pen(System.Drawing.Color.Black);

            backgroundEllipse.SetMargin(marginSize);

            mainPanel.Children.Add(ellipse);
            mainPanel.Children.Add(backgroundEllipse);
            mainPanel.Children.Add(stackPanel);
        }
    }
}
