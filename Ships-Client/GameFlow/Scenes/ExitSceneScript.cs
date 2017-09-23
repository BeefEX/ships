using System;

namespace Ships_Client.GameFlow.Scenes {
    public class ExitSceneScript : IScript {
        
        public void Start() {
            Program.game.Exit(0);
        }

        public void Unload() { }

        public void Update() { }

        public void KeyPressed(ConsoleKeyInfo key) { }
    }
}