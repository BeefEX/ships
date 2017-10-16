﻿using System;
using System.Collections.Generic;
using Ships_Client.Rendering;
using Ships_Client.States;
using Ships_Common;
using Ships_Common.Net;

namespace Ships_Client.GameFlow.Scenes {
    public class GameSceneScript : IScript {

        private bool waiting;
        private bool shouldRender;
        private Loader loader;
        
        private Vector2 cursor;
        
        public void Start() {
            ConnectionState.OnMessage.addTrigger(new PacketHandler(Packets.OPPONENT_READY, OnOpponentJoin));
            ConnectionState.OnMessage.addTrigger(new PacketHandler(Packets.HIT_ANSWER, OnHitProcessed));
            ConnectionState.OnMessage.addTrigger(new PacketHandler(Packets.YOU_WON, OnWin));
            ConnectionState.OnMessage.addTrigger(new PacketHandler(Packets.YOU_LOST, OnLoss));
            
            cursor = new Vector2(5, 5);
            loader = new Loader(new Vector2(Console.WindowWidth / 2f, Console.WindowHeight / 2f + 4));
            
            List<string> ships = new List<string>();
            
            for (int i = 0; i < GameState.yourShips.Count; i++) {
                ships.Add(GameState.yourShips[i].ToString());
            }
            
            string loadingString = "Waiting for the opponent to place ships";
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - loadingString.Length / 2, Console.WindowHeight / 2);
            Console.Write(loadingString);
            
            waiting = true;
            shouldRender = true;
            ConnectionState.Send(PacketUtils.constructPacket(Packets.SUBMIT_SHIP_POSITIONS.ToString(), ships.ToArray()));
        }

        private void RenderWait() {
            loader.Render();
        }

        private void RenderGame() {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            
            for (int i = 0; i < 10; i++) {
                Console.SetCursorPosition(i + 1, 0);
                Console.Write(i);
                Console.SetCursorPosition(0, i + 1);
                Console.Write((char) (65 + i));
            }

            for (int i = 0; i < GameState.yourShips.Count; i++) {
                Console.ForegroundColor = ConsoleColor.White;
                
                Renderer.renderShip(GameState.yourShips[i]);
            }
            
            for (int i = 0; i < 10; i++) {
                Console.SetCursorPosition(i + 1 + 20, 0);
                Console.Write(i);
                Console.SetCursorPosition(20, i + 1);
                Console.Write((char) (65 + i));
            }
            
            foreach (Hit hit in GameState.yourHits) {
                Console.ForegroundColor = hit.succesful ? ConsoleColor.Red : ConsoleColor.Blue;
                
                Console.SetCursorPosition((int) hit.location.x + 20, (int) hit.location.y);
                Console.Write('X');
            }
            
            Console.SetCursorPosition((int) cursor.x + 20, (int) cursor.y);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write('O');
        }
        
        public void Unload() {
            shouldRender = true;
        }

        public void Update() {
            if (waiting)
                RenderWait();
            else {
                if (!shouldRender)
                    return;
                shouldRender = false;
                
                RenderGame();
            }
        }

        public void KeyPressed(ConsoleKeyInfo key) {
            shouldRender = true;

            Vector2 delta = new Vector2();

            if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                delta.y += 1;
            if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                delta.y -= 1;
            if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                delta.x += 1;
            if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A)
                delta.x -= 1;
            if (key.Key == ConsoleKey.Enter) {
                ConnectionState.Send(PacketUtils.constructPacket(Packets.SUBMIT_HIT.ToString(), cursor.ToString()));
            }
            
            Vector2 tmp = cursor + delta;
                
            if (tmp.x < 1 || tmp.x > 10 || tmp.y < 1 || tmp.y > 10)
                delta = new Vector2();

            cursor += delta;
        }

        private void OnOpponentJoin(string[] message) {
            waiting = false;
            shouldRender = true;
        }

        private void OnHitProcessed(string[] message) {
            GameState.yourHits.Add(Hit.FromString(message[0]));
        }

        private void OnWin(string[] message) {
            RoomState.won = true;
            Program.game.SwitchScene("EndGameScene");
        }

        private void OnLoss(string[] message) {
            RoomState.won = false;
            Program.game.SwitchScene("EndGameScene");
        }
    }
}