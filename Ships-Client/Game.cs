using System;
using System.Collections.Generic;
using System.Threading;
using Ships_Client.GameFlow.Scenes;

namespace Ships_Client {
    
    public class Game {

        public string activeScene { get; protected set; }

        protected bool shouldExit;
        protected int exitCode;
        
        public Dictionary<string, Scene> scenes;
        
        public Game() {
            scenes = new Dictionary<string, Scene>();
            init();
        }

        protected void init() {
            addScene(new Scene("MainMenu", new MainMenuScript()));
            addScene(new Scene("RoomSelectionScene", new RoomSelectionSceneScript()));
            addScene(new Scene("RoomCreationScene", new RoomCreationSceneScript()));
            addScene(new Scene("RoomWaitingScene", new RoomWaitingSceneScript()));
            addScene(new Scene("RoomLoginScene", new RoomLoginSceneScript()));
            addScene(new Scene("RoomListScene", new RoomListSceneScript()));
            addScene(new Scene("GameScene", new GameSceneScript()));
            addScene(new Scene("ExitScene", new ExitSceneScript()));
        }

        public void addScene(Scene scene) {
            scenes.Add(scene.name, scene);
        }

        public void SwitchScene(string scene) {
            scenes[activeScene].script.Unload();
            if (!scenes.ContainsKey(scene))
                return;
            activeScene = scene;
            scenes[scene].script.Start();
        }

        public void Exit(int _exitCode) {
            shouldExit = true;
            exitCode = _exitCode;
        }

        public int Start(string scene) {
            SwitchScene(scene);
            while (!shouldExit) {
                scenes[activeScene].script.Update();
                
                if (Console.KeyAvailable)
                    scenes[activeScene].script.KeyPressed(Console.ReadKey());
                
                Thread.Sleep(1000/60);
            }
            return exitCode;
        }
        
    }
}