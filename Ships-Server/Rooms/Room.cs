using SocketLib;

namespace Ships_Server.Rooms {
    
    public class Room {

        public readonly int id;
        public bool open { get; protected set; }

        public string name { get; protected set; }
        protected string password;
        
        protected Client host;
        protected Client client;

        public Room(int id, Client host, string name, string password) {
            open = true;
            this.id = id;
            this.host = host;
            this.name = name;
            this.password = password;
        }

        public bool Authenticate(string password) {
            return this.password == password;
        }

        public void addClient(Client client) {
            this.client = client;
            this.open = false;
        }
    }
}