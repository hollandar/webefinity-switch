using System;

namespace Webefinity.Switch
{

    /// <summary>
    /// A value provider that captures a string value and converts it to an enumeration value which is equivalent to that string value.
    /// Usage provides the string variants of each of your enum values automatically.
    /// </summary>
    public class ArgumentEnumProvider<TEnum> : IValueProvider where TEnum: Enum
    {
        object? value;
        bool wasSet = false;
        object? defaultValue;

        /// <summary>
        /// Construct a new enum provider with an optional default value
        /// </summary>
        /// <param name="value"></param>
        public ArgumentEnumProvider(object? value = null)
        {
            this.defaultValue = value;
        }

        /// <inheritdoc/>
        public object? Value => this.value;

        /// <inheritdoc/>
        public bool WasSet => this.wasSet;

        /// <inheritdoc/>
        public object? DefaultValue => this.defaultValue;

        /// <inheritdoc/>
        public ValidationResult Set(string? value)
        {
            this.wasSet = true;

            if (Enum.TryParse(typeof(TEnum), value, true, out var result))
            {
                this.value = result;
            }
            return new ValidationResult(true);
        }

        /// <inheritdoc/>
        public void Usage(Action<string> log)
        {
            log(String.Join(", ", Enum.GetNames(typeof(TEnum))));
        }
    }
}
