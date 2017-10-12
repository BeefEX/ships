namespace Ships_Common.Net {
    public enum Packets {
        CREATE_ROOM,
        JOIN_ROOM,
        OPPONENT_JOINED,
        OPPONENT_DISCONNECTED,
        OPPONENT_READY,
        ROOM_LIST,
        
        SHIP_LIST,
        SUBMIT_SHIP_POSITIONS,
        
        SUBMIT_HIT,
        HIT_ANSWER,
        OPPONENT_HIT,
        
        YOU_WON,
        YOU_LOST
        
    }
}