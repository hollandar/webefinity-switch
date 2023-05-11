using System;
using System.IO;

namespace Webefinity.Switch
{
    /// <summary>
    /// Ensures that a directory is passed to an argument, and optionally that the directory exists.
    /// </summary>
    public class ArgumentDirectoryProvider : IValueProvider
    {
        string? filename;
        bool mustExist;
        bool wasSet = false;
        string? defaultValue;

        /// <param name="mustExist">Does the director need to exist for the argument to be valid?</param>
        public ArgumentDirectoryProvider(bool mustExist, string? defaultValue = null)
        {
            this.mustExist = mustExist;
            this.defaultValue = defaultValue;

            if (this.mustExist && defaultValue != null && !Directory.Exists(defaultValue))
            {
                throw new ArgumentException("Directory must exist, but the default defines a directory that does not exist.");
            }
        }

        /// <inheritdoc/>
        public object? Value => this.filename;

        /// <inheritdoc/>
        public bool WasSet => this.wasSet;

        /// <inheritdoc/>
        public object? DefaultValue => defaultValue;

        /// <inheritdoc/>
        public ValidationResult Set(string? value)
        {
            this.wasSet = true;

            if (value != null && (this.mustExist && Directory.Exists(value)) || !this.mustExist)
            {
                this.filename = value;
                return new ValidationResult(true);
            }


            return new ValidationResult(false, $"The directory {value} does not exist.");
        }

        /// <inheritdoc/>
        public void Usage(Action<string> log)
        {
            log("directory");
            if (mustExist)
            {
                log(" (required)");
            }
        }
    }
}
