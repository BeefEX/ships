namespace Ships_Server.Handlers {
    
    public static class Room {

        private static void receiveShipPlacement(Packet<string[]> packet) {
            
        }

        public static void Init() {
            Program.eventSystem.addTrigger(new EventSystem<Packet<string[]>>.Handler("rcsp", receiveShipPlacement));
        }
    }
}