using System.Collections.Generic;
using SocketLib;

namespace Ships_Server.Rooms {
    
    public class RoomManager {

        private int id;

        public readonly List<Room> rooms;

        public RoomManager() {
            this.rooms = new List<Room>();
            id = 0;
        }

        public void createRoom(string name, string password, Client host) {
            rooms.Add(new Room(id, host, name, password));
            id++;
        }

        public int findOpenRoom() {
            foreach (Room room in rooms) {
                if (room.open)
                    return room.id;
            }
            return -1;
        }

        public Room[] getOpenRooms() {
            List<Room> openRooms = new List<Room>();
            
            foreach (Room room in rooms) {
                if (room.open)
                    openRooms.Add(room);
            }
            
            return openRooms.ToArray();
        }
    }
    
}