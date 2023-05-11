using System;
using System.Runtime.Serialization;

namespace Webefinity.Switch
{
    /// <summary>
    /// An exception thrown in the event that an ArgumentOption has no IValueProvider during parsing of the arguments.
    /// </summary>
    [Serializable]
    internal class ValueProviderException : Exception
    {
        public ValueProviderException()
        {
        }

        public ValueProviderException(string message) : base(message)
        {
        }

        public ValueProviderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ValueProviderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}