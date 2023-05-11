using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Webefinity.Switch
{
    /// <summary>
    /// Fluid extensions that define value providers for options.
    /// </summary>
    public static class ArgumentOptionExtensions
    {
        /// <summary>
        /// Add a value provider that accepts a filename.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="mustExist">Is the file validated for existence?</param>
        /// <returns>The option, for fluid extension.</returns>
        public static ArgumentOption AcceptFilename(this ArgumentOption option, bool mustExist = false, string? defaultValue = null)
        {
            option.AddProvider(new ArgumentFilenameProvider(mustExist, defaultValue));
            return option;
        }

        /// <summary>
        /// Add a value provider that accepts a directory.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="mustExist">Is the directory validated for existence?</param>
        /// <returns>The option, for fluid extension.</returns>
        public static ArgumentOption AcceptDirectory(this ArgumentOption option, bool mustExist = false, string? defaultValue = null)
        {
            option.AddProvider(new ArgumentDirectoryProvider(mustExist, defaultValue));
            return option;
        }

        /// <summary>
        /// Add a value provider that accepts a string.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="defaultValue">The default value, or null.</param>
        /// <returns>The option, for fluid extension.</returns>
        public static ArgumentOption AcceptString(this ArgumentOption option, string? defaultValue = null)
        {
            option.AddProvider(new ArgumentStringProvider(defaultValue));
            return option;
        }
        
        /// <summary>
        /// Add a value provider that accepts an integer.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="defaultValue">The default value, or null.</param>
        /// <returns>The option, for fluid extension.</returns>
        public static ArgumentOption AcceptInteger(this ArgumentOption option, long? defaultValue = null)
        {
            option.AddProvider(new ArgumentLongProvider(defaultValue));
            return option;
        }

        /// <summary>
        /// Add a value provider that accepts a floating point number.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="defaultValue">The default value, or null.</param>
        /// <returns>The option, for fluid extension.</returns>
        public static ArgumentOption AcceptDecimal(this ArgumentOption option, decimal? defaultValue = null)
        {
            option.AddProvider(new ArgumentDecimalProvider(defaultValue));
            return option;
        }

        /// <summary>
        /// Add a value provider that accepts a flag only, or a flag with the values true or false.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>The option, for fluid extension.</returns>
        public static ArgumentOption AcceptFlag(this ArgumentOption option, bool? defaultValue = false)
        {
            option.AddProvider(new ArgumentFlagProvider(defaultValue));
            return option;
        }

        /// <summary>
        /// Add a value provider that accepts an enum value
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum to be used</typeparam>
        /// <param name="option">The option.</param>
        /// <param name="defaultValue">A default value, or null.</param>
        /// <returns>The option, for fluid extension.</returns>
        public static ArgumentOption AcceptEnum<TEnum>(this ArgumentOption option, object? defaultValue = null) where TEnum: Enum
        {
            option.AddProvider(new ArgumentEnumProvider<TEnum>(defaultValue));
            return option;
        }

    }
}
