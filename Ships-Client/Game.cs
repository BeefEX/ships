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
            this.scenes = new Dictionary<string, Scene>();
            this.init();
        }

        protected void init() {
            this.addScene(new Scene("MainMenu", new MainMenuScript()));
            this.addScene(new Scene("RoomSelectionScene", new RoomSelectionSceneScript()));
            this.addScene(new Scene("RoomCreationScene", new RoomCreationSceneScript()));
            this.addScene(new Scene("RoomWaitingScene", new RoomWaitingSceneScript()));
            this.addScene(new Scene("RoomLoginScene", new RoomLoginSceneScript()));
            this.addScene(new Scene("RoomListScene", new RoomListSceneScript()));
            this.addScene(new Scene("GameScene", new GameSceneScript()));
            this.addScene(new Scene("ExitScene", new ExitSceneScript()));
        }

        public void addScene(Scene scene) {
            this.scenes.Add(scene.name, scene);
        }

        public void SwitchScene(string scene) {
            if (!this.scenes.ContainsKey(scene))
                return;
            this.activeScene = scene;
            this.scenes[scene].script.Start();
        }

        public void Exit(int exitCode) {
            this.shouldExit = true;
            this.exitCode = exitCode;
        }

        public int Start(string scene) {
            this.SwitchScene(scene);
            while (!shouldExit) {
                this.scenes[this.activeScene].script.Update();
                
                if (Console.KeyAvailable)
                    this.scenes[this.activeScene].script.KeyPressed(Console.ReadKey());
                
                Thread.Sleep(1000/20);
            }
            return exitCode;
        }
        
    }
}