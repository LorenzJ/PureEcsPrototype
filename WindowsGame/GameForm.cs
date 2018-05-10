using Game;
using Game.Dependencies;
using GameGl;
using OpenGL;
using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using TinyEcs;

namespace WindowsGame
{
    public partial class GameForm : Form
    {
        private Game.Game game;
        private double previousTime;
        private double currentTime;
        private double accumulatedTime;
        private double timeStep = 1 / 120.0;
        private Renderer renderer;
        private KeyBinds keyBinds;
        private Vector2[] directions = new Vector2[8];
        private Stopwatch stopwatch;
        private int ticks;

        private Entity? selectedEntity;

        public GameForm()
        {
            InitializeComponent();
        }

        private void GlControl1_ContextCreated(object sender, GlControlEventArgs e)
        {
            game = new Game.Game();
            game.World.DebugEvents.EntityAdded += DebugEvents_EntityAdded;
            game.World.DebugEvents.EntityRemoved += DebugEvents_EntityRemoved;

            game.Init();
            renderer = game.World.GetDependency<Renderer>();
            keyBinds = GetKeyBindings();
            stopwatch = new Stopwatch();

            timescaleInput.ValidatingType = typeof(float);
            timestepInput.ValidatingType = typeof(float);
            Gl.Enable(EnableCap.Blend);
            Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            previousTime = stopwatch.Elapsed.TotalSeconds;
        }

        private void DebugEvents_EntityRemoved(object sender, Entity e)
        {
            entityList.Items.Remove(e);
        }

        private void DebugEvents_EntityAdded(object sender, Entity e)
        {
            entityList.Items.Add(e);
        }

        private void DoLogicUpdate()
        {
            currentTime = stopwatch.Elapsed.TotalSeconds;
            var deltaTime = currentTime - previousTime;
            previousTime = currentTime;
            accumulatedTime += deltaTime;
            var inputCommands = keyBinds.InputCommands;
            game.World.Post(new InputMessage(inputCommands, directions));
            while (accumulatedTime > 0)
            {
                accumulatedTime -= timeStep;
                game.Update((float)timeStep);
            }
            frametimeLabel.Text = $"{deltaTime:0.0000}s/frame";
            framerateLabel.Text = $"{1.0 / deltaTime:000.00}fps";
        }

        private KeyBinds GetKeyBindings()
        {
            var keyBinds = new KeyBinds();
            keyBinds.Bind(Keys.Left, 0, InputCommands.MoveLeft);
            keyBinds.Bind(Keys.Up, 0, InputCommands.MoveUp);
            keyBinds.Bind(Keys.Right, 0, InputCommands.MoveRight);
            keyBinds.Bind(Keys.Down, 0, InputCommands.MoveDown);
            keyBinds.Bind(Keys.Z, 0, InputCommands.Fire);
            return keyBinds;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
        }

        private void GlControl_Render(object sender, GlControlEventArgs e)
        {
            DoLogicUpdate();
            renderer.Render(game.Time);

            ticks++;
            if (ticks >= 50 && !splitContainer.Panel2Collapsed)
            {
                ticks = 0;
                foreach (var control in tableLayoutPanel.Controls)
                {
                    (control as Control).Refresh();
                }
                UpdateFields();
                pauseResumeButton.Refresh();
                entityList.Refresh();
                componentList.Refresh();
                fieldList.Refresh();
            }
            glControl.Invalidate();
        }

        private void GlControl_Resize(object sender, EventArgs e)
        {
            Gl.Viewport(0, 0, glControl.Width, glControl.Height);
        }

        private void GlControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                splitContainer.Panel2Collapsed = !splitContainer.Panel2Collapsed;
            }
            keyBinds.HandleKeyDown(e.KeyCode);
        }

        private void GlControl_KeyUp(object sender, KeyEventArgs e)
        {
            keyBinds.HandleKeyUp(e.KeyCode);
        }

        private void GlControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }

        private void pauseResumeButton_Click(object sender, EventArgs e)
        {
            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
            }
            else
            {
                stopwatch.Start();
            }
        }

        private void EntityList_SelectedValueChanged(object sender, EventArgs e)
        {
            componentList.Items.Clear();
            if (entityList.SelectedItem == null)
            {
                selectedEntity = null;
                return;
            }
            selectedEntity = (Entity)entityList.SelectedItem;
            var entity = selectedEntity.Value;
            var archetype = game.World.GetArchetype(entity);
            var componentTypes = game.World.GetArchetypeTypes(archetype);
            componentList.Items.AddRange(componentTypes);
        }

        private void ComponentList_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateFields();
        }

        private void UpdateFields()
        {
            fieldList.Items.Clear();
            var componentType = componentList.SelectedItem as Type;
            if (componentType == null || componentType.GetInterfaces().Contains(typeof(ITag)))
            {
                return;
            }
            var component = game.World.Get(selectedEntity.Value, componentType);
            var fields = componentType.GetFields();
            foreach (var field in fields)
            {
                fieldList.Items.Add($"{field.Name}: {field.GetValue(component)}");
            }
        }
    }
}
