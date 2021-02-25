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
        public void Save()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            var configFile = this;
            LocalAppData localAppDataFolder = new LocalAppData();
            FieldInfo fieldInfo = this.GetType().GetField("ConfigsFileName", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            string value = (string)fieldInfo.GetValue(null);
            using (FileStream stream = localAppDataFolder.GetFile(value))
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
            FieldInfo fieldInfo = typeof(T).GetField("ConfigsFileName", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            string value = (string)fieldInfo.GetValue(null);
            LocalAppData localAppDataFolder = new LocalAppData();
            using (FileStream stream = localAppDataFolder.GetFile(value))
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
