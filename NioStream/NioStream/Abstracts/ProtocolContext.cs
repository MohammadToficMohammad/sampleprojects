
using NioStream.Pipe;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace NioStream.Abstracts
{
    public class ProtocolContext
    {

        public IPEndPoint localEndPoint;
        public IPEndPoint localUdpEndPoint;

        public volatile AbstractHandler TailWriter;
        public volatile AbstractHandler UDPTailWriter;
        public volatile TcpProtocolInitStarter _TCPProtocolInitStarter;

        //TLS
        public X509Certificate2 TlsCert;
        public bool isTlsImplicit = false;
        public System.Security.Authentication.SslProtocols SslProtocol = System.Security.Authentication.SslProtocols.Default;

        public readonly ConcurrentQueue<InternalMessageContainer> scheduledMessagesQueue = new ConcurrentQueue<InternalMessageContainer>();
        //  public readonly BlockingQueue<InternalMessageContainer> scheduledMessagesQueue = new BlockingQueue<InternalMessageContainer>();
      //we need async // public readonly CustomBlockingCollection<InternalMessageContainer> scheduledMessagesQueue = new CustomBlockingCollection<InternalMessageContainer>();
        public readonly ConcurrentDictionary<string, PresistentClientContext> presistentClientContextsMap = new ConcurrentDictionary<string, PresistentClientContext>();
       

        public Guid Tenant;

        public PresistentClientContext getPresistentClientContextByMac(string mac)
        {
            if (presistentClientContextsMap.TryGetValue(mac, out var outVal))
                return outVal;
            else return null;

        }
    }
    public class InternalMessageContainer
    {
        public volatile byte[] messageBytes;
        public volatile MessageContext messageContext;
        public volatile ProtocolContext protocolContext;

    }
}
