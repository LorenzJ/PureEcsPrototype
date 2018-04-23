using Game;
using Game.Components;
using Game.Components.Transform;
using TinyEcs;

namespace WindowsGame.Systems
{
    public class ParticleRenderingSystem : ComponentSystem<UpdateMessage>
    {
        public class Data
        {
            public int length;
            public RoArray<Position> positions;
            public RoArray<ParticleTag> tag;
        }
        [Group] public Data data;
        [Resource] public Renderer renderer;

        protected override void Execute(World world, UpdateMessage message)
        {
            
            
        }
    }
}
