using System;
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
        }

        public bool Authenticate(string password) {
            return this.password.Equals(password);
        }

        public void addClient(Client client) {
            this.client = client;
            this.host.send(Packet.constructPacket("jn-op", ""));
            this.open = false;
        }

        public override string ToString() {
            return id + "$" + name + "$" + !password.Equals("");
        }
    }
}