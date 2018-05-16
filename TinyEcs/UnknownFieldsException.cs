using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace TinyEcs
{
    /// <summary>
    /// Thrown when a component <see cref="GroupAttribute">group</see> defined in a <see cref="ComponentSystem{T}"/> contains
    /// types that aren't components or tags
    /// </summary>
    [Serializable]
    public class UnknownFieldsException : Exception
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unknownFields">Fields with an unknown type</param>
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

        /// <inheritdoc/>
        protected UnknownFieldsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}