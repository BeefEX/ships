using System;

namespace Ships_Client {
    
    public struct ColorScheme {

        public readonly ConsoleColor background;
        public readonly ConsoleColor foreground;
        
        public readonly ConsoleColor backgroundSelected;
        public readonly ConsoleColor foregroundSelected;

        public ColorScheme(
            ConsoleColor background = ConsoleColor.Black,
            ConsoleColor foreground = ConsoleColor.White,
            ConsoleColor backgroundSelected = ConsoleColor.White,
            ConsoleColor foregroundSelected = ConsoleColor.Black) {
            
            this.background = background;
            this.foreground = foreground;
            this.backgroundSelected = backgroundSelected;
            this.foregroundSelected = foregroundSelected;
        }
    }
}