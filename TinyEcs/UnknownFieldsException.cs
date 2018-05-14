using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace TinyEcs
{
    [Serializable]
    internal class UnknownFieldsException : Exception
    {
        private IEnumerable<FieldInfo> unknownFields;

        public UnknownFieldsException()
        {
        }

        public UnknownFieldsException(string message) : base(message)
        {
        }

        public UnknownFieldsException(IEnumerable<FieldInfo> unknownFields)
            : base (CreateMessage(unknownFields))
        {
        }

        private static string CreateMessage(IEnumerable<FieldInfo> unknownFields)
        {
            var sb = new StringBuilder();
            foreach (var field in unknownFields)
            {
                sb.AppendFormat("Unknown field '{0}' of type '{1}'\n", field.Name, field.FieldType.Name);
            }
            return sb.ToString();
        }

        public UnknownFieldsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownFieldsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}