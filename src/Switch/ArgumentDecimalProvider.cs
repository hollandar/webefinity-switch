using System;

namespace Webefinity.Switch
{

    /// <summary>
    /// A value provider that captures an unvalidated integer value.
    /// </summary>
    public class ArgumentDecimalProvider : IValueProvider
    {
        decimal? defaultValue;
        decimal? value;
        bool wasSet = false;

        /// <summary>
        /// Create a new integer value provider with an optional default value.
        /// </summary>
        /// <param name="defaultValue">The default value</param>
        public ArgumentDecimalProvider(decimal? defaultValue = null)
        {
            this.defaultValue = defaultValue;
        }

        /// <inheritdoc/>
        public ValidationResult Set(string? value)
        {
            this.wasSet = true;

            if (value == null)
            {
                this.value = null;
                return new ValidationResult();
            }

            if (decimal.TryParse(value, out var result))
            {
                this.value = result;
                return new ValidationResult(true);
            }

            return new ValidationResult(false, "An decimal number is required.");
        }

        /// <inheritdoc/>
        public void Usage(Action<string> log)
        {
            log("decimal");
        }

        /// <inheritdoc/>
        public object? Value => this.value;

        /// <inheritdoc/>
        public bool WasSet => this.wasSet;

        /// <inheritdoc/>
        public object? DefaultValue => this.defaultValue;
    }
}
