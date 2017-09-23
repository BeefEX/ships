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
            
        	Console.Clear();
            Ship ship = Ship.SHIP_PLANE.Instantiate(new Vector2(10, 10));
            ship.isHit(new Vector2(10, 10));
            Renderer.renderShip(ship);
            
            client = new Client(new TcpClient("localhost", 8080).Client);
            
            client.OnMessage += message => Console.WriteLine("Received => " + message);
            
            client.OnDisconnect += () => {
                Console.WriteLine("Server closed.");
                client.close();
                Environment.Exit(1);
            };
            
            string input;
            while ((input = Console.ReadLine()) != "quit") {
                client.send(Encoding.ASCII.GetBytes(input));
            }
        }
    }
}