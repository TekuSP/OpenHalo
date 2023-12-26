using System;
using System.Text;

using nanoFramework.Presentation;
using nanoFramework.UI.Input;
using nanoFramework.UI;

namespace OpenHalo.Windows
{
    public class MainPrinterPage : Window
    {
        private OpenHaloApplication App
        {
            get; set;
        }
        public MainPrinterPage(OpenHaloApplication application)
        {
            App = application;
            Visibility = Visibility.Visible;
            Width = DisplayControl.ScreenWidth;
            Height = DisplayControl.ScreenHeight;
            Buttons.Focus(this);
        }
    }
}
