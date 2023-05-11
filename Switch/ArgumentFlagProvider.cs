using System;
using System.Runtime.InteropServices;

namespace Webefinity.Switch
{
    /// <summary>
    /// A true or false value provider.
    /// The true default causes this flag to be set if just the flag is passed, without the true or false value trailing it.
    /// </summary>
    public class ArgumentFlagProvider : IValueProvider
    {
        bool? value;
        bool wasSet = false;
        bool? defaultValue;

        /// <summary>
        /// Construct a flag, passing a default value.
        /// If true is not specified after the flag, it will receive its true.
        /// In general that default should be false.
        /// </summary>
        /// <param name="defaultValue"></param>
        public ArgumentFlagProvider(bool? defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        /// <inheritdoc/>
        public object? Value => value;

        /// <inheritdoc/>
        public object? DefaultValue => defaultValue;

        /// <inheritdoc/>
        public bool WasSet => wasSet;

        /// <inheritdoc/>
        public ValidationResult Set(string? value)
        {
            this.wasSet = true;

            if (bool.TryParse(value, out var result))
            {
                this.value = result;
                return new ValidationResult();
            }

            if (value == null)
            {
                this.value = true;
                return new ValidationResult();
            }

            return new ValidationResult(false, $"{value} is not a valid boolean value.");
        }

        /// <inheritdoc/>
        public void Usage(Action<string> log)
        {
            log("true, false");
        }
    }
}
