using System;
using System.Runtime.Serialization;

namespace TinyEcs
{
    [Serializable]
    internal class UnknownFieldsException : Exception
    {
        public UnknownFieldsException()
        {
        }

        public UnknownFieldsException(string message) : base(message)
        {
        }

        public UnknownFieldsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownFieldsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}