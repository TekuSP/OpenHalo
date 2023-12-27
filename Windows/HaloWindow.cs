using System.Threading;

using nanoFramework.Presentation;
using nanoFramework.UI.Input;
using nanoFramework.UI;

namespace OpenHalo.Windows
{
    /// <summary>
    /// Our implementation of NFP Window
    /// </summary>
    public abstract class HaloWindow : Window
    {
        /// <summary>
        /// Our App main class
        /// </summary>
        protected OpenHaloApplication App
        {
            get; set;
        }
        /// <summary>
        /// Constructor to construct a Window
        /// </summary>
        /// <param name="app">Requires App main class</param>
        public HaloWindow(OpenHaloApplication app)
        {
            App = app;

            Visibility = Visibility.Visible;
            Width = DisplayControl.ScreenWidth;
            Height = DisplayControl.ScreenHeight;
            Buttons.Focus(this);

            Background = new nanoFramework.Presentation.Media.SolidColorBrush(System.Drawing.Color.Black);
            Foreground = new nanoFramework.Presentation.Media.SolidColorBrush(System.Drawing.Color.White);

            RenderElements();

            Thread th = new Thread(OnLoaded);
            th.Start();
        }
        /// <summary>
        /// Here runs active code, remember so you are in different thread!
        /// </summary>
        public abstract void OnLoaded();
        /// <summary>
        /// Insert all elements to be initialized here
        /// </summary>
        public abstract void RenderElements();
    }
}
