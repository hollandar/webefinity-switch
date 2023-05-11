using System;
using System.Runtime.Serialization;

namespace Webefinity.Switch
{
    /// <summary>
    /// An exception thrown in the event that multiple default options are added during options building.
    /// </summary>
    [Serializable]
    internal class MultipleDefaultsException : Exception
    {
        public MultipleDefaultsException()
        {
        }

        public MultipleDefaultsException(string message) : base(message)
        {
        }

        public MultipleDefaultsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MultipleDefaultsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}