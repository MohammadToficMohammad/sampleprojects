
using NioStream.Pipe;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace NioStream.Abstracts
{
    public class PresistentClientContext
    {
        public volatile AbstractHandler writerHandler;
        public int test = 0;
        public DateTime lastActivity = DateTime.Now;
        public DateTime lastInitActivity = DateTime.MinValue;
        public volatile ConcurrentBag<IDisposable> PermanentDisposeList = new ConcurrentBag<IDisposable>();
        public volatile ConcurrentBag<IDisposable> HalfPermanentDisposeList = new ConcurrentBag<IDisposable>();
        public volatile Socket socket;
        public volatile Stream stream;
        public readonly ConcurrentQueue<InternalMessageContainer> internalMessagesFirstQueue = new ConcurrentQueue<InternalMessageContainer>();
        public readonly ConcurrentQueue<PresistentClientContext> subscribers = new ConcurrentQueue<PresistentClientContext>();
        public readonly ConcurrentDictionary<string, Object> props = new ConcurrentDictionary<string, Object>();
        public volatile IPEndPoint clientEndPoint;
        public volatile ProtocolContext protocolContext;
        public volatile Object dbPessimisticLock = new Object();
        public volatile string mac="UNDEFINED";


        public volatile string ClientIP;
        public volatile string ClientPort;


        public readonly ConcurrentQueue<byte[]> SendingsDelayedQueue = new ConcurrentQueue<byte[]>();

    }
}
