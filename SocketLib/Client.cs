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
        
        public bool debugEnabled { get; protected set; }
        protected StreamWriter debugFile;

        public Client(Socket socket) {
            this.socket = socket;
            socket.ReceiveTimeout = 1000;
            running = true;
            listenTask = new Task(() => {
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

                            if (debugEnabled)
                                debugFile.WriteLine(DateTime.Now.ToShortTimeString() + " -- RECEIVED -- " + message);
                            
                            message = "";
                            received = new Byte[256];
                        }
                    }

                    if (!socket.IsConnected()) {
                        if (OnDisconnect != null) OnDisconnect();
                        running = false;
                    }
                }
            });
            listenTask.Start();
        }

        public void enableDebug() {
            debugEnabled = true;
            debugFile = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "/log.txt");
        }

        public void disableDebug() {
            debugEnabled = false;
            debugFile.Close();
        }

        public int send(Byte[] message) {
            if (debugEnabled)
                debugFile.WriteLine(DateTime.Now.ToLongTimeString() + " -- SENT -- " + Encoding.ASCII.GetString(message));
            return socket.Send(message);
        }

        public void close() {
            socket.Close();
            debugFile.Close();
        }


        public void Dispose() {
            close();
            socket.Dispose();
            listenTask.Dispose();
            debugFile.Close();
            OnMessage = null;
            OnDisconnect = null;
        }
    }
}