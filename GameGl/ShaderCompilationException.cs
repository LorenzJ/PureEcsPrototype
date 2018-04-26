using System;
using System.Runtime.Serialization;

namespace GameGl
{
    [Serializable]
    internal class ShaderCompilationException : Exception
    {
        public ShaderCompilationException()
        {
        }

        public ShaderCompilationException(string message) : base(message)
        {
        }

        public ShaderCompilationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ShaderCompilationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}