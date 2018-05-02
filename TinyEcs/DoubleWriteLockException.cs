using System;
using System.Runtime.Serialization;

namespace TinyEcs
{
    /// <summary>
    /// This exception is thrown when two different systems are attempting to write to the same entity.
    /// </summary>
    [Serializable]
    public class DoubleWriteLockException : Exception
    {
        /// <inheritdoc/>
        public DoubleWriteLockException()
        {
        }

        /// <inheritdoc/>
        public DoubleWriteLockException(string message) : base(message)
        {
        }

        /// <inheritdoc/>
        public DoubleWriteLockException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <inheritdoc/>
        protected DoubleWriteLockException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}