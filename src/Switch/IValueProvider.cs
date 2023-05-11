using System;

namespace Webefinity.Switch
{
    /// <summary>
    /// A value provider is stores and optionally validates the result of a switch when the value is parsed.
    /// 
    /// Set is called if an option is matched with the value passed to that option.
    /// The value can be converted into the target type by the provider, and stored as a nullable object.
    /// Later, GetValue can be called on the handler to retrieve the typed value of the option as passed.
    /// 
    /// You may implement your own value provider and add it to an option and apply it to an option during the build phase using:
    /// build.Add("switch").AddProvider(new MyCustomProvider());
    /// </summary>
    public interface IValueProvider
    {
        /// <summary>
        /// Called by the ArgumentsHandler to set the value of the option in the provider.
        /// Should set WasSet to true if the value was used, regardless of its validity.
        /// </summary>
        /// <param name="value">The value of the option as provided on the command line.</param>
        /// <returns>
        /// An validation result, either success, or with errors.
        /// If errors are returned the entire command line is considered invalid by the handler.
        /// </returns>
        ValidationResult Set(string? value);

        /// <summary>
        /// The value of the option, stored as a boxed value according to the type the handler should produce.
        /// </summary>
        object? Value { get; }

        /// <summary>
        /// Indicates whether the value was set by command line processing.
        /// This should be set by the call to the Set method.
        /// </summary>
        bool WasSet { get; }

        /// <summary>
        /// The default value if there is one.
        /// If a switch is followed only by another switch, the default value will be used by the handler.
        /// </summary>
        object? DefaultValue { get; }

        /// <summary>
        /// Returns a string for the usage description that describes show to set the value.
        /// A true/false flag might return "true, false" here.
        /// </summary>
        /// <param name="log">
        /// The action to call when returning the value.
        /// This action is expected to be a call to Console.Write.
        /// </param>
        void Usage(Action<string> log);
    }
}
