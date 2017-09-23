﻿using System.Net.Sockets;
using System.Text;
using SocketLib;

namespace Ships_Client.States {
    
    public static class ConnectionState {

        public static Client client { get; private set; }

        //TODO: Add disconnect handeling.
        
        public static void Init() {
            client = new Client(new TcpClient("localhost", 8080).Client);
            client.send(Encoding.ASCII.GetBytes("ig~00000000"));
        }

        public static void Close() {
            if (client != null)
                client.close();
        }
    }
}