using System;
using System.Runtime.Serialization;

namespace GameGl.Core.Shaders
{
    [Serializable]
    internal class UniformNotFoundException : Exception
    {
        public UniformNotFoundException()
        {
        }

        public UniformNotFoundException(string message) : base(message)
        {
        }

        public UniformNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UniformNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}