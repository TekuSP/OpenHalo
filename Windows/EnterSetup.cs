using System;
using System.Text;

using nanoFramework.Presentation;
using nanoFramework.UI.Input;
using nanoFramework.UI;
using nanoFramework.Presentation.Controls;
using OpenHalo.Resources;

namespace OpenHalo.Windows
{
    public class EnterSetup : Window
    {
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
            StackPanel stackPanel = new StackPanel();
            Child = stackPanel;
            stackPanel.Visibility = Visibility.Visible;
            Image logo = new Image(IconResources.GetBitmap(IconResources.BitmapResources.settings));
            logo.HorizontalAlignment = HorizontalAlignment.Center;
            logo.Visibility = Visibility.Visible;
            stackPanel.Children.Add(logo);

        }
        protected override void OnButtonDown(ButtonEventArgs e)
        {
            Close(); //Close Enter Setup window
            new Setup(App); //Open Setup
        }
    }
}
