using System.Linq;
using System.Net.Sockets;
using System.Text;
using Ships_Common;
using SocketLib;

namespace Ships_Client.States {
    
    public struct Packet<T> {
        public readonly T data;

        public Packet(T data) {
            this.data = data;
        }
    }
    
    public static class ConnectionState {


        public static Client client { get; private set; }

        public static EventSystem<Packet<string[]>> OnMessage;
        
        //TODO: Add disconnect handeling.

        public static void Init() {
            OnMessage = new EventSystem<Packet<string[]>>();
            client = new Client(new TcpClient("localhost", 8080).Client);
            client.send(Encoding.ASCII.GetBytes("ig~00000000"));
            
            client.OnMessage += message => {
                string[] split = message.Split('~');
                OnMessage.Invoke(split[0], new Packet<string[]>(split.Skip(1).Take(split.Length - 1).ToArray()));
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