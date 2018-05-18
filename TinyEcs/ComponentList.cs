namespace TinyEcs
{
    internal class ComponentList<T>
        where T : struct, IComponent
    {
        private static FlatMap<Archetype, Array2> arrays;

        internal static void Create(Archetype archetype)
        {
            arrays[archetype] = new Array2(typeof(T), 1);
        }

        internal static T[] Get(Archetype archetype)
        {
            return (T[])arrays[archetype].Data;
        }

    }
}
