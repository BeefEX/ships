using System;
using System.Net.Sockets;
using System.Text;
using Ships_Common;
using SocketLib;

namespace Ships_Client {
    
    internal class Program {
        private static Client client;

        public static Game game;
        
        public static void Main(string[] args) {
            game = new Game();
            Environment.Exit(game.Start("MainMenu"));
        }
    }
}