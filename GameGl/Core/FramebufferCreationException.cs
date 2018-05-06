using System;
using System.Runtime.Serialization;

namespace GameGl.Core
{
    [Serializable]
    internal class FramebufferCreationException : Exception
    {
        public FramebufferCreationException()
        {
        }

        public FramebufferCreationException(string message) : base(message)
        {
        }

        public FramebufferCreationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FramebufferCreationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}