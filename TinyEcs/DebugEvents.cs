using System;
using System.Diagnostics;

namespace TinyEcs
{
    /// <summary>
    /// Helper class for debugging.
    /// </summary>
    public class DebugEvents
    {
        private readonly World world;

        internal DebugEvents(World world)
        {
            this.world = world;
        }

        /// <summary>
        /// Event called when an entity is added and the DEBUG symbol is defined.
        /// </summary>
        public event EventHandler<Entity> EntityAdded;
        /// <summary>
        /// Event called when an entity is removed and the DEBUG symbol is defined.
        /// </summary>
        public event EventHandler<Entity> EntityRemoved;

        [Conditional("DEBUG")]
        internal void RaiseEntityAdded(Entity entity)
        {
            EntityAdded?.Invoke(world, entity);
        }

        [Conditional("DEBUG")]
        internal void RaiseEntityRemoved(Entity entity)
        {
            EntityRemoved?.Invoke(world, entity);
        }

    }
}
