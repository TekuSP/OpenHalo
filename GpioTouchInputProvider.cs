using System;
using System.Collections;
using System.Device.Gpio;
using nanoFramework.UI.Input;
using nanoFramework.UI.Threading;
using nanoFramework.Presentation;

namespace OpenHalo
{
    public sealed class GpioTouchInputProvider
    {
        public readonly Dispatcher Dispatcher;

        private ArrayList buttons;
        private DispatcherOperationCallback callback;
        private InputProviderSite site;
        private PresentationSource source;

        /// <summary>
        /// Maps GPIOs to Buttons that can be processed by 
        /// nanoFramework.Presentation.
        /// </summary>
        /// <param name="source"></param>
        public GpioTouchInputProvider(PresentationSource source)
        {
            // Set the input source.
            this.source = source;

            // Register our object as an input source with the input manager and 
            // get back an InputProviderSite object which forwards the input 
            // report to the input manager, which then places the input in the 
            // staging area.
            site = InputManager.CurrentInputManager.RegisterInputProvider(this);

            // Create a delegate that refers to the InputProviderSite object's 
            // ReportInput method.
            callback = new DispatcherOperationCallback(delegate (object report)
            {
                InputReportArgs args = (InputReportArgs)report;
                return site.ReportInput(args.Device, args.Report);
            });

            Dispatcher = Dispatcher.CurrentDispatcher;

            this.buttons = new ArrayList();
        }

        /// <summary>
        /// Add a GPIO pin as a specific Button
        /// </summary>
        /// <param name="gpioPinNumber">GPIO pin number</param>
        /// <param name="button">Button that this pin represents</param>
        /// <param name="internalPullup">If true will enable the internal pull up on pin ( SetDriveMode = InputPullUp )
        public void AddTouch(Button button, TekuSP.Drivers.CST816D.CST816D cts, TekuSP.Drivers.CST816D.Gesture gestureToReact)
        {
            this.buttons.Add(new TouchPad(this, button, cts, gestureToReact));
        }

        /// <summary>
        /// Represents a button pad on the emulated device, containing five 
        /// buttons for user input. 
        /// </summary>
        internal class TouchPad : IDisposable
        {
            private readonly Button button;
            private readonly GpioTouchInputProvider sink;
            private readonly ButtonDevice buttonDevice = InputManager.CurrentInputManager.ButtonDevice;
            TekuSP.Drivers.CST816D.Gesture gest;
            /// <summary>
            /// Constructs a ButtonPad object that handles the  
            /// hardware's button interrupts.
            /// </summary>
            /// <param name="sink"></param>
            /// <param name="button"></param>
            /// <param name="pin"></param>
            public TouchPad(GpioTouchInputProvider sink, Button button, TekuSP.Drivers.CST816D.CST816D cts, TekuSP.Drivers.CST816D.Gesture gestureToReact = TekuSP.Drivers.CST816D.Gesture.GEST_NONE)
            {
                this.sink = sink;
                this.button = button;
                gest = gestureToReact;
                cts.OnStateChanged += Cts_OnStateChanged;
            }

            private void Cts_OnStateChanged(object sender, TekuSP.Drivers.DriverBase.Interfaces.ITouchData data)
            {
                DateTime time = DateTime.UtcNow;

                if (gest == TekuSP.Drivers.CST816D.Gesture.GEST_NONE)
                {
                    sink.Dispatcher.BeginInvoke(sink.callback, new InputReportArgs(buttonDevice, new RawTouchInputReport(sink.source, time, data.Gesture, new nanoFramework.UI.TouchInput[] { new nanoFramework.UI.TouchInput() { X = data.X, Y = data.Y, ContactHeight = 15, ContactWidth = 15 } })));
                    return;
                }
                if (data.Gesture != (byte)gest)
                    return;

                RawButtonInputReport report = new RawButtonInputReport(sink.source, time, button, RawButtonActions.ButtonDown);

                RawButtonInputReport report2 = new RawButtonInputReport(sink.source, time, button, RawButtonActions.ButtonUp);

                // Queue the button press to the input provider site.
                sink.Dispatcher.BeginInvoke(sink.callback, new InputReportArgs(buttonDevice, report));
                sink.Dispatcher.BeginInvoke(sink.callback, new InputReportArgs(buttonDevice, report2));
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

        }
    }
}
