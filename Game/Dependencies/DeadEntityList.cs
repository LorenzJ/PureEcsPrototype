using System.Collections.Generic;
using TinyEcs;

namespace Game.Dependencies
{
    public class DeadEntityList
    {
        private List<Entity> entities = new List<Entity>();

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

        public void Commit(World world)
        {
            foreach (var entity in entities)
            {
                world.DestroyEntity(entity);
            }
            entities.Clear();
        }
    }
}
