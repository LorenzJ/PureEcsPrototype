using Game.Components.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TinyEcs;

namespace Game.Systems
{
    class TtlSystem : ISystem
    {
        public Type MessageType => typeof(UpdateMessage);

        public class Components
        {
            [Length] public int length;
            [Write] public Ttl[] ttls;
            [Entities] public Entity[] entities;
        }
        [InjectComponents] public Components components = new Components();

        public void Do(World world, Message message)
        {
            var deltaTime = (message as UpdateMessage).DeltaTime;
            var ttls = components.ttls;
            var handles = components.entities;

            Parallel.For(0, components.length, i =>
            {
                ttls[i].value -= deltaTime;
                if (ttls[i].value < 0)
                {
                    world.QueueForRemoval(handles[i]);
                }
            });


        }
    }


}
