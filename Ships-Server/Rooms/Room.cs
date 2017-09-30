using Ships_Common.Net;
using SocketLib;

namespace Ships_Server.Rooms {
    
    public class Room {

        public readonly int id;
        public bool open { get; protected set; }

        public string name { get; protected set; }
        public string password { get; protected set; }

        protected Client host;
        protected Client client;
        protected Game game;

        public Room(int id, Client host, string name, string password) {
            open = true;
            this.id = id;
            this.host = host;
            this.name = name;
            this.password = password;

            host.OnDisconnect += OnHostDisconnect;
        }

        public bool Authenticate(string _password) {
            return password.Equals(_password);
        }

        public void addClient(Client _client) {
            client = _client;
            host.send(PacketUtils.constructPacket("jn-op", ""));
            _client.OnDisconnect += OnOpponentDisconnect;
            open = false;
        }

        public override string ToString() {
            return id + "$" + name + "$" + !password.Equals("");
        }

        private void OnHostDisconnect() {
            if (client != null)
                client.send(PacketUtils.constructPacket("dc-op", ""));
        }
        
        private void OnOpponentDisconnect() {
            host.send(PacketUtils.constructPacket("dc-op", ""));
        }
    }
}