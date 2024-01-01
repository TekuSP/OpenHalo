using System;
using System.Globalization;
using System.Text;
using System.Threading;

using nanoFramework.Presentation;
using nanoFramework.Presentation.Controls;
using nanoFramework.Presentation.Media;
using nanoFramework.Presentation.Shapes;

using OpenHalo.Moonraker;
using OpenHalo.Resources;

namespace OpenHalo.Windows.PrintingStates
{
    public class PrintingState : HaloWindow
    {
        private Text hotend;
        private Text heatbed;
        private Text hotendTarget;
        private Text heatbedTarget;
        private Text fileName;
        private Text percentage;
        private Text M117;
        private Ellipse backgroundEllipse;
        public PrintingState(OpenHaloApplication app) : base(app)
        {
        }

        public override void OnLoaded()
        {
            Query.dataChanged += Query_dataChanged;
        }

        private void Query_dataChanged(object sender, EventArgs e)
        {
            if (hotend == null || heatbed == null || hotendTarget == null || heatbedTarget == null || fileName == null || percentage == null || M117 == null || backgroundEllipse == null)
            {
                return;
            }
            Dispatcher.Invoke(TimeSpan.MaxValue, (args) =>
            {
                hotend.TextContent = ToStringFormatDouble(Query.data.status.extruder.temperature);
                heatbed.TextContent = ToStringFormatDouble(Query.data.status.heater_bed.temperature);
                hotendTarget.TextContent = ToStringFormatDouble(Query.data.status.extruder.target);
                heatbedTarget.TextContent = ToStringFormatDouble(Query.data.status.heater_bed.target);
                fileName.TextContent = Query.data.status.print_stats.filename;
                percentage.TextContent = ToStringFormatDouble(Query.data.status.display_status.progress * 100) + " %";
                M117.TextContent = string.IsNullOrEmpty(Query.data.status.display_status.message) ? "Printing" : "M117: " + Query.data.status.display_status.message;
                int progressSize = 240 - (int)Math.Round(Query.data.status.display_status.progress * 10);
                int marginSize = (240 - progressSize) / 2;
                backgroundEllipse.Width = progressSize;
                backgroundEllipse.Height = progressSize;
                backgroundEllipse.SetMargin(marginSize);
                return null;
            }, null);
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

            M117 = new Text(OpenHaloApplication.NinaBFont, "M117: " + Query.data.status.display_status.message);
            M117.HorizontalAlignment = HorizontalAlignment.Center;
            M117.VerticalAlignment = VerticalAlignment.Center;
            M117.Visibility = Visibility.Visible;
            M117.SetMargin(25, 0, 25, 0);
            M117.ForeColor = System.Drawing.Color.Gray;
            M117.TextWrap = true;
            M117.TextAlignment = nanoFramework.Presentation.Media.TextAlignment.Center;
            if (string.IsNullOrEmpty(Query.data.status.display_status.message))
                M117.TextContent = "Printing";
            stackPanel.Children.Add(M117);

            StackPanel temps = new StackPanel(Orientation.Horizontal);
            temps.Visibility = Visibility.Visible;

            StackPanel hotendPanel = new StackPanel(Orientation.Vertical);
            hotendPanel.Visibility = Visibility.Visible;
            hotendPanel.SetMargin(10, 0, 0, 0);
            hotendPanel.Width = 110;

            Image hotendImage = new Image(Resources.ResourceDictionary.GetBitmap(ResourceDictionary.BitmapResources.nozzle));
            hotendImage.HorizontalAlignment = HorizontalAlignment.Center;
            hotendImage.VerticalAlignment = VerticalAlignment.Center;
            hotendImage.Visibility = Visibility.Visible;

            hotendPanel.Children.Add(hotendImage);

            hotend = new Text(OpenHaloApplication.SegoeUI24, ToStringFormatDouble(Query.data.status.extruder.temperature));
            heatbed = new Text(OpenHaloApplication.SegoeUI24, ToStringFormatDouble(Query.data.status.heater_bed.temperature));
            hotend.VerticalAlignment = VerticalAlignment.Center;
            hotend.HorizontalAlignment = HorizontalAlignment.Center;
            hotend.ForeColor = System.Drawing.Color.White;

            hotendTarget = new Text(OpenHaloApplication.SegoeUI14, ToStringFormatDouble(Query.data.status.extruder.target));
            heatbedTarget = new Text(OpenHaloApplication.SegoeUI14, ToStringFormatDouble(Query.data.status.heater_bed.target));
            hotendTarget.VerticalAlignment = VerticalAlignment.Center;
            hotendTarget.HorizontalAlignment = HorizontalAlignment.Center;
            hotendTarget.ForeColor = System.Drawing.Color.Red;

            heatbedTarget.VerticalAlignment = VerticalAlignment.Center;
            heatbedTarget.HorizontalAlignment = HorizontalAlignment.Center;
            heatbedTarget.ForeColor = System.Drawing.Color.Red;

            hotendPanel.Children.Add(hotend);
            hotendPanel.Children.Add(hotendTarget);

            heatbed.VerticalAlignment = VerticalAlignment.Center;
            heatbed.HorizontalAlignment = HorizontalAlignment.Center;
            heatbed.ForeColor = System.Drawing.Color.White;

            StackPanel heatbedPanel = new StackPanel(Orientation.Vertical);
            heatbedPanel.Visibility = Visibility.Visible;
            heatbedPanel.Width = 110;
            heatbedPanel.SetMargin(0, 0, 10, 0);            

            Image heatbedImage = new Image(Resources.ResourceDictionary.GetBitmap(ResourceDictionary.BitmapResources.heatbed));
            heatbedImage.HorizontalAlignment = HorizontalAlignment.Center;
            heatbedImage.VerticalAlignment = VerticalAlignment.Center;
            heatbedImage.Visibility = Visibility.Visible;

            heatbedPanel.Children.Add(heatbedImage);
            heatbedPanel.Children.Add(heatbed);
            heatbedPanel.Children.Add(heatbedTarget);

            temps.Children.Add(hotendPanel);
            temps.Children.Add(heatbedPanel);

            stackPanel.Children.Add(temps);

            fileName = new Text(OpenHaloApplication.SmallFont, Query.data.status.print_stats.filename);
            fileName.HorizontalAlignment = HorizontalAlignment.Center;
            fileName.VerticalAlignment = VerticalAlignment.Center;
            fileName.Visibility = Visibility.Visible;
            fileName.SetMargin(25, 0, 25, 0);
            fileName.ForeColor = System.Drawing.Color.LightGray;
            fileName.TextWrap = true;
            fileName.TextAlignment = nanoFramework.Presentation.Media.TextAlignment.Center;
            stackPanel.Children.Add(fileName);

            percentage = new Text(OpenHaloApplication.SegoeUI24, ToStringFormatDouble(Query.data.status.display_status.progress * 100) + " %");
            percentage.HorizontalAlignment = HorizontalAlignment.Center;
            percentage.VerticalAlignment = VerticalAlignment.Center;
            percentage.Visibility = Visibility.Visible;
            percentage.ForeColor = System.Drawing.Color.WhiteSmoke;
            percentage.TextAlignment = nanoFramework.Presentation.Media.TextAlignment.Center;
            stackPanel.Children.Add(percentage);

            //ProgressBar

            int progressSize = 240 - (int)Math.Round(Query.data.status.display_status.progress * 10);
            int marginSize = (240 - progressSize) / 2;

            Ellipse ellipse = new Ellipse(240, 240);
            ellipse.Fill = new SolidColorBrush(System.Drawing.Color.Green);
            ellipse.Visibility = Visibility.Visible;
            ellipse.Stroke = new Pen(System.Drawing.Color.Green);

            backgroundEllipse = new Ellipse(progressSize, progressSize);
            backgroundEllipse.Fill = new SolidColorBrush(System.Drawing.Color.Black);
            backgroundEllipse.Visibility = Visibility.Visible;
            backgroundEllipse.Stroke = new Pen(System.Drawing.Color.Black);

            backgroundEllipse.SetMargin(marginSize);

            mainPanel.Children.Add(ellipse);
            mainPanel.Children.Add(backgroundEllipse);
            mainPanel.Children.Add(stackPanel);
        }

        public string ToStringFormatDouble(double value)
        {
            var result = "";
            var str = value.ToString();
            var split = str.Split('.');
            if (split[0].Length == 1)
            {
                result += "00" + split[0];
            }
            else if (split[0].Length == 2)
            {
                result += "0" + split[0];
            }
            else
            {
                result = split[0];
            }
            result += ".";

            if (split.Length == 1)
            {
                result += "00";
            }
            else
            {
                if (split[1].Length == 1)
                {
                    result += split[1] + "0";
                }
                else if (split[1].Length == 2)
                {
                    result += split[1];
                }
                else
                {
                    result += split[1][0].ToString() + split[1][1].ToString();
                }
            }
            return result;
        }
    }
}
