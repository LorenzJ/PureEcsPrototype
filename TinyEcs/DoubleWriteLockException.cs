using System;
using System.Runtime.Serialization;

namespace TinyEcs
{
    [Serializable]
    public class DoubleWriteLockException : Exception
    {
        public DoubleWriteLockException()
        {
        }

        public DoubleWriteLockException(string message) : base(message)
        {
        }

        public DoubleWriteLockException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DoubleWriteLockException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}