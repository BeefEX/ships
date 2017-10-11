using System.Collections.Generic;

namespace Ships_Server.Rooms {
    
    public class RoomManager {

        private int id;

        public readonly Dictionary<string, Room> rooms;

        public RoomManager() {
            rooms = new Dictionary<string, Room>();
            id = 0;
        }

        public void createRoom(string name, string password, Client host) {
            Room room = new Room(id, host, name, password);
            rooms.Add(id.ToString(), room);
            host.room = room;
            host.isHost = true;
            id++;
        }

        public Room findRoomByID(string ID) {
            if (!rooms.ContainsKey(ID))
                return null;

            return rooms[ID];
        }

        public string findOpenRoom() {
            foreach (KeyValuePair<string, Room> room in rooms) {
                if (room.Value.open)
                    return room.Key;
            }
            return "";
        }

        public Room[] getOpenRooms() {
            List<Room> openRooms = new List<Room>();
            
            foreach (KeyValuePair<string, Room> room in rooms) {
                if (room.Value.open)
                    openRooms.Add(room.Value);
            }
            
            return openRooms.ToArray();
        }
    }
    
}