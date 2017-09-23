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
        protected bool running = false;
        
        public Client(Socket socket) {
            this.socket = socket;
            socket.ReceiveTimeout = 1000;
            this.running = true;
            this.listenTask = new Task(() => {
                Byte[] received = new Byte[256];
                int bytes = 0;
                string message = "";
                
                while (running) {
                    if (socket.Available > 0) {
                        bytes = socket.Receive(received, received.Length, 0);
                        message += Encoding.ASCII.GetString(received, 0, bytes);

                        if (message.Length != 0) {
                            if (OnMessage != null) OnMessage(message);

                            message = "";
                            received = new Byte[256];
                        }
                    }

                    if (!socket.IsConnected()) {
                        if (OnDisconnect != null) OnDisconnect();
                    }
                }
            });
            this.listenTask.Start();
        }

        public int send(Byte[] message) {
            if (message.Length < 11) {
                byte[] tmp = message;
                message = new byte[11];
                for (int i = 0; i < tmp.Length; i++) {
                    message[i] = tmp[i];
                }
            }
            return this.socket.Send(message);
        }

        public void close() {
            this.socket.Close();
        }


        public void Dispose() {
            this.close();
            this.socket.Dispose();
            this.listenTask.Dispose();
            this.OnMessage = null;
            this.OnDisconnect = null;
        }
    }
    
}