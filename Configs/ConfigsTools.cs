using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LocalAppDataFolder;

namespace Configs
{
    abstract class ConfigsTools
    {
        [JsonIgnore]
        protected static string ConfigsFileName = "Configs.json";

        public void Save()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            LocalAppData localAppDataFolder = new LocalAppData();
            var configFile = this;
            using (FileStream stream = localAppDataFolder.GetFile(ConfigsFileName))
            using (StreamWriter sw = new StreamWriter(stream))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, configFile);
            }
        }

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
            if (result == null)
            {
                result = new T();
            }
            return result;
        }
    }
}
