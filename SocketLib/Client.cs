using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketLib {
    
    public class Client : IDisposable {

        public delegate void OnDisconnectHandler();

        public event OnDisconnectHandler OnDisconnect;
        
        public delegate void OnMessageHandler(string message);

        public event OnMessageHandler OnMessage;
        
        protected Socket socket;
        protected Task listenTask;
        protected bool running;

        public Client(Socket socket) {
            this.socket = socket;
            socket.ReceiveTimeout = 1000;
            running = true;
            listenTask = new Task(() => {
                try {
                    Byte[] received = new Byte[512];
                    string message = "";

                    while (running) {
                        if (socket.Available > 0) {
                            while (socket.Available > 0) {
                                int bytes = socket.Receive(received, received.Length, 0);
                                message += Encoding.ASCII.GetString(received, 0, bytes);
                            }

                            if (message.Length != 0) {
                                if (OnMessage != null) OnMessage(message);

                                message = "";
                                received = new Byte[256];
                            }
                        }

                        if (!socket.IsConnected()) {
                            running = false;
                        }
                    }
                    
                    if (OnDisconnect != null) OnDisconnect();
                } catch (Exception e) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    Console.ResetColor();
                    Environment.Exit(1);
                }
            });
            listenTask.Start();
        }

        public int send(Byte[] message) {
            return socket.Send(message);
        }

        public void close() {
            socket.Close();
        }


        public void Dispose() {
            close();
            socket.Dispose();
            OnMessage = null;
            OnDisconnect = null;
            
            //TODO: Add task disposal, uncomennting this right now will crash the game (both client and server);
            // listenTask.Dispose();
        }
    }
}