using System;
using System.IO;

namespace Webefinity.Switch
{
    /// <summary>
    /// A value provider that accepts a filename, and optionally validates that the file exists.
    /// </summary>
    public class ArgumentFilenameProvider : IValueProvider
    {
        string? filename;
        bool mustExist;
        bool wasSet = false;
        string? defaultValue;

        /// <param name="mustExist">Should the file be checked to make sure it exists?</param>
        public ArgumentFilenameProvider(bool mustExist, string? defaultValue = null)
        {
            this.mustExist = mustExist;
            this.defaultValue = defaultValue;

            if (this.mustExist && defaultValue != null && !File.Exists(defaultValue))
            {
                throw new ArgumentException("File must exist, but the default defines a file that does not exist.");
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

            if ((this.mustExist && File.Exists(value)) || !this.mustExist)
            {
                this.filename = value;
                return new ValidationResult(true);
            }


            return new ValidationResult(false, $"The file {value} does not exist.");
        }

        /// <inheritdoc/>
        public void Usage(Action<string> log)
        {
            log("filename");
            if (mustExist)
            {
                log(" (required)");
            }
        }
    }
}
