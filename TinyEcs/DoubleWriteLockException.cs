using System;
using System.Runtime.Serialization;

namespace TinyEcs
{
    /// <summary>
    /// An exception that is thrown when two different systems are attempting to write to the same entity.
    /// </summary>
    [Serializable]
    public class DoubleWriteLockException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public DoubleWriteLockException()
        {
        }

        /// <summary>
        /// New DoubleWriteLockException.
        /// </summary>
        /// <param name="message">Custom message.</param>
        public DoubleWriteLockException(string message) : base(message)
        {
        }

        /// <summary>
        /// New DoubleWriteLockException.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public DoubleWriteLockException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// New DoubleWriteLockException.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected DoubleWriteLockException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}