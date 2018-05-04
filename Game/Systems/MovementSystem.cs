using TinyEcs;
using System.Threading.Tasks;
using Game.Components.Transform;
using System.Collections.Concurrent;
using Game.Components;

namespace Game.Systems
{
    public class MovementSystem : ComponentSystem<UpdateMessage>
    {

        public class Data
        {
            public int Length;
            public RwDataStream<Position> Positions;
            public RoDataStream<Heading> Headings;
            [Exclude] public ParticleTag ExcludeParticleTag;
        }
        [Group] public Data data;

        public class Particles
        {
            public int Length;
            public RwDataStream<Position> Positions;
            public RoDataStream<Heading> Headings;
            public ParticleTag ParticleTag;
        }
        [Group] public Particles particles;

        protected override void Execute(World world, UpdateMessage message)
        {
            var moveNonParticles = Task.Run(() =>
            {
                for (int i = 0; i < data.Length; i++)
                {
                    data.Positions[i].Vector += data.Headings[i].Vector * message.DeltaTime;
                }
            });
            var moveParticles = Task.Run(() =>
            {
                if (particles.Length == 0)
                {
                    return;
                }
                var partitioner = Partitioner.Create(0, particles.Length);
                Parallel.ForEach(partitioner, partition =>
                {
                    for (var i = partition.Item1; i < partition.Item2; i++)
                    {
                        particles.Positions[i].Vector += particles.Headings[i].Vector * message.DeltaTime;
                    }
                });
            });
            Task.WaitAll(moveNonParticles, moveParticles);
        }
    }
}
