using Ships_Common;
using Ships_Common.Net;

namespace Ships_Server.Handlers {
    
    public static class Server {

        private static void createRoom(Packet<string[]> packet) {
            packet.roomManager.createRoom(packet.data[0], packet.data[1], packet.client);
        }

        private static void listRooms(Packet<string[]> packet) {            
            Rooms.Room[] roomArray = packet.roomManager.getOpenRooms();
            
            string[] roomStrings = new string[roomArray.Length];
            for (int i = 0; i < roomStrings.Length; i++) {
                roomStrings[i] = roomArray[i].ToString();
            }
            
            packet.client.send(PacketUtils.constructPacket(Packets.ROOM_LIST.ToString(), roomStrings));
        }

        private static void joinRoom(Packet<string[]> packet) {
            bool status = true;
            Rooms.Room room = packet.roomManager.findRoomByID(packet.data[0]);
                            
            if (!room.open)
                status = false;
            else if (room.Authenticate(packet.data[1])) {
                room.addClient(packet.client);
            } else
                status = false;
                            
            packet.client.send(PacketUtils.constructPacket(Packets.JOIN_ROOM.ToString(), status.ToString()));
        }
        
        public static void Init () {
            Program.eventSystem.addTrigger(new EventSystem<Packet<string[]>>.Handler(Packets.CREATE_ROOM.ToString(), createRoom));
            Program.eventSystem.addTrigger(new EventSystem<Packet<string[]>>.Handler(Packets.ROOM_LIST.ToString(), listRooms));
            Program.eventSystem.addTrigger(new EventSystem<Packet<string[]>>.Handler(Packets.JOIN_ROOM.ToString(), joinRoom));
        }
    }
}