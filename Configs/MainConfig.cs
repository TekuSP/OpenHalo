using System;
using System.Collections;
using System.Text;

namespace OpenHalo.Configs
{
    [Serializable]
    public class MainConfig
    {
        public string MoonrakerApiKey
        {
            get; set;
        }
        public string MoonrakerUri
        {
            get; set;
        }
        public Wifi[] Wifis
        {
            get; set;
        }
    }
    [Serializable]
    public class Wifi
    {
        public string SSID
        {
            get; set;
        }
        public string Password
        {
            get; set;
        }
        public bool Hidden
        {
            get; set;
        }
    }
}
