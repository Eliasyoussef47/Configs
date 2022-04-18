using Newtonsoft.Json;
using System;
using System.IO;
using LocalAppDataFolder;
using System.Reflection;

namespace Configs
{
    /// <summary>
    /// Contains the needed members to save and load a configurations file. Your configurations model should inherit 
    /// this class.
    /// </summary>
    public abstract class ConfigsTools
    {
        /// <summary>
        /// The name of the configurations file. File extension included.
        /// ATTENTION: if you want a costum file name add this field to your model(class) and set the value to the 
        /// desired file name. DO NOT CHANGE IT IN THE CONSTRUCTOR.
        /// <example>
        /// Example:
        /// <code>
        /// [JsonIgnore]
        /// public static string FileName = "MyConfigsFile.json";
        /// </code>
        /// </example>
        /// </summary>
        [JsonIgnore]
        protected static string FileName;

        /// <summary>
        /// The method that's going to be used everytime to determine how to determine the configurations file's name 
        /// to load and save the configurations into the configurations file.
        /// NOTE:
        /// You can add this field to your model and set its value so that it's going to be used by the 
        /// <c>GetConfigs()</c> and <c>Save()</c> methods everytime they are called. This way you can use the 
        /// desired file name without having to pass a <c>FileNameMethod</c> enum everytime you call 
        /// <c>GetConfigs(FileNameMethod fileNameMethod)</c> or <c>Save(FileNameMethod fileNameMethod)</c>. 
        /// Technically this is only useful if you plan on using <c>FileNameMethod.DefaultName</c> because in that 
        /// case you don't have to pass an argument everytime you call the aforementioned methods, the reasoning behind 
        /// that is when you want to use <c>FileNameMethod.FileNameField</c> it's less work to just add the 
        /// <c>FileName</c> static field and use the <c>GetConfigs()</c> and <c>Save()</c> methods which will 
        /// automatically use the <c>FileName</c> static field if the model doesn't have a <c>FileNameMethod</c> 
        /// static field and if you want to use the model's name then just don't add the <c>FileName</c> static field 
        /// and the <c>FileNameMethod</c> static field and just use the <c>GetConfigs()</c> and <c>Save()</c> 
        /// methods.
        /// </summary>
        /// <example>
        /// <code>
        /// [JsonIgnore]
        /// public static FileNameMethod FileNameMethod = FileNameMethod.DefaultName;
        /// </code>
        /// </example>
        [JsonIgnore]
        protected static FileNameMethod FileNameMethod;

        /// <summary>
        /// The default name of the configurations file. This value is used when FileNameMethod.DefaultName is used. 
        /// </summary>
        [JsonIgnore]
        public static readonly string DefaultFileName = "Configs.json";

        /// <summary>
        /// Automatically determines the name of the configurations file depending on the model's type and chosen 
        /// <c>Configs.FileNameMethod</c> enum.
        /// </summary>
        /// <param name="type">Depending on <paramref name="fileNameMethod"/> the name of this type, the value of a 
        /// field in this type or something else might be used for the proces of determining the configurations file's 
        /// name.</param>
        /// <param name="fileNameMethod">A "Configs.FileNameMethod" option that specifies which method should be used to 
        /// determine the configurations file's name.</param>
        /// <returns>The name of the configurations file determined by <paramref name="type"/> and 
        /// <paramref name="fileNameMethod"/>. File extension included</returns>
        /// <exception cref="System.Reflection.TargetException">In the .NET for Windows Store apps or the Portable 
        /// Class Library, catch System.Exception instead. The field is non-static and obj is null.</exception>
        /// <exception cref="System.NotSupportedException">A field is marked literal, but the field does not have one 
        /// of the accepted literal types.</exception>
        /// <exception cref="System.FieldAccessException">In the .NET for Windows Store apps or the Portable Class 
        /// Library, catch the base class exception, System.MemberAccessException, instead. The caller does not have 
        /// permission to access this field.</exception>
        /// <exception cref="System.ArgumentException">The method is neither declared nor inherited by the class of 
        /// obj.</exception>
        protected static string DetermineFileName(Type type, FileNameMethod fileNameMethod)
        {
            if (fileNameMethod == FileNameMethod.ModelName)
            {
                return type.Name + ".json";
            }
            else if (fileNameMethod == FileNameMethod.DefaultName)
            {
                return DefaultFileName;
            }
            else if (fileNameMethod == FileNameMethod.FileNameField)
            {
                FieldInfo fileNameFieldInfo = type.GetField("FileName", BindingFlags.Static | BindingFlags.Public);

                if (fileNameFieldInfo == null)
                {
                    throw new MissingFieldException(
                        $"Field FileNam wasn't found in {type.Name}");
                }

                return (string)fileNameFieldInfo.GetValue(null);
            }
            else
            {
                return type.Name + ".json";
            }
        }

        /// <summary>
        /// Handles the actual conversion from json, deserialization and loading of a configurations file and creating 
        /// the file if it doesn't exist.
        /// </summary>
        /// <typeparam name="T">The type of the configurations file model. Type must inherit from ConfigsTools.
        /// </typeparam>
        /// <param name="fileName">The name of the configurations file to load from. File extension included.</param>
        /// <returns>An instance of the configurations file model populated with the current configurations in  the 
        /// configurations file. Creates a new file and returns a new instance of the model if no file was found.
        /// </returns>
        protected static T GetConfigsBase<T>(string fileName) where T : ConfigsTools, new()
        {
            T result;
            var serializer = new JsonSerializer();
            var localAppDataFolder = new LocalAppData();
            using (FileStream stream = localAppDataFolder.GetFile(fileName))
            using (var sw = new StreamReader(stream))
            using (JsonReader reader = new JsonTextReader(sw))
            {
                result = serializer.Deserialize<T>(reader);
            }

            result ??= new T();

            return result;
        }

        /// <summary>
        /// Gets the current configurations from the configurations file.
        /// The configurations file's name is determined by checking some conditions. First this method checks if 
        /// there is a <c>FileNameMethod</c> static field in the model if there is one then the value of it is used to 
        /// determine how the name of configurations file is determined, if that doesn't exist then the method looks 
        /// for the <c>FileName</c> static field in the model and tries to use its value as the configurations file's 
        /// name, if that doesn't exist either then the name of the model(class) is used as the name of the 
        /// configurations file.
        /// </summary>
        /// <typeparam name="T">The type of the configurations file model. Type must inherit from ConfigsTools.
        /// </typeparam>
        /// <returns>An instance of the configurations file model populated with the current configurations in  the 
        /// configurations file. Creates a new file and returns a new instance of the model if no file was found.
        /// </returns>
        public static T GetConfigs<T>() where T : ConfigsTools, new()
        {
            FieldInfo fileNameMethodFieldInfo = typeof(T).GetField("FileNameMethod", BindingFlags.Static |
                BindingFlags.Public);
            if (fileNameMethodFieldInfo != null)
            {
                FileNameMethod fileNameMethod = (FileNameMethod)fileNameMethodFieldInfo.GetValue(null);

                return GetConfigs<T>(fileNameMethod);
            }
            else
            {
                FieldInfo fileNameFieldInfo = typeof(T).GetField("FileName", BindingFlags.Static |
                                                                             BindingFlags.Public);
                if (fileNameFieldInfo != null)
                {
                    string fileName = (string)fileNameFieldInfo.GetValue(null);

                    return GetConfigs<T>(fileName);
                }
                else
                {
                    return GetConfigs<T>(FileNameMethod.ModelName);
                }
            }
        }

        /// <summary>
        /// Gets the current configurations from the configurations file.
        /// </summary>
        /// <typeparam name="T">The type of the configurations file model. Type must inherit from ConfigsTools.
        /// </typeparam>
        /// <param name="fileNameMethod">A "Configs.FileNameMethod" option that specifies which method should be used to 
        /// determine the configurations file's name.</param>
        /// <returns>An instance of the configurations file model populated with the current configurations in  the 
        /// configurations file. Creates a new file and returns a new instance of the model if no file was found.
        /// </returns>
        public static T GetConfigs<T>(FileNameMethod fileNameMethod) where T : ConfigsTools, new()
        {
            return GetConfigsBase<T>(DetermineFileName(typeof(T), fileNameMethod));
        }

        /// <summary>
        /// Gets the current configurations from the configurations file.
        /// </summary>
        /// <typeparam name="T">The type of the configurations file model. Type must inherit from ConfigsTools.
        /// </typeparam>
        /// <param name="fileName">The name of the configurations file. File extension included.</param>
        /// <returns>An instance of the configurations file model populated with the current configurations in  the 
        /// configurations file. Creates a new file and returns a new instance of the model if no file was found.
        /// </returns>
        public static T GetConfigs<T>(string fileName) where T : ConfigsTools, new()
        {
            return GetConfigsBase<T>(fileName);
        }

        /// <summary>
        /// Handles the actual conversion to json, serialization and saving to the configurations file as json.
        /// </summary>
        /// <param name="fileName">The name of the configurations file to save to. File extension included.</param>
        protected void SaveBase(string fileName)
        {
            var serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented
            };
            ConfigsTools configFile = this;
            var localAppDataFolder = new LocalAppData();

            using FileStream stream = localAppDataFolder.GetFile(fileName);
            using var sw = new StreamWriter(stream);
            using JsonWriter writer = new JsonTextWriter(sw);
            serializer.Serialize(writer, configFile);
        }

        /// <summary>
        /// Saves the instance to the configurations file as json.
        /// The configurations file's name is determined by checking some conditions. First this method checks if 
        /// there is a <c>FileNameMethod</c> static field in the model if there is one then the value of it is used to 
        /// determine how the name of configurations file is determined, if that doesn't exist then the method looks 
        /// for the <c>FileName</c> static field in the model and tries to use its value as the configurations file's 
        /// name, if that doesn't exist either then the name of the model(class) is used as the name of the 
        /// configurations file.
        /// </summary>
        public void Save()
        {
            FieldInfo fileNameMethodFieldInfo = GetType().GetField("FileNameMethod", BindingFlags.Static |
                BindingFlags.Public);
            if (fileNameMethodFieldInfo != null)
            {
                var fileNameMethod = (FileNameMethod)fileNameMethodFieldInfo.GetValue(null);
                Save(fileNameMethod);
            }
            else
            {
                FieldInfo fileNameFieldInfo = GetType().GetField("FileName", BindingFlags.Static |
                                                                             BindingFlags.Public);
                if (fileNameFieldInfo != null)
                {
                    string fileName = (string)fileNameFieldInfo.GetValue(null);
                    Save(fileName);
                }
                else
                {
                    Save(FileNameMethod.ModelName);
                }
            }
        }

        /// <summary>
        /// Saves the instance to the configurations file as json. The name of the file is determined by the 
        /// <paramref name="fileNameMethod"/> parameter.
        /// </summary>
        /// <param name="fileNameMethod">A "Configs.FileNameMethod" option that specifies which method should be used 
        /// to determine the configurations file's name.</param>
        /// <exception cref="System.Reflection.TargetException">In the .NET for Windows Store apps or the Portable 
        /// Class Library, catch System.Exception instead. The field is non-static and obj is null.</exception>
        /// <exception cref="System.NotSupportedException">A field is marked literal, but the field does not have one 
        /// of the accepted literal types.</exception>
        /// <exception cref="System.FieldAccessException">In the .NET for Windows Store apps or the Portable Class 
        /// Library, catch the base class exception, System.MemberAccessException, instead. The caller does not have 
        /// permission to access this field.</exception>
        /// <exception cref="System.ArgumentException">The method is neither declared nor inherited by the class of 
        /// obj.</exception>
        public void Save(FileNameMethod fileNameMethod)
        {
            SaveBase(DetermineFileName(GetType(), fileNameMethod));
        }

        /// <summary>
        /// Saves the instance to the configurations file as json. The name of the file is determined by the 
        /// <paramref name="fileName"/> parameter.
        /// </summary>
        /// <param name="fileName">The name of the configurations file to save to.</param>
        public void Save(string fileName)
        {
            SaveBase(fileName);
        }
    }
}