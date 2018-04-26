using System;
using System.Runtime.Serialization;

namespace GameGl
{
    [Serializable]
    internal class ProgramLinkingException : Exception
    {
        public ProgramLinkingException()
        {
        }

        public ProgramLinkingException(string message) : base(message)
        {
        }

        public ProgramLinkingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProgramLinkingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}