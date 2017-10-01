using System.Collections.Generic;
using Ships_Common;
using Ships_Common.Net;

namespace Ships_Server.Handlers {
    
    public static class Room {

        private static void sendShipList(Packet<string[]> packet) {
            List<string> ships = new List<string>();
            
            for (int i = 0; i < Ship.defaultInventory.Length; i++) {
                ships.Add(Ship.defaultShips[Ship.defaultInventory[i]].Instantiate(new Vector2()).ToString());
            }

            packet.client.send(PacketUtils.constructPacket(Packets.SHIP_LIST.ToString(), ships.ToArray()));
        }
        
        private static void receiveShipPlacement(Packet<string[]> packet) {
            
        }

        public static void Init() {
            Program.eventSystem.addTrigger(new EventSystem<Packet<string[]>>.Handler(Packets.SUBMIT_SHIP_POSITIONS.ToString(), receiveShipPlacement));
            Program.eventSystem.addTrigger(new EventSystem<Packet<string[]>>.Handler(Packets.SHIP_LIST.ToString(), sendShipList));
        }
    }
}