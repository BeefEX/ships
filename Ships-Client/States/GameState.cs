using System.Collections.Generic;
using Ships_Common;

namespace Ships_Client.States {
    
    public static class GameState {
        
        public static List<Ship> yourShips = new List<Ship>();
        public static List<Hit> yourHits = new List<Hit>();
        public static List<Hit> opponentsHits = new List<Hit>();
    }
}