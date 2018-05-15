namespace GameGl
{
    public class RendererEx : IRenderable
    {
        private readonly RenderPass[] renderPasses;

        public RendererEx()
        {
            renderPasses = new RenderPass[]
            {
                new GameRenderPass()
            };
        }

        public void Render()
        {
            foreach (var pass in renderPasses)
            {
                pass.Render();
            }
        }
    }
}
