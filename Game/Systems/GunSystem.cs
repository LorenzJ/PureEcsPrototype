using System;
using TinyEcs;

namespace Game.Systems
{
    class GunSystem : ISystem
    {
        public Type MessageType => typeof(UpdateMessage);

        public class Data
        {
            [Length] public int length;
            
        }
        [InjectComponents] public Data data = new Data();

        public void Do(World world, Message message)
        {
            
        }
    }
}
