using System;
using System.Collections.Generic;

namespace TinyEcs
{
    internal class EntityManager
    {
        private ushort id;
        private ushort generation;
        private Queue<ushort> openIds = new Queue<ushort>();
        internal Entity[] entities;

        public EntityManager()
        {
            entities = new Entity[1024];
        }

        public int Count { get => id; }

        public Entity CreateEntity()
        {
            var id = GetNewId();
            if (id >= entities.Length)
            {
                var newEntities = new Entity[entities.Length * 2];
                Array.Copy(entities, newEntities, Math.Min(entities.Length, newEntities.Length));
                entities = newEntities;
            }
            entities[id] = new Entity(id, generation++);
            return entities[id];
        }

        public void RemoveEntity(Entity e) => openIds.Enqueue(e.id);

        private ushort GetNewId()
        {
            if (openIds.Count > 0)
            {
                return openIds.Dequeue();
            }
            else
            {
                return id++;
            }
        }
    }
}
