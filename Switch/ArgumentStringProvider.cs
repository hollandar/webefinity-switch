using System;

namespace Webefinity.Switch
{

    /// <summary>
    /// A value provider that captures an unvalidated string value.
    /// </summary>
    public class ArgumentStringProvider : IValueProvider
    {
        string? defaultValue;
        string? value;
        bool wasSet = false;

        /// <summary>
        /// Create a new string value provider with an optional default value.
        /// </summary>
        /// <param name="defaultValue">The default value</param>
        public ArgumentStringProvider(string? defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        /// <inheritdoc/>
        public ValidationResult Set(string? value)
        {
            this.wasSet = true;

            this.value = value ?? defaultValue;
            return new ValidationResult(true);
        }

        /// <inheritdoc/>
        public void Usage(Action<string> log)
        {
            log("string");
        }

        /// <inheritdoc/>
        public object? Value => this.value;

        /// <inheritdoc/>
        public bool WasSet => this.wasSet;

        /// <inheritdoc/>
        public object? DefaultValue => this.defaultValue;
    }
}
