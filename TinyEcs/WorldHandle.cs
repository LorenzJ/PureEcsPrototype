using System;
using System.Collections.Generic;

namespace TinyEcs
{
    /// <summary>
    /// A lightweight <see cref="World"/> wrapper exposing only thread-safe operations.
    /// </summary>
    public struct WorldHandle
    {
        private World world;

        internal WorldHandle(World world)
        {
            this.world = world;
        }

        /// <summary>
        /// Read-only reference to a component owned by an entity
        /// </summary>
        /// <typeparam name="T">Type of component</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Read-only reference to component of type T</returns>
        public ref readonly T Ref<T>(Entity entity)
            where T : struct, IComponent=> ref world.Ref<T>(entity);

        /// <summary>
        /// Send a message
        /// </summary>
        /// <typeparam name="T">type of message</typeparam>
        /// <param name="message">message</param>
        /// <remarks>not yet implemented</remarks>
        public void Send<T>(T message)
            where T : IMessage => world.Send(message);

        /// <summary>
        /// Send messages
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messages"></param>
        public void Send<T>(IEnumerable<T> messages)
            where T : IMessage => world.Send(messages);

        /// <summary>
        /// An action to be executed after the current message has been handled.
        /// </summary>
        /// <param name="action">Action to execute</param>
        public void Post(Action action) => world.PostAction(action);

        /// <summary>
        /// Get a dependency
        /// </summary>
        /// <typeparam name="T">type of dependency</typeparam>
        /// <returns>dependency</returns>
        public T GetDependency<T>() where T : class => world.GetDependency<T>();

    }
}
