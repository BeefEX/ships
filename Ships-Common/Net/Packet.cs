using System.Text;

namespace Ships_Common.Net {
    
    public class Packet {    
        
        public static string[] readPacket(byte[] packet) {
            return Encoding.ASCII.GetString(packet).Split('~');
        }
        
        public static byte[] constructPacket(string packetName, params string[] data) {
            return Encoding.ASCII.GetBytes(packetName + "~" + string.Join("~", data));
        }
    }
}