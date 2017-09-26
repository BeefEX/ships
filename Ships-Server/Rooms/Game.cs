using System.Collections.Generic;
using Ships_Common;

namespace Ships_Server.Rooms {
    
    public class Game {

        private Room room;
        private List<Ship> playerOneShips;
        private List<Ship> playerTwoShips;

        public Game(Room room) {
            this.room = room;
            playerOneShips = new List<Ship>();
            playerTwoShips = new List<Ship>();
        }
    }
}