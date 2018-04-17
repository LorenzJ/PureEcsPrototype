using System;

namespace TinyEcs
{
    public interface ISystem
    {
        Type MessageType { get; }
        void Do(World world, Message message);
    }
}
