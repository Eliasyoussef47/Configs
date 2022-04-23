namespace Configs
{
    /// <summary>
    /// Specifies how to determine the name of the configurations file.
    /// </summary>
    public enum FileNameMethod
    {
        /// <summary>
        /// Use the name of the model class. This is the default.
        /// </summary>
        ModelName,

        /// <summary>
        /// Use the default configurations files name that is specified by the field
        /// <c>ConfigsTools.DefaultFileName</c>.
        /// </summary>
        DefaultName,

        /// <summary>
        /// Use the name specified in the static field <c>FileName</c> in the model class.
        /// </summary>
        FileNameField
    }
}