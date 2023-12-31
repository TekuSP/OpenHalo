﻿using System;
using System.Globalization;
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
        private Text hotendTarget;
        private Text heatbedTarget;
        private Text fileName;
        public PrintingState(OpenHaloApplication app) : base(app)
        {
        }

        public override void OnLoaded()
        {
            Query.dataChanged += Query_dataChanged;
        }

        private void Query_dataChanged(object sender, EventArgs e)
        {
            if (hotend == null || heatbed == null || hotendTarget == null || heatbedTarget == null || fileName == null)
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
                return null;
            }, null);
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

            StackPanel hotendPanel = new StackPanel(Orientation.Vertical);
            hotendPanel.Visibility = Visibility.Visible;
            hotendPanel.SetMargin(40, 10, 0, 0);

            Image hotendImage = new Image(Resources.ResourceDictionary.GetBitmap(ResourceDictionary.BitmapResources.nozzle));
            hotendImage.HorizontalAlignment = HorizontalAlignment.Center;
            hotendImage.VerticalAlignment = VerticalAlignment.Center;
            hotendImage.Visibility = Visibility.Visible;

            hotendPanel.Children.Add(hotendImage);

            hotend = new Text(OpenHaloApplication.NinaBFont, ToStringFormatDouble(Query.data.status.extruder.temperature));
            heatbed = new Text(OpenHaloApplication.NinaBFont, ToStringFormatDouble(Query.data.status.heater_bed.temperature));
            hotend.VerticalAlignment = VerticalAlignment.Center;
            hotend.HorizontalAlignment = HorizontalAlignment.Left;
            hotend.ForeColor = System.Drawing.Color.White;
            hotend.SetMargin(7, 0, 0, 0);

            hotendTarget = new Text(OpenHaloApplication.NinaBFont, ToStringFormatDouble(Query.data.status.extruder.target));
            heatbedTarget = new Text(OpenHaloApplication.NinaBFont, ToStringFormatDouble(Query.data.status.heater_bed.target));
            hotendTarget.VerticalAlignment = VerticalAlignment.Center;
            hotendTarget.HorizontalAlignment = HorizontalAlignment.Left;
            hotendTarget.ForeColor = System.Drawing.Color.Red;
            hotendTarget.SetMargin(7, 0, 0, 0);

            heatbedTarget.VerticalAlignment = VerticalAlignment.Center;
            heatbedTarget.HorizontalAlignment = HorizontalAlignment.Left;
            heatbedTarget.ForeColor = System.Drawing.Color.Red;
            heatbedTarget.SetMargin(7, 0, 0, 0);

            hotendPanel.Children.Add(hotend);
            hotendPanel.Children.Add(hotendTarget);

            heatbed.VerticalAlignment = VerticalAlignment.Center;
            heatbed.HorizontalAlignment = HorizontalAlignment.Left;
            heatbed.ForeColor = System.Drawing.Color.White;
            heatbed.SetMargin(7, 0, 0, 0);

            StackPanel heatbedPanel = new StackPanel(Orientation.Vertical);
            heatbedPanel.Visibility = Visibility.Visible;
            heatbedPanel.SetMargin(45, 10, 0, 0);

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
            fileName.SetMargin(25,10,25,10);
            fileName.ForeColor = System.Drawing.Color.LightGray;
            fileName.TextWrap = true;
            fileName.TextAlignment = nanoFramework.Presentation.Media.TextAlignment.Center;
            stackPanel.Children.Add(fileName);
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
