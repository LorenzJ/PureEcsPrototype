using Game;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsGame
{
    class KeyBinds
    {
        private struct InputBinding
        {
            readonly int playerId;
            readonly InputCommands command;

            public InputBinding(int playerId, InputCommands command)
            {
                this.playerId = playerId;
                this.command = command;
            }

            public int PlayerId => playerId;
            public InputCommands Command => command;
        }
        private Dictionary<Keys, InputBinding> bindings = new Dictionary<Keys, InputBinding>();
        private InputCommands[] commands = new InputCommands[8];

        public InputCommands[] InputCommands => commands;

        public void Bind(Keys key, int playerId, InputCommands command)
        {
            bindings.Add(key, new InputBinding(playerId, command));
        }

        public void Unbind(Keys key)
        {
            bindings.Remove(key);
        }

        public void HandleKeyDown(Keys key)
        {
            if (bindings.TryGetValue(key, out var inputBinding))
            {
                commands[inputBinding.PlayerId] |= inputBinding.Command;
            }
        }

        public void HandleKeyUp(Keys key)
        {
            if (bindings.TryGetValue(key, out var inputBinding))
            {
                commands[inputBinding.PlayerId] &= ~inputBinding.Command;
            }
        }
    }
}
