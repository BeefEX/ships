using System.Text;

namespace Ships_Common.Net {
    
    public class PacketUtils {    
        
        public static string[] readPacket(byte[] packet) {
            return Encoding.ASCII.GetString(packet).Split('~');
        }
        
        public static byte[] constructPacket(string packetName, params string[] data) {
            return Encoding.ASCII.GetBytes(packetName + "~" + string.Join("~", data));
        }
    }

    public class PacketHandler : EventSystem<string[]>.Handler {
        public PacketHandler(Packets packet, EventSystem<string[]>.OnTrigger handler) :
            base(packet.ToString(), handler) { }
    }
}