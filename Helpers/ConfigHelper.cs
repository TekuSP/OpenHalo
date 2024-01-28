using System;
using System.IO;
using System.Text;

using nanoFramework.Json;

using OpenHalo.Configs;

using Windows.Storage;

namespace OpenHalo.Helpers
{
    public class ConfigHelper
    {
        /// <summary>
        /// Config path on internal storage
        /// </summary>
        public static string ConfigPath => "I:\\Configuration.json";
        /// <summary>
        /// Backup config file on internal storage
        /// </summary>
        public static string BackupConfigPath => "I:\\BackupConfiguration.json";
        /// <summary>
        /// Verifies existence and validity of config file
        /// </summary>
        /// <returns>Existence and validity of config file</returns>
        public static bool VerifyConfig()
        {
            if (File.Exists(ConfigPath))
            {
                Console.WriteLine("Found configuration file, reading...");
                var file = StorageFile.GetFileFromPath(ConfigPath);
                var configString = FileIO.ReadText(file);
                try
                {
                    _ = JsonConvert.DeserializeObject(configString, typeof(MainConfig));
                    Console.WriteLine("Configuration file verified successfully.");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Configuration file could not be parsed, stacktrace:");
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            Console.WriteLine("Configuration file missing on drive.");
            return false;
        }
        /// <summary>
        /// Verifies existence and validity of backup config file
        /// </summary>
        /// <returns>Existence and validity of backup config file</returns>
        public static bool VerifyBackupConfig()
        {
            if (File.Exists(BackupConfigPath))
            {
                Console.WriteLine("Found backup configuration file, reading...");
                var file = StorageFile.GetFileFromPath(BackupConfigPath);
                var configString = FileIO.ReadText(file);
                try
                {
                    _ = JsonConvert.DeserializeObject(configString, typeof(MainConfig));
                    Console.WriteLine("Backup configuration file verified successfully.");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Backup configuration file could not be parsed, stacktrace:");
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            Console.WriteLine("Backup configuration missing on drive.");
            return false;
        }
        /// <summary>
        /// Loads primary config, if not found, copies backup to primary and loads backup, if not possible, returns null
        /// </summary>
        /// <returns>Config or null</returns>
        public static MainConfig LoadConfig()
        {
            if (VerifyConfig())
            {
                var file = StorageFile.GetFileFromPath(ConfigPath);
                var configString = FileIO.ReadText(file);
                Console.WriteLine("Main configuration found and valid, returning config object.");
                return (MainConfig)JsonConvert.DeserializeObject(configString, typeof(MainConfig));
            }
            if (VerifyBackupConfig())
            {
                var file = StorageFile.GetFileFromPath(BackupConfigPath);
                var configString = FileIO.ReadText(file);
                Console.WriteLine("Backup configuration file found and valid, copying to main...");
                var config = (MainConfig)JsonConvert.DeserializeObject(configString, typeof(MainConfig));
                SaveConfig(config);
                Console.WriteLine("Returning backup configuration file as primary...");
                return config;
            }
            Console.WriteLine("Config primary and backup is missing!");
            return null; //File is missing
        }
        /// <summary>
        /// Loads config JSON file
        /// </summary>
        /// <returns>Returns JSON string</returns>
        public static string LoadConfigJSON()
        {
            if (VerifyConfig())
            {
                var file = StorageFile.GetFileFromPath(ConfigPath);
                var configString = FileIO.ReadText(file);
                Console.WriteLine("Main configuration found and valid, returning config json.");
                return configString;
            }
            if (VerifyBackupConfig())
            {
                var file = StorageFile.GetFileFromPath(BackupConfigPath);
                var configString = FileIO.ReadText(file);
                Console.WriteLine("Backup configuration file found and valid, copying to main...");
                var config = (MainConfig)JsonConvert.DeserializeObject(configString, typeof(MainConfig));
                SaveConfig(config);
                Console.WriteLine("Returning backup configuration json as primary...");
                return configString;
            }
            Console.WriteLine("Config primary and backup is missing!");
            return null; //File is missing
        }
        /// <summary>
        /// Saves config from JSON string
        /// </summary>
        /// <param name="json">JSON string to save</param>
        public static void SaveConfigJSON(string json)
        {
            var config = (MainConfig)JsonConvert.DeserializeObject(json, typeof(MainConfig));
            SaveConfig(config);
        }
        /// <summary>
        /// Saves config primary and backup
        /// </summary>
        /// <param name="config">Config to save</param>
        public static void SaveConfig(MainConfig config)
        {
            var data = JsonConvert.SerializeObject(config);

            //Primary config
            File.Create(ConfigPath);
            var file = StorageFile.GetFileFromPath(ConfigPath);
            FileIO.WriteText(file, data);
            Console.WriteLine("Primary config overwritten.");
            if (!VerifyConfig())
            {
                Console.WriteLine("Error! Validation of config file failed. Is memory damaged?");
                return;
            }

            //Backup config
            File.Create(BackupConfigPath);
            file = StorageFile.GetFileFromPath(BackupConfigPath);
            FileIO.WriteText(file, data);
            Console.WriteLine("Backup config overwritten.");
            if (!VerifyBackupConfig())
            {
                Console.WriteLine("Error! Validation of backup config file failed. Is memory damaged?");
                return;
            }
        }
    }
}
