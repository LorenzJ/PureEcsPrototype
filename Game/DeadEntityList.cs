using System.Collections.Generic;
using TinyEcs;

namespace Game
{
    public class DeadEntityList : Resource
    {
        private List<Entity> entities = new List<Entity>();
        private World world;

        protected override void OnLoad(World world)
        {
            this.world = world;
        }

        public void Add(Entity entity)
        {
            lock (entities)
            {
                entities.Add(entity);
            }
        }

        public void AddRange(IEnumerable<Entity> entities)
        {
            lock (entities)
            {
                this.entities.AddRange(entities);
            }
        }

        protected override void Flush(IMessage message)
        {
            foreach (var entity in entities)
            {
                world.DestroyEntity(entity);
            }
            entities.Clear();
        }
    }
}
