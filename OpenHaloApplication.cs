using nanoFramework.Presentation;
using nanoFramework.UI;
using nanoFramework.UI.Input;
using System;
using System.Threading;
using System.Device.Gpio;
using System.Diagnostics;
using nanoFramework.Hardware.Esp32;
using System.IO;
using nanoFramework.Json;
using OpenHalo.Configs;
using OpenHalo.Windows;
using OpenHalo.Resources;
using nanoFramework.Networking;
using System.Device.Pwm;
using OpenHalo.Helpers;
using System.Device.Wifi;

namespace OpenHalo
{
    public class OpenHaloApplication : Application
    {
        public static MainConfig config;
        public static Font NinaBFont;
        public static Font SmallFont;
        public static Font Courrier10;
        public static Font SegoeUI12;
        public static Font SegoeUI14;
        public static Font SegoeUI16;
        public static Font SegoeUI18;
        public static Font SegoeUI24;
        public static readonly string ConfigLocation = "I:\\configuration.json";
        public static PwmChannel BackLightPWM
        {
            get; set;
        }
        public static void Main()
        {
#if DEBUG
            Thread.Sleep(15000);
#endif
            int backLightPin = 8;
            int chipSelect = 10;
            int dataCommand = 9;
            int reset = 13;
            int i2c_clock = 40;
            int i2c_data = 39;
            Console.WriteLine("OpenHalo starting!");
            Console.WriteLine("Memory test...");
            Diagnostics.PrintMemory("OpenHalo");
            Console.WriteLine("Starting proceeding!");
            Console.WriteLine("Initializing screen SPI...");
            Console.WriteLine("1 MISO");
            Configuration.SetPinFunction(1, DeviceFunction.SPI1_MISO);
            Console.WriteLine("11 MOSI");
            Configuration.SetPinFunction(11, DeviceFunction.SPI1_MOSI);
            Console.WriteLine("12 CLOCK");
            Configuration.SetPinFunction(12, DeviceFunction.SPI1_CLOCK);
            Console.WriteLine("SPI Initialized!");
            Console.WriteLine("Initializing touchscreen I2C...");
            Console.WriteLine("40 CLOCK");
            Configuration.SetPinFunction(i2c_clock, DeviceFunction.I2C1_CLOCK);
            Console.WriteLine("39 DATA");
            Configuration.SetPinFunction(i2c_data, DeviceFunction.I2C1_DATA);
            Console.WriteLine("I2C Initialized!");
            Console.WriteLine("Initializing display PWM Backlight....");
            nanoFramework.Hardware.Esp32.Configuration.SetPinFunction(backLightPin, DeviceFunction.PWM3);
            Console.WriteLine("PWM Initialized!");
            Console.WriteLine("Initializing screen GC9A01...");
            DisplayControl.Initialize(new SpiConfiguration(1, chipSelect, dataCommand, reset, -1), new ScreenConfiguration(0, 0, 240, 240), 200000);
            Console.WriteLine("Initialized screen 240x240 with 200000 bytes of memory!");
            Console.WriteLine($"Screen is using CS: {chipSelect}, DC: {dataCommand} and RST {reset}");
            Console.WriteLine("Initializing fonts....");
            Console.WriteLine("Loading NinaB...");
            NinaBFont = ResourceDictionary.GetFont(ResourceDictionary.FontResources.NinaB);
            Console.WriteLine("Loading small....");
            SmallFont = ResourceDictionary.GetFont(ResourceDictionary.FontResources.small);
            Console.WriteLine("Loading Courier Regular 10");
            Courrier10 = ResourceDictionary.GetFont(ResourceDictionary.FontResources.courierregular10);
            Console.WriteLine("Loading Segoe UI 12");
            SegoeUI12 = ResourceDictionary.GetFont(ResourceDictionary.FontResources.SegoeUI12);
            Console.WriteLine("Loading Segoe UI 14");
            SegoeUI14 = ResourceDictionary.GetFont(ResourceDictionary.FontResources.SegoeUI14);
            Console.WriteLine("Loading Segoe UI 16");
            SegoeUI16 = ResourceDictionary.GetFont(ResourceDictionary.FontResources.SegoeUI16);
            Console.WriteLine("Loading Segoe UI 18");
            SegoeUI18 = ResourceDictionary.GetFont(ResourceDictionary.FontResources.SegoeUI18);
            Console.WriteLine("Loading Segoe UI 24");
            SegoeUI24 = ResourceDictionary.GetFont(ResourceDictionary.FontResources.SegoeUI24);
            Console.WriteLine("Fonts initialized!");
            Console.WriteLine("Initializing backlight...");
            BackLightPWM = PwmChannel.CreateFromPin(backLightPin, 400);
            BackLightPWM.Start();
            SetBrightness(0.5);
            Console.WriteLine($"Backlight initialized on pin: {backLightPin} !");
            Console.WriteLine("Initializing CST816D TouchScreen...");
            TekuSP.Drivers.CST816D.CST816D cts = new TekuSP.Drivers.CST816D.CST816D(1, 41, 42);
            TouchInputProvider touchInput = new TouchInputProvider(null);
            touchInput.AddTouch(Button.VK_LEFT, cts, TekuSP.Drivers.CST816D.Gesture.GEST_MOVE_LEFT);
            touchInput.AddTouch(Button.VK_RIGHT, cts, TekuSP.Drivers.CST816D.Gesture.GEST_MOVE_RIGHT);
            touchInput.AddTouch(Button.VK_UP, cts, TekuSP.Drivers.CST816D.Gesture.GEST_MOVE_UP);
            touchInput.AddTouch(Button.VK_SELECT, cts, TekuSP.Drivers.CST816D.Gesture.GEST_SINGLE_CLICK);
            touchInput.AddTouch(Button.VK_BACK, cts, TekuSP.Drivers.CST816D.Gesture.GEST_LONG_PRESS);
            touchInput.AddTouch(Button.VK_DOWN, cts, TekuSP.Drivers.CST816D.Gesture.GEST_MOVE_DOWN);
            cts.Start();
            Console.WriteLine("CST816D initialized!");
            Console.WriteLine("Initializing WPF framework...");
            OpenHaloApplication myApplication = new OpenHaloApplication();
            Console.WriteLine("Framework initialized!");
            Console.WriteLine("Loading config...");
            config = ConfigHelper.LoadConfig();
            Window window;
            if (config == null || Networking.IsAPModeEnabled())
            {
                Console.WriteLine("No configuration loaded or Setup requested.");
                Console.WriteLine("Loading Setup window...");
                window = new Setup(myApplication);
            }
            else
            {
                Console.WriteLine("Configuration loaded.");
                Console.WriteLine("Loading connecting window...");
                window = new EnterSetup(myApplication);
            }


            //WifiAdapter wifi = WifiAdapter.FindAllAdapters()[0];
            //wifi.AvailableNetworksChanged += WifiAdapter_AvailableNetworksChanged;
            //Thread th = new Thread(() =>
            //{
            //    while (true)
            //    {
            //        try
            //        {
            //            Console.WriteLine("Trying scan....");
            //            wifi.ScanAsync();
            //        }
            //        catch (Exception)
            //        {
            //            Console.WriteLine("Exception scanning...");
            //        }
            //        finally { Thread.Sleep(10000); }
            //    }

            //});
            //th.Start();

            Console.WriteLine("Rendering and launching app!");
            myApplication.Run(window);
        }

        //private static void WifiAdapter_AvailableNetworksChanged(WifiAdapter sender, object e)
        //{
        //    Console.WriteLine(JsonConvert.SerializeObject(sender.NetworkReport.AvailableNetworks));
        //}

        public static void SetBrightness(double brightness)
        {
            BackLightPWM.DutyCycle = brightness;
        }
    }
    public static class Diagnostics
    {
        public static void PrintMemory(string msg)
        {
            NativeMemory.GetMemoryInfo(NativeMemory.MemoryType.Internal, out uint totalSize, out uint totalFreeSize, out uint largestBlock);
            Console.WriteLine($"\n{msg}-> Internal Total Mem {totalSize} Total Free {totalFreeSize} Largest Block {largestBlock}");
            Debug.WriteLine($"nF Mem {nanoFramework.Runtime.Native.GC.Run(false)}\n ");
            NativeMemory.GetMemoryInfo(NativeMemory.MemoryType.SpiRam, out uint totalSize1, out uint totalFreeSize1, out uint largestBlock1);
            Console.WriteLine($"\n{msg}-> SpiRam Total Mem {totalSize1} Total Free {totalFreeSize1} Largest Block {largestBlock1}");
        }
        public static string GetNetworkStatus(NetworkHelperStatus networkHelperStatus)
        {
            switch (networkHelperStatus)
            {
                case NetworkHelperStatus.None:
                    return "None";
                case NetworkHelperStatus.Started:
                    return "Started";
                case NetworkHelperStatus.NetworkIsReady:
                    return "Network is ready";
                case NetworkHelperStatus.FailedNoNetworkInterface:
                    return "Failed, no network interface";
                case NetworkHelperStatus.TokenExpiredWaitingIPAddress:
                    return "Token expired waiting for IP Address";
                case NetworkHelperStatus.TokenExpiredWaitingDateTime:
                    return "Token expired waiting for NTP";
                case NetworkHelperStatus.ErrorConnetingToWiFiWhileScanning:
                    return "Error connecting to WiFi while scanning";
                case NetworkHelperStatus.ExceptionOccurred:
                    return "Exception occured";
                default:
                    return "Unknown";
            }
        }
    }
}
