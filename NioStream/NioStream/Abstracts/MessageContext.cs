using NioStream.Pipe;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NioStream.Abstracts
{
    public class MessageContext
    {
        public volatile byte[] bytes;
        public volatile Socket UdpSocket;
        public volatile string OperationId;
        public volatile string flag;
        public volatile ConcurrentDictionary<string, Object> props = new ConcurrentDictionary<string, Object>();

        public volatile bool UDP = false;
        public volatile IPEndPoint UDPRempteEndPoint;

        public volatile string ClientIP;
        public volatile string ClientPort;
        public volatile string mac="UNDEFINED";

        public volatile ClientContainer clientContainer;
    }
}
