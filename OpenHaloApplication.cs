using nanoFramework.Presentation;
using nanoFramework.UI;
using nanoFramework.UI.Input;
using System;
using System.Threading;
using System.Device.Gpio;
using System.Diagnostics;
using nanoFramework.Hardware.Esp32;

namespace OpenHalo
{
    public class OpenHaloApplication : Application
    {
        static Window mainWindow;

        public static void Main()
        {
            int backLightPin = 8;
            int chipSelect = 10;
            int dataCommand = 9;
            int reset = 13;

            Console.WriteLine("OpenHalo starting!");
            Diag.PrintMemory("OpenHalo");
            Console.WriteLine("Starting proceeding!");
            Console.WriteLine("Initializing screen SPI...");
            Console.WriteLine("1 MISO");
            Console.WriteLine("11 MOSI");
            Console.WriteLine("12 CLOCK");
            Configuration.SetPinFunction(1, DeviceFunction.SPI1_MISO);
            Configuration.SetPinFunction(11, DeviceFunction.SPI1_MOSI);
            Configuration.SetPinFunction(12, DeviceFunction.SPI1_CLOCK);
            Console.WriteLine("SPI Initialized!");
            Console.WriteLine("Initializing screen GC9A01...");
            DisplayControl.Initialize(new SpiConfiguration(1, chipSelect, dataCommand, reset, -1), new ScreenConfiguration(0, 0, 240, 240), 200000);
            Console.WriteLine("Initialized screen 240x240 with 200000 bytes of memory!");
            Console.WriteLine($"Screen is using CS: {chipSelect}, DC: {dataCommand} and RST {reset}");
            Console.WriteLine("Initializing backlight...");
            new GpioController().OpenPin(backLightPin, PinMode.Output);
            new GpioController().Write(backLightPin, PinValue.High);
            Console.WriteLine($"Backlight initialized on pin: {backLightPin} !");
            Console.WriteLine("Initializing WPF framework...");
            OpenHaloApplication myApplication = new OpenHaloApplication();
            Console.WriteLine("Framework initialized!");
            Console.WriteLine("Loading config...");


            Console.WriteLine("Loading first time setup window...");


            Console.WriteLine("Loading connecting window...");


            Console.WriteLine("Rendering and launching app!");
            myApplication.Run(mainWindow);
        }
    }
    public static class Diag
    {
        public static void PrintMemory(string msg)
        {
            NativeMemory.GetMemoryInfo(NativeMemory.MemoryType.Internal, out uint totalSize, out uint totalFreeSize, out uint largestBlock);
            Console.WriteLine($"\n{msg}-> Internal Total Mem {totalSize} Total Free {totalFreeSize} Largest Block {largestBlock}");
            //Debug.WriteLine($"nF Mem {Memory.Run(false)}\n ");
            NativeMemory.GetMemoryInfo(NativeMemory.MemoryType.SpiRam, out uint totalSize1, out uint totalFreeSize1, out uint largestBlock1);
            Console.WriteLine($"\n{msg}-> SpiRam Total Mem {totalSize1} Total Free {totalFreeSize1} Largest Block {largestBlock1}");
        }
    }
}
