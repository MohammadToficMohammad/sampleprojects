using Microsoft.Extensions.Logging;
using NioStream.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NioStream.Pipe
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);

        object NextHandle(ProtocolContext prCtx,MessageContext msgCtx);

        IHandler SetPrevious(IHandler handler);

        object PreviousHandle(ProtocolContext prCtx, MessageContext msgCtx);
    }

    public abstract class AbstractHandler : IHandler
    {
        public ILogger _logger;
        public ProtocolContext protocolContext;

        public AbstractHandler(ILogger logger, ProtocolContext _prtCtx)
        {
            _logger = logger;
            protocolContext = _prtCtx;

            _logger.LogInformation("NIO subscribed started");
            if (protocolContext._TCPProtocolInitStarter != null)//udp check
            {
                _logger.LogInformation("NIO subscribed ok");
                protocolContext._TCPProtocolInitStarter.onAccept += onAcceptEvent;
                protocolContext._TCPProtocolInitStarter.onClose += onCloseEvent;
                protocolContext._TCPProtocolInitStarter.onBeforeNextRead += onBeforeNextReadEvent;
                protocolContext._TCPProtocolInitStarter.onBeforeAccept += onBeforeAcceptEvent;
                protocolContext._TCPProtocolInitStarter.onBeforeNextAccept += onBeforeNextAcceptEvent;
                protocolContext._TCPProtocolInitStarter.onJustAccept += onJustAcceptEvent;

                ;
            }
        }


        protected virtual void onJustAcceptEvent(Socket listener, Socket lastclient)
        {

        }
        protected virtual void onBeforeNextAcceptEvent(Socket listener)
        {

        }


        protected virtual void onBeforeAcceptEvent(Socket listener)
        {

        }

        protected virtual void onBeforeNextReadEvent(ProtocolContext pCtx,MessageContext msgCtx)
        {


        }

        protected virtual void onAcceptEvent(ClientContainer cnt)
        {
           

        }


        //can be called several time for one close
        protected virtual  void onCloseEvent(ClientContainer cnt)
        {

            
        }


        public IHandler _nextHandler;

        public IHandler _prevtHandler;

        public IHandler SetNext(IHandler handler)
        {
            this._nextHandler = handler;


            return handler;
        }

        public virtual object NextHandle(ProtocolContext prCtx, MessageContext msgCtx)
        {
            try { 
            if (this._nextHandler != null)
            {
                return this._nextHandler.NextHandle(prCtx, msgCtx);
            }
            else
            {
                return null;
            }
            }
            catch (Exception ee) { _logger.LogError($"handler  error {ee}"); return null; };
        }

        public IHandler SetPrevious(IHandler handler)
        {
            this._prevtHandler = handler;


            return handler;
        }

        public virtual object PreviousHandle(ProtocolContext prCtx, MessageContext msgCtx)
        {
            if (this._prevtHandler != null)
            {
                return this._prevtHandler.PreviousHandle(prCtx, msgCtx);
            }
            else
            {
                if (!msgCtx.UDP)
                {
                    // msgCtx.socket.SendTimeout = 10;
                    // msgCtx.socket.SendBufferSize = msgCtx.bytes.Length;
                    // msgCtx.socket.Send(msgCtx.bytes);
                    msgCtx.clientContainer.SendToClient(msgCtx.bytes);
                    return null;
                }
                else {
                    msgCtx.UdpSocket.SendTimeout = 500;
                    msgCtx.UdpSocket.SendTo(msgCtx.bytes, 0, msgCtx.bytes.Length,SocketFlags.None, msgCtx.UDPRempteEndPoint);
                    return null;

                }
            }
        }
        //TODO async
        public virtual void SendUdp(ProtocolContext prCtx, MessageContext msgCtx, byte[] msg, string OperationId = null)
        {
            Task.Run( ()=> {
                msgCtx.UdpSocket.SendTimeout = 500;
                msgCtx.UdpSocket.SendTo(msgCtx.bytes, 0, msgCtx.bytes.Length, SocketFlags.None, msgCtx.UDPRempteEndPoint);
            });
           
        }

        public virtual object Send(ProtocolContext prCtx, MessageContext msgCtx,byte[] msg,string OperationId=null)
        {
           
            var mCtx = new MessageContext();
            mCtx.OperationId = msgCtx.OperationId;
            mCtx.clientContainer = msgCtx.clientContainer;
            mCtx.bytes = msg;
            mCtx.OperationId = OperationId;
            if (this._prevtHandler != null)
            {
                return this._prevtHandler.PreviousHandle(prCtx, mCtx);
            }
            else
            {
                //  msgCtx.socket.SendBufferSize = mCtx.bytes.Length;
                //  msgCtx.socket.Send(mCtx.bytes);
                msgCtx.clientContainer.SendToClient(mCtx.bytes);
                return null;
            }
        }
    }
}
