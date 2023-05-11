using System;
using System.Collections.Generic;
using System.Linq;

namespace Webefinity.Switch
{
    /// <summary>
    /// A fluent builder for setting up options and their value providers.
    /// </summary>
    public class ArgumentsBuilder
    {
        string[] args;
        List<ArgumentOption> options = new();

        /// <summary>
        /// Construct a new argument builder, taking command line arguments from Environment.GetCommandLineArgs.
        /// You can override this argument source after construction by calling SetArguments.
        /// </summary>
        public ArgumentsBuilder()
        {
            this.args = Environment.GetCommandLineArgs().Skip(1).ToArray();
        }

        /// <summary>
        /// Add an option.
        /// This returns and ArgumentOption which can be extended via a variety of fluent methods
        /// </summary>
        /// <param name="longVersion">The long form flag as a string, without the leading --.</param>
        /// <param name="shortVersion">A short form flag as a char, without the leading -, or null if there is no short form.</param>
        /// <param name="isDefault">True if this is a default option (the value can be passed as the first argument without the flag.</param>
        /// <returns>An ArgumentOption, for fluid extension.</returns>
        /// <exception cref="MultipleDefaultsException">Thrown if more than one default option is added.</exception>
        public ArgumentOption Add(string longVersion, char? shortVersion = null, bool isDefault = false)
        {
            if (options.Where(r => r.Default).Any() && isDefault)
            {
                throw new MultipleDefaultsException("Only one option can be the default option.");
            }

            if (options.Where(r => r.Long == longVersion || (r.Short != null && r.Short == shortVersion)).Any())
            {
                throw new ArgumentException($"Neither the long flag {longVersion}, nor the short flag {shortVersion} can be repeated on multiple options.");
            }

            var argumentOption = new ArgumentOption(longVersion, shortVersion, isDefault);
            this.options.Add(argumentOption);

            return argumentOption;
        }

        /// <summary>
        /// Override the arguments used after construction of the builder.
        /// Useful for test cases.
        /// </summary>
        /// <param name="args">The arguments to use instead of the environment arguments.</param>
        public void SetArguments(params string[] args)
        {
            this.args = args;
        }

        /// <summary>
        /// Construct a handler and parse the arguments using the options.
        /// Calling Build causes the arguments to be validated and the time Build is called.
        /// </summary>
        /// <returns>The handler, which contains information about validity and the values of the arguments</returns>
        public ArgumentsHandler Build()
        {
            return new ArgumentsHandler(args, options);
        }
    }
}
