using System;
using Ships_Common.Net;

namespace Ships_Server.Rooms {
    
    public class Room {

        public readonly int id;
        public bool open { get; protected set; }

        public string name { get; protected set; }
        public string password { get; protected set; }

        public Client host { get; protected set; }
        public Client client { get; protected set; }
        public Game game { get; protected set; }

        public bool hostsTurn = true;
        
        public Room(int id, Client host, string name, string password) {
            open = true;
            this.id = id;
            this.host = host;
            this.name = name;
            this.password = password;
            game = new Game(this);

            host.client.OnDisconnect += OnHostDisconnect;
        }

        public bool Authenticate(string _password) {
            return password.Equals(_password);
        }

        public void addClient(Client _client) {
            client = _client;
            
            host.send(PacketUtils.constructPacket(Packets.OPPONENT_JOINED.ToString(), ""));
            _client.client.OnDisconnect += OnOpponentDisconnect;
            
            open = false;
            
            _client.room = this;
            _client.isHost = false;
        }

        public override string ToString() {
            return id + "$" + name + "$" + !password.Equals("");
        }

        private void OnHostDisconnect() {
            client.client.OnDisconnect -= OnHostDisconnect;
            if (!client.client.isConnected())
                return;
            client.send(PacketUtils.constructPacket(Packets.OPPONENT_DISCONNECTED.ToString(), ""));
            Console.WriteLine("test - host");
            Program.rooms.removeRoom(this);
            host = null;
            client = null;
        }
        
        private void OnOpponentDisconnect() {
            Console.WriteLine("test - client");
            host.send(PacketUtils.constructPacket(Packets.OPPONENT_DISCONNECTED.ToString(), ""));
            host.client.OnDisconnect -= OnOpponentDisconnect;
            Program.rooms.removeRoom(this);
            host = null;
            client = null;
        }
    }
}