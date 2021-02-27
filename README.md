![Configs icon](/Configs/Assets/icons8-window-settings-96.png)
# Configs

https://www.nuget.org/packages/Configs/

A simple way to save the configurations of a C# app as a json file.

The json file(s) is saved in AppData/Local and there is currently no way to change that.
# How to use
Before we start, install the package from NuGet.

This package uses [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) and the idea is to make a class that's going to be used as the model for the configuration file.
## Setup
Make a class that inherits from ConfigsTools. This class is the model for your configurations file, so it details how the json file is going to look like.

	using Configs;
	using Newtonsoft.Json;

	namespace MyApp
	{
	    class AppConfigs : ConfigsTools
	    {
	        public AppConfigs()
	        {
	            ConfigsFileName = "MyConfigurations.json";
	        }

	        [JsonProperty("activated", Required = Required.AllowNull)]
	        public bool Activated { get; set; }

	        [JsonProperty("randomString", Required = Required.Default)]
	        public string RandomString { get; set; }

	        [JsonProperty("registeredTriggerSenders", Required = Required.AllowNull)]
	        public List<ImportantThing> ImportantStuff { get; set; }

	        public ImportantThing GetImportantThing(string owner)
	        {
	            return ImportantStuff.Find(x => x.OwnerName.Equals(owner));
	        }
	    }
	}
Pretty much anything supported by Newtonsoft.Json and is serializable can be used in this model. You can of course make multiple models each with a different ConfigsFileName to have multiple configurations files.

## Usage:
### Getting the file's values:    

    // Get the current app configurations from the json configurations file.
    AppConfigs appConfigs = Configs.ConfigsTools.GetConfigs<AppConfigs>();
    // Get a value from the configurations file.
    bool appConfigActivated = appConfigs.Activated;
    // This the value at time GetConfigs was called. If you want updated values then you have to call GetConfigs again.

### Editing and saving the file:

    // Get the current app configurations from the json configurations file.
    AppConfigs appConfigs = Configs.ConfigsTools.GetConfigs<AppConfigs>();
    // Change value.
    appConfigs.Activated = false;
    // Save the file.
    appConfigs.Save();

## Result

    {
	"Activated": true,
	"RandomString": "Hey",
	"ImportantStuff": [
			{
				"ownerName": "Elias Youssef"
			},
			{
				"ownerName": "Slim Shady"
			}
		]
	}

<a target="_blank" href="https://icons8.com/icons/set/window-settings">Window Settings icon</a> icon by <a target="_blank" href="https://icons8.com">Icons8</a>

