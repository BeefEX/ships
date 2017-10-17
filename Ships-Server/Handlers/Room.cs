using System;
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

        private static int ready;
        
        private static void receiveShipPlacement(Packet<string[]> packet) {
            List<Ship> ships = new List<Ship>();

            for (int i = 0; i < packet.data.Length; i++) {
                ships.Add(Ship.FromString(packet.data[i]));
            }
            
            foreach (Ship ship in ships) {
                if (packet.client.isHost)
                    packet.client.room.game.totalHitsToWinP2 += ship.shape.Length;
                else
                    packet.client.room.game.totalHitsToWinP1 += ship.shape.Length;
            }

            if (packet.client.isHost)
                packet.client.room.game.playerOneShips = ships;
            else
                packet.client.room.game.playerTwoShips = ships;
            
            byte[] _packet = PacketUtils.constructPacket(Packets.OPPONENT_READY.ToString());
            packet.client.getOpponent().send(_packet);
            ready++;
            if (ready == 2)
                packet.client.send(_packet);
        }

        private static void receiveHitInformation(Packet<string[]> packet) {
            if ((packet.client == packet.client.room.host && !packet.client.room.hostsTurn)
                ||
                (packet.client == packet.client.room.client && packet.client.room.hostsTurn)) {
                packet.client.send(PacketUtils.constructPacket(Packets.HIT_ANSWER.ToString(), new Hit(new Vector2(), false).ToString()));
                return;
            }
            
            Vector2 pos = Vector2.FromString(packet.data[0]);
            List<Ship> ships = packet.client.isHost
                ? packet.client.room.game.playerTwoShips
                : packet.client.room.game.playerOneShips;

            packet.client.room.hostsTurn = !packet.client.room.hostsTurn;
            
            bool success = false;
            
            foreach (Ship ship in ships) {
                if (ship.checkShape(pos)) {
                    success = true;
                    if (packet.client.isHost)
                        packet.client.room.game.playerOneHits++;
                    else
                        packet.client.room.game.playerTwoHits++;
                }
            }

            packet.client.send(PacketUtils.constructPacket(Packets.HIT_ANSWER.ToString(), new Hit(pos, success).ToString()));
            
            
            Console.WriteLine(packet.client.room.game.playerOneHits + "/" + packet.client.room.game.totalHitsToWinP1 + " -- " + packet.client.room.game.playerTwoHits + "/" + packet.client.room.game.totalHitsToWinP2);
            
            if (packet.client.room.game.playerOneHits >= packet.client.room.game.totalHitsToWinP1) {
                
                packet.client.room.host.send(PacketUtils.constructPacket(Packets.YOU_WON.ToString()));
                packet.client.room.client.send(PacketUtils.constructPacket(Packets.YOU_LOST.ToString()));
                
            } else if (packet.client.room.game.playerTwoHits >= packet.client.room.game.totalHitsToWinP2) {
                
                packet.client.room.host.send(PacketUtils.constructPacket(Packets.YOU_LOST.ToString()));
                packet.client.room.client.send(PacketUtils.constructPacket(Packets.YOU_WON.ToString()));
            }
        }

        public static void Init() {
            Program.eventSystem.addTrigger(new EventSystem<Packet<string[]>>.Handler(Packets.SUBMIT_SHIP_POSITIONS.ToString(), receiveShipPlacement));
            Program.eventSystem.addTrigger(new EventSystem<Packet<string[]>>.Handler(Packets.SHIP_LIST.ToString(), sendShipList));
            Program.eventSystem.addTrigger(new EventSystem<Packet<string[]>>.Handler(Packets.SUBMIT_HIT.ToString(), receiveHitInformation));
        }
    }
}