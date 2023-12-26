using nanoFramework.Presentation;
using nanoFramework.UI;
using nanoFramework.UI.Input;
using System;
using System.Threading;
using System.Device.Gpio;
using System.Diagnostics;
using nanoFramework.Hardware.Esp32;
using System.IO;
using Windows.Storage;
using nanoFramework.Json;
using OpenHalo.Configs;
using OpenHalo.Windows;
using OpenHalo.Resources;
using nanoFramework.Networking;

namespace OpenHalo
{
    public class OpenHaloApplication : Application
    {
        static Window mainWindow;
        public static MainConfig config;
        public static Font NinaBFont;
        public static Font SmallFont;
        public static void Main()
        {
            int backLightPin = 8;
            int chipSelect = 10;
            int dataCommand = 9;
            int reset = 13;
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
            Console.WriteLine("Initializing screen GC9A01...");
            DisplayControl.Initialize(new SpiConfiguration(1, chipSelect, dataCommand, reset, -1), new ScreenConfiguration(0, 0, 240, 240), 200000);
            Console.WriteLine("Initialized screen 240x240 with 200000 bytes of memory!");
            Console.WriteLine($"Screen is using CS: {chipSelect}, DC: {dataCommand} and RST {reset}");
            Console.WriteLine("Initializing fonts....");
            Console.WriteLine("Loading NinaB...");
            NinaBFont = ResourceDictionary.GetFont(ResourceDictionary.FontResources.NinaB);
            Console.WriteLine("Loading small....");
            SmallFont = ResourceDictionary.GetFont(ResourceDictionary.FontResources.small);
            Console.WriteLine("Initializing backlight...");
            new GpioController().OpenPin(backLightPin, PinMode.Output);
            new GpioController().Write(backLightPin, PinValue.High);
            Console.WriteLine($"Backlight initialized on pin: {backLightPin} !");
            Console.WriteLine("Initializing WPF framework...");
            OpenHaloApplication myApplication = new OpenHaloApplication();
            Console.WriteLine("Framework initialized!");
            Console.WriteLine("Loading config...");
            Console.WriteLine("Searching for configuration file...");
            if (File.Exists("I:\\configuration.json"))
            {
                Console.WriteLine("Found configuration file, reading...");
                var file = StorageFile.GetFileFromPath("I:\\configuration.json");
                var configString = FileIO.ReadText(file);
                try
                {
                    config = (MainConfig)JsonConvert.DeserializeObject(configString, typeof(MainConfig));
                    Console.WriteLine("Configuration file read succesfully.");
                }
                catch
                {
                    Console.WriteLine("Configuration file damaged. Recreating file.");
                    file.Delete();
                }
            }
            if (!File.Exists("I:\\configuration.json"))
            {
                Console.WriteLine("No configuration loaded.");
                Console.WriteLine("Loading first time setup window...");
            }
            else
            {
                Console.WriteLine("Configuration loaded.");
                Console.WriteLine("Loading connecting window...");
                mainWindow = new EnterSetup(myApplication);
            }

            //DEBUG! TESTING ONLY!
            mainWindow = new EnterSetup(myApplication);
            //REMOVE WHEN DONE

            Console.WriteLine("Rendering and launching app!");
            myApplication.Run(mainWindow);
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
