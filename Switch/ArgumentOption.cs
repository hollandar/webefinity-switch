namespace Webefinity.Switch
{
    /// <summary>
    /// The ArgumentOption is the definition of an option.
    /// It can define long and short versions of the flag, as in --long and -s.
    /// A value provider accepts the string in the field after the flag, and converts it to an appropriate value for the option.
    /// Once the argument is parsed by the ArgumentHandler, the value provider holds and serves the values of the parsed arguments.
    /// 
    /// You can use the fluent extensions to add a varierty of value providers, to mark the option as required, and to mark it as Default.
    /// 
    /// A default option can be passed as the first argument on the command line without the flag.  This is a good way to handle commands.
    /// </summary>
    public class ArgumentOption
    {
        string longVersion;
        char? shortVersion;
        bool isDefault;
        IValueProvider? valueProvider;
        bool required = false;
        string description = "No description provided.";

        /// <summary>
        /// Create a new argument option, defining the long and short versions, and whether this is the single default command.
        /// </summary>
        /// <param name="longVersion">A long flag, matched as --{longVersion} against the arguments</param>
        /// <param name="shortVersion">An optional short flag, matched as -{shortVersion}, or null for no short version.</param>
        /// <param name="isDefault">Is this the default argument? A default argument as the first argument does not require the flag.</param>
        public ArgumentOption(string longVersion, char? shortVersion, bool isDefault)
        {
            this.longVersion = longVersion;
            this.shortVersion = shortVersion;
            this.isDefault = isDefault;
        }

        /// <summary>
        /// The long argument flag.
        /// </summary>
        public string Long => this.longVersion;

        /// <summary>
        /// The short argument flag.
        /// Null if the short version is not used.
        /// </summary>
        public char? Short => this.shortVersion;

        /// <summary>
        /// Is this a default argument?
        /// 
        /// A command argument might be defined as:
        /// builder.Add("command", isDefault: true).AcceptString()
        /// 
        /// Calling the command line "Command the_command" would then cause "the_command" to be the value of the "command" argument.
        /// There is only one Default flag.
        /// </summary>
        public bool Default => this.isDefault;

        /// <summary>
        /// The value provider for this option.
        /// </summary>
        public IValueProvider? ValueProvider => this.valueProvider;

        /// <summary>
        /// Do we require a value to be passed on the command line for this option?
        /// </summary>
        public bool Required => this.required;
        
        /// <summary>
        /// The description of this option, for the usage output.
        /// </summary>
        public string Description => this.description;

        /// <summary>
        /// Check an argument to see if it is a matching flag for this option?
        /// </summary>
        /// <param name="arg">The argument string</param>
        /// <returns>True, if the argument matches the flag for this option.</returns>
        public bool IsMatch(string arg)
        {
            return arg == $"--{longVersion}" || (shortVersion != null && arg == $"-{shortVersion}");
        }

        /// <summary>
        /// Mark this option as required.
        /// </summary>
        /// <returns></returns>
        public ArgumentOption MakeRequired()
        {
            this.required = true;
            return this;
        }

        /// <summary>
        /// Provide an option description for the usage.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public ArgumentOption WithDescription(string description)
        {
            this.description = description;
            return this;
        }

        /// <summary>
        /// Add a custom provider for this option
        /// </summary>
        /// <param name="valueProvider">The custom provider instance.</param>
        /// <returns>This option, for fluid extensions.</returns>
        public ArgumentOption AddProvider(IValueProvider valueProvider)
        {
            this.valueProvider = valueProvider;
            return this;
        }
    }
}
