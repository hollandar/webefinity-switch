using System;
using System.Collections.Generic;
using System.Linq;

namespace Webefinity.Switch
{
    /// <summary>
    /// Validates the arguments according to the list of options.
    /// </summary>
    public class ArgumentsHandler
    {
        private string[] args;
        private IReadOnlyCollection<ArgumentOption> options;
        private ValidationResult validationResult = new();

        /// <summary>
        /// Validate arguments.
        /// 
        /// Constructing the ArgumentsHandler immediately validates the passed options against the passed argument list.
        /// </summary>
        /// <param name="args">The arguments to use.</param>
        /// <param name="options">The options to validate the arguments against</param>
        /// <exception cref="ValueProviderException">Thrown if any option is missing a value provider.</exception>
        public ArgumentsHandler(string[] args, IReadOnlyCollection<ArgumentOption> options)
        {
            this.args = args;
            this.options = options;

            if (options.Where(r => r.ValueProvider == null).Any())
                throw new ValueProviderException("All options must have a value provider.");

            int i = 0;

            // Handle the default argument
            if (options.Any(r => r.Default) && args.Length > 0)
            {
                var arg0 = args[i];
                var isSwitch = options.Any(r => r.IsMatch(arg0));
                if (!isSwitch && options.Any(r => r.Default))
                {
                    var option = options.Single(r => r.Default);
                    validationResult.Combine(option.ValueProvider!.Set(arg0));
                    i++;
                }
            }

            // And now the rest
            for (; i < args.Length; i++)
            {
                bool foundOption = false;
                string arg = args[i];
                foreach (var option in options)
                {
                    if (option.IsMatch(arg))
                    {
                        string? val = null;
                        if (i < args.Length - 1)
                        {
                            val = args[i + 1];
                        }
                        if ((i == args.Length - 1 || val.StartsWith("-")) && option.ValueProvider!.DefaultValue != null)
                        {
                            validationResult.Combine(option.ValueProvider!.Set(null));
                        }
                        else
                        {
                            validationResult.Combine(option.ValueProvider!.Set(val));
                            ++i;
                        }
                        foundOption = true;
                    }
                }
                if (!foundOption)
                {
                    this.validationResult.Combine(new ValidationResult(false, $"{arg} is not a valid option."));
                }
            }

            foreach (var option in options)
            {
                if (option.Required && option.ValueProvider?.WasSet == false)
                {
                    this.validationResult.Combine(new ValidationResult(false, $"The option {option.Long} is a required option."));
                }
            }
        }

        /// <summary>
        /// After parsing, were the arguments valid?
        /// </summary>
        public bool IsValid => this.validationResult.IsValid;

        /// <summary>
        /// What were the errors recorded, in the case of invalidity.
        /// A value provider need not add errors, but it would be good practice for it to do so.
        /// </summary>
        public ICollection<string> Errors => this.validationResult.Errors;

        /// <summary>
        /// Get the value of an option as a strongly typed value, by long option name.
        /// </summary>
        /// <typeparam name="T">The type you expect.</typeparam>
        /// <param name="longOption">The long version of the option flag, without the leading --.</param>
        /// <returns>The value.</returns>
        public T? GetValue<T>(string longOption)
        {
            var option = this.options.Where(r => r.Long == longOption).FirstOrDefault();
            if (option != null)
            {
                var value = option.ValueProvider!.Value;
                if (value == null && option.ValueProvider.DefaultValue != null)
                {
                    return (T?)Convert.ChangeType(option.ValueProvider!.DefaultValue, typeof(T));
                }
                else if (value == null)
                {
                    return default(T?);
                }
                else if (value is T)
                {
                    return (T?)value;
                }
                else
                {
                    return (T?)Convert.ChangeType(value, typeof(T));
                }

            }

            return default(T?);
        }

        /// <summary>
        /// Report the usage using a logger, or default to Console.Write as the logger.
        /// </summary>
        /// <param name="name">The name of the application.</param>
        /// <param name="version">The version of the application.</param>
        /// <param name="showErrors">Should errors be logged, or suppressed?</param>
        /// <param name="log">The logger, or null to use Console.Writeline</param>
        public void Usage(string name, string version, bool showErrors = true, Action<string>? log = null)
        {
            if (log == null) log = e => { Console.Write(e); };

            log($"{name}\n");
            log($"{version}\n");
            log("---------\n");
            foreach (var option in options)
            {
                log($"--{option.Long}");
                if (option.Short != null)
                {
                    log($", -{option.Short}");
                }

                log("\t");
                option.ValueProvider!.Usage(log);
                log("\t");
                if (option.Description != null)
                {
                    log(option.Description);
                }
                log("\n");
            }
            log("\n");

            if (showErrors && !IsValid)
            {
                log("Errors:\n");
                foreach (var error in Errors)
                {
                    log($" * {error}\n");
                }
            }
        }
    }
}
