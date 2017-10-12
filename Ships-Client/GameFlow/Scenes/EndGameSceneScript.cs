using System;
using Ships_Client.States;

namespace Ships_Client.GameFlow.Scenes {
    
    public class EndGameSceneScript : IScript {

        private int counter;
        
        public void Start() {
            counter = 0;
            string loadingString = "Opponent connected";

            RoomState.connected = false;
            ConnectionState.Close();
            
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - loadingString.Length / 2, Console.WindowHeight / 2);
            Console.Write(loadingString);
        }

        public void Unload() { }

        public void Update() {
            counter++;
            if (counter > 60)
                Program.game.SwitchScene("MainMenu");
        }

        public void KeyPressed(ConsoleKeyInfo key) { }
    }
}