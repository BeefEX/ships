namespace Ships_Server.Rooms {
    
    public class Client {

        public readonly SocketLib.Client client;
        public bool isHost;
        public Room room;

        public Client(SocketLib.Client client, Room room, bool isHost) {
            this.client = client;
            this.room = room;
            this.isHost = isHost;
        }

        public int send(byte[] message) {
            return client.send(message);
        }

        public Client getOpponent() {
            return isHost ? room.client : room.host;
        }
    }
}