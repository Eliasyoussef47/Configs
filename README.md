![Configs icon](https://raw.githubusercontent.com/Eliasyoussef47/Configs/master/Configs/Assets/icons8-window-settings-96.png)

# Configs

https://www.nuget.org/packages/Configs/

A simple way to save the configurations of a C# app as a json file.

The json file(s) is saved in AppData/Local/{YourAppName} and there is currently no way to change that.

**If you have any questionS you can post them in the discussions section.**

# How to use

Before we start, install the package from NuGet.

This package uses [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) and the idea is to make a class that's
going to be used as the model for the configuration file.

## Setup

Make a class that inherits from ConfigsTools. This class is the model for your configurations file, so it details how the json file is going to look like.
Model example:

```C#
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

	[JsonProperty("importantStuff", Required = Required.AllowNull)]
	public List<ImportantThing> ImportantStuff { get; set; }

	public ImportantThing GetImportantThing(string owner)
	{
	    return ImportantStuff.Find(x => x.OwnerName.Equals(owner));
	}
    }
}
```

Pretty much anything supported by Newtonsoft.Json and is serializable can be used in this model. You can of course make
multiple models each with a different ConfigsFileName to have multiple configurations files.

## Usage

### Getting the file's values

```C#
using Configs;

...

// Get the current app configurations from the json configurations file.
AppConfigs appConfigs = ConfigsTools.GetConfigs<AppConfigs>();
// Get a value from the configurations file.
bool appConfigActivated = appConfigs.Activated;
```

### Editing and saving the file

```C#
using Configs;

...

// Get the current app configurations from the json configurations file.
AppConfigs appConfigs = ConfigsTools.GetConfigs<AppConfigs>();
// Change value.
appConfigs.Activated = false;
// Save the file.
appConfigs.Save();
```

## Result

```json
{
  "activated": false,
  "randomString": "Hey",
  "importantStuff": [
    {
      "ownerName": "Elias Youssef"
    },
    {
      "ownerName": "Slim Shady"
    }
  ]
}
```

[Window Settings icon](https://icons8.com/icons/set/window-settings) icon by [Icons8](https://icons8.com)
