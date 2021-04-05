using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using LocalAppDataFolder;

namespace Configs
{
    /// <summary>
    /// Contains the needed members to save and load a configurations file.
    /// </summary>
    public abstract class ConfigsTools
    {
        /// <summary>
        /// The name of the configurations file. File extension included. Change this if you want to change the file's
        /// name.
        /// </summary>
        [JsonIgnore]
        protected static string ConfigsFileName = "Configs.json";

        /// <summary>
        /// Saves the instance that inherents from ConfigsTools to the file as json.
        /// </summary>
        public void Save()
        {
            JsonSerializer serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented
            };
            LocalAppData localAppDataFolder = new LocalAppData();
            var configFile = this;
            using (FileStream stream = localAppDataFolder.GetFile(ConfigsFileName))
            using (StreamWriter sw = new StreamWriter(stream))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, configFile);
            }
        }

        /// <summary>
        /// Gets the current configurations from the configurations file.
        /// </summary>
        /// <typeparam name="T">The type of the configurations file model.</typeparam>
        /// <returns>An instance of the configurations file model filled with the current configurations in  the 
        /// configurations file. Creates a new file and returns a new instance of the class/model if no file was found.
        /// </returns>
        public static T GetConfigs<T>() where T : ConfigsTools, new()
        {
            T result;
            JsonSerializer serializer = new JsonSerializer();
            LocalAppData localAppDataFolder = new LocalAppData();
            using (FileStream stream = localAppDataFolder.GetFile(ConfigsFileName))
            using (StreamReader sw = new StreamReader(stream))
            using (JsonReader reader = new JsonTextReader(sw))
            {
                result = serializer.Deserialize<T>(reader);
            }
            // The file is empty then return a new instance of the model.
            if (result == null)
            {
                result = new T();
            }
            return result;
        }
    }
}
