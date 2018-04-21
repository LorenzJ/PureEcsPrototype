using System;
using System.Collections.Generic;

namespace TinyEcs
{
    internal class EntityManager
    {
        private uint id;
        private Queue<uint> openIds = new Queue<uint>();
        internal Entity[] entities;

        public EntityManager()
        {
            entities = new Entity[1024];
        }

        public int Count { get => (int)id; }
        public long LongCount { get => id; }

        public Entity CreateEntity()
        {
            var id = GetNewId();
            if (id >= entities.Length)
            {
                var newEntities = new Entity[entities.LongLength * 2];
                Array.Copy(entities, newEntities, Math.Min(entities.Length, newEntities.Length));
                entities = newEntities;
            }
            entities[id] = new Entity(id);
            return entities[id];
        }

        public void RemoveEntity(Entity e) => openIds.Enqueue(e.id);

        private uint GetNewId()
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
