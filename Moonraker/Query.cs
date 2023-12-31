namespace OpenHalo.Moonraker
{
    using System;
    using System.Threading;

    using nanoFramework.Json;
    using OpenHalo.Helpers;

    public partial class Query
    {
        private static bool isRunning = false;
        public static Result data = null;
        public static event EventHandler dataChanged = delegate { };
        public Result result
        {
            get; set;
        }
        public static void StartQuery()
        {
            if (isRunning) { return; }
            isRunning = true;
            Thread thread = new Thread(Run);
            thread.Start();
        }
        public static bool IsQueryRunning() => isRunning;
        public static void StopQuery()
        {
            isRunning = false;
        }

        private static void Run()
        {
            var client = HttpHelpers.HttpClient();
            while (isRunning)
            {
                try
                {
                    var str = client.GetString($"http://{OpenHaloApplication.config.MoonrakerUri}/printer/objects/query?extruder&heater_bed&print_stats&system_stats&display_status&webhooks&virtual_sdcard");
                    data = ((Query)JsonConvert.DeserializeObject(str, typeof(Query))).result;
                    dataChanged?.Invoke(null, new EventArgs());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception happened at getting Moonraker Query.");
                    Console.WriteLine(ex.ToString());
                    data = null;
                }
                finally
                {
                    Thread.Sleep(500);
                }
            }
        }
    }

    public partial class Result
    {
        public double eventtime
        {
            get; set;
        }

        public Status status
        {
            get; set;
        }
    }

    public partial class Status
    {
        public string[] objects
        {
            get; set;
        }
        public Webhooks webhooks
        {
            get; set;
        }

        public VirtualSdcard virtual_sdcard
        {
            get; set;
        }

        public PrintStats print_stats
        {
            get; set;
        }
        public Extruder extruder
        {
            get; set;
        }
        public HeaterBed heater_bed
        {
            get; set;
        }
        public SystemStats system_stats
        {
            get; set;
        }
        public DisplayStatus display_status
        {
            get; set;
        }

    }

    public partial class PrintStats
    {
        public string filename
        {
            get; set;
        }

        public double total_duration
        {
            get; set;
        }

        public double print_duration
        {
            get; set;
        }

        public double filament_used
        {
            get; set;
        }

        public string state
        {
            get; set;
        }

        public string message
        {
            get; set;
        }

        public Info info
        {
            get; set;
        }
    }

    public partial class Info
    {
        public object total_layer
        {
            get; set;
        }

        public object current_layer
        {
            get; set;
        }
    }

    public partial class VirtualSdcard
    {
        public string file_path
        {
            get; set;
        }

        public double progress
        {
            get; set;
        }

        public bool is_active
        {
            get; set;
        }

        public long file_position
        {
            get; set;
        }

        public long file_size
        {
            get; set;
        }
    }

    public partial class Webhooks
    {
        public string state
        {
            get; set;
        }

        public string state_message
        {
            get; set;
        }
    }

    public partial class Extruder
    {
        public double temperature
        {
            get; set;
        }

        public double target
        {
            get; set;
        }

        public double power
        {
            get; set;
        }

        public bool can_extrude
        {
            get; set;
        }

        public double pressure_advance
        {
            get; set;
        }

        public double smooth_time
        {
            get; set;
        }

        public object motion_queue
        {
            get; set;
        }
    }

    public partial class HeaterBed
    {
        public double temperature
        {
            get; set;
        }

        public double target
        {
            get; set;
        }

        public double power
        {
            get; set;
        }
    }

    public partial class SystemStats
    {
        public double sysload
        {
            get; set;
        }

        public double cputime
        {
            get; set;
        }

        public long memavail
        {
            get; set;
        }
    }

    public partial class DisplayStatus
    {
        public double progress
        {
            get; set;
        }

        public object message
        {
            get; set;
        }
    }
}
