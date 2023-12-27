using System;
using System.Net.NetworkInformation;

using nanoFramework.Presentation;

using OpenHalo.Windows;

namespace OpenHalo.Helpers
{
    /// <summary>
    /// Networking helper for WIFI
    /// </summary>
    public static class Networking
    {
        /// <summary>
        /// Gets WIFI Interface
        /// </summary>
        /// <returns>Returns WIFI Interface</returns>
        public static NetworkInterface GetInterface()
        {
            NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();

            // Find WirelessAP interface
            foreach (NetworkInterface ni in Interfaces)
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.WirelessAP)
                {
                    return ni;
                }
            }
            return null;
        }
        /// <summary>
        /// Gets configuration of client mode
        /// </summary>
        /// <returns>Configuration of client mode</returns>
        public static WirelessAPConfiguration GetConfiguration()
        {
            NetworkInterface ni = GetInterface();
            return WirelessAPConfiguration.GetAllWirelessAPConfigurations()[ni.SpecificConfigId];
        }
        /// <summary>
        /// Gets configuration of AP mode
        /// </summary>
        /// <returns>Configuration of AP mode</returns>
        public static Wireless80211Configuration Get80211Configuration()
        {
            NetworkInterface ni = GetInterface();
            return Wireless80211Configuration.GetAllWireless80211Configurations()[ni.SpecificConfigId];
        }
        /// <summary>
        /// Enables AP Mode and reboots
        /// </summary>
        /// <param name="instance">Window instance to close</param>
        /// <param name="app">App to use for opening Reboot</param>
        /// <param name="SoftApIP">Static IP Address</param>
        public static void EnableAPMode(Window instance, OpenHaloApplication app, string SoftApIP)
        {
            NetworkInterface ni = GetInterface();
            WirelessAPConfiguration wirelessAPConfiguration = GetConfiguration();
            Wireless80211Configuration config = Get80211Configuration(); //Disable WIFI
            config.Options = Wireless80211Configuration.ConfigurationOptions.None;
            config.SaveConfiguration();
            Console.WriteLine("Disabled WIFI Client mode...");

            ni.EnableStaticIPv4(SoftApIP, "255.255.255.0", SoftApIP);
            Console.WriteLine($"Static IP {SoftApIP}/24 is set...");
            wirelessAPConfiguration.Authentication = System.Net.NetworkInformation.AuthenticationType.WPA2;
            wirelessAPConfiguration.Ssid = "OpenHalo Setup";
            wirelessAPConfiguration.Password = "OpenHalo";
            wirelessAPConfiguration.Options = WirelessAPConfiguration.ConfigurationOptions.AutoStart | WirelessAPConfiguration.ConfigurationOptions.Enable;
            wirelessAPConfiguration.MaxConnections = 3;
            wirelessAPConfiguration.SaveConfiguration();
            Console.WriteLine("Enabling AP Mode for 3 clients with SSID \"OpenHalo Setup\"...");
            Console.WriteLine("Reboot to save WIFI configuration....");
            app.MainWindow = new Reboot(app);
            instance.Close();
        }
        /// <summary>
        /// Enables Client Mode and reboots
        /// </summary>
        /// <param name="instance">Window instance to close</param>
        /// <param name="app">APp to use for opening Reboot</param>
        public static void EnableClientMode(Window instance, OpenHaloApplication app)
        {
            NetworkInterface ni = GetInterface();
            ni.EnableDhcp();
            Console.WriteLine("Enabled DHCP Client...");
            WirelessAPConfiguration wirelessAPConfiguration = GetConfiguration();
            Console.WriteLine("Disabled WIFI AP Mode....");
            wirelessAPConfiguration.Options = WirelessAPConfiguration.ConfigurationOptions.None;
            wirelessAPConfiguration.SaveConfiguration();
            Wireless80211Configuration config = Get80211Configuration();
            config.Options = Wireless80211Configuration.ConfigurationOptions.AutoConnect;
            config.SaveConfiguration();
            Console.WriteLine("Enable WIFI Client mode...");
            Console.WriteLine("Reboot to save WIFI configuration....");
            app.MainWindow = new Reboot(app);
            instance.Close();
        }
        /// <summary>
        /// Is AP Mode enabled?
        /// </summary>
        /// <returns>True if AP mode is enabled</returns>
        /// <remarks>There is chance for false positive, always use with <see cref="IsModeValid"/></remarks>
        public static bool IsAPModeEnabled()
        {
            if (GetInterface().IsDhcpEnabled)
                return false;
            if (GetConfiguration().Options != (WirelessAPConfiguration.ConfigurationOptions.Enable | WirelessAPConfiguration.ConfigurationOptions.AutoStart))
                return false;
            if (Get80211Configuration().Options != Wireless80211Configuration.ConfigurationOptions.None)
                return false;
            return true;
        }
        /// <summary>
        /// Is Client Mode enabled?
        /// </summary>
        /// <returns>True if Client mode is enabled</returns>
        /// <remarks>There is chance for false positive, always use with <see cref="IsModeValid"/></remarks>
        public static bool IsClientModeEnabled()
        {
            if (!GetInterface().IsDhcpEnabled)
                return false;
            if (GetConfiguration().Options != WirelessAPConfiguration.ConfigurationOptions.None)
                return false;
            if (Get80211Configuration().Options == Wireless80211Configuration.ConfigurationOptions.None)
                return false;
            return true;
        }
        /// <summary>
        /// Verifies that both <see cref="IsClientModeEnabled"/> and <see cref="IsAPModeEnabled"/> are not both true or both false at once.
        /// </summary>
        /// <returns>Returns validity of network configuration</returns>
        public static bool IsModeValid()
        {
            if (IsClientModeEnabled() && IsAPModeEnabled())
                return false;
            if (!IsClientModeEnabled() && !IsAPModeEnabled())
                return false;
            return true;
        }
    }
}
