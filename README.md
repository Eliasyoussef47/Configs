![Configs icon](https://raw.githubusercontent.com/Eliasyoussef47/Configs/master/Configs/Assets/icons8-window-settings-96.png)

# Configs

https://www.nuget.org/packages/Configs/

A simple way to save the configurations/settings of a C# app as a json file.

The json file(s) is saved in AppData/Local/{YourAppName} and there is currently no way to change that.

**If you have any questions you can post them in the discussions section.**

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
using System.Collections.Generic;

namespace ConfigsTest
{
    class AppConfigs : ConfigsTools
    {
        public AppConfigs()
        {
            ImportantStuff = new List<ImportantThing>();
        }

        [JsonProperty("activated", Required = Required.AllowNull)]
        public bool Activated { get; set; }

        [JsonProperty("randomString", Required = Required.Default)]
        public string RandomString { get; set; }

        [JsonProperty("importantStuff", Required = Required.AllowNull)]
        public List<ImportantThing> ImportantStuff { get; set; }

        // You can add methods that you can use when loading the config file
        public void ChangeFirstThing()
        {
            ImportantStuff[0].OwnerName = "Slim Shady";
        }
    }
}
```

Pretty much anything supported by Newtonsoft.Json and is serializable can be used in this model. You can of course make
multiple models to have multiple configurations files.
By default the name of the json file is the name of model/class.

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
appConfigs.RandomString = "this text is going to be saved in the json file";
// Add object to list.
appConfigs.ImportantStuff.Add(new ImportantThing("Elias Youssef"));
// Use methods that are inside the model
appConfigs.ChangeFirstThing();
// Save the file.
appConfigs.Save();
```

## Result

```json
{
  "activated": false,
  "randomString": "this text is going to be saved in the json file",
  "importantStuff": [
    {
      "OwnerName": "Slim Shady"
    }
  ]
}
```

[Window Settings icon](https://icons8.com/icons/set/window-settings) icon by [Icons8](https://icons8.com)
