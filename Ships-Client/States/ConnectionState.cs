using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Ships_Common;
using SocketLib;

namespace Ships_Client.States {
    
    public static class ConnectionState {


        public static Client client { get; private set; }

        public static EventSystem<string[]> OnMessage;

        public static bool connected = false;

        public static void Init() {
            OnMessage = new EventSystem<string[]>();
            client = new Client(new TcpClient("localhost", 8080).Client);
            connected = true;
            Send(Encoding.ASCII.GetBytes("ig~00000000"));
            
            client.OnMessage += message => {
                string[] split = message.Split('~');
                OnMessage.Invoke(split[0], split.Skip(1).Take(split.Length - 1).ToArray());
            };

            client.OnDisconnect += () => {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;

                connected = false;
                
                string loadingString = "You have been disconnected from the server.";
                Console.Clear();
                Console.SetCursorPosition(Console.WindowWidth / 2 - loadingString.Length / 2, Console.WindowHeight / 2);
                Console.Write(loadingString);
                Environment.Exit(1);
            };
        }

        public static void Send(byte[] message) {
            if (client != null)
                client.send(message);
        }

        public static void Close() {
            if (client != null)
                client.close();
        }
    }
}