﻿namespace Ships_Client.States {
    
    public static class RoomState {

        public static bool connected = false;
        
        public static bool isHost = true;

        public static string roomID;
        public static string roomName;
        public static string roomPassword;

        public static bool won = false;
        public static bool finished = false;
    }
}