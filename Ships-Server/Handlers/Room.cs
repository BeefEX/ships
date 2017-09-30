﻿using Ships_Common;
using Ships_Common.Net;

namespace Ships_Server.Handlers {
    
    public static class Room {

        private static void receiveShipPlacement(Packet<string[]> packet) {
            
        }

        public static void Init() {
            Program.eventSystem.addTrigger(new EventSystem<Packet<string[]>>.Handler(Packets.SUBMIT_SHIP_POSITIONS.ToString(), receiveShipPlacement));
        }
    }
}