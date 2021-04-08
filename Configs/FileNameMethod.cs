using System;
using System.Collections.Generic;
using System.Text;

namespace Configs
{
    /// <summary>
    /// Specifies how to determine the name of the configurations file.
    /// </summary>
    public enum FileNameMethod
    {
        /// <summary>
        /// Use The name of the configurations file is going to be the name of the model class. This is the default.
        /// </summary>
        ModelName,
        /// <summary>
        /// Use the default configurations files name that is specified by the field 
        /// <c>ConfigsTools.DefaultFileName</c>.
        /// </summary>
        DefaultName,
        /// <summary>
        /// Use the name sepcified in the static field <c>FileName</c> in the model class.
        /// </summary>
        FileNameField
    }
}
