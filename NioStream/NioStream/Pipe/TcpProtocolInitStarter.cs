using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NioStream.Abstracts;
using NioStream.Defaults;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NioStream.Pipe.Pools;
using static NioStream.Pipe.TcpProtocolInitStarter;

namespace NioStream.Pipe
{

    public class ClientContainer
    {


        public Stream stream;
        public TlsCustomStream _TlsCustomStream;
        public Stream TlsStream;
        public Stream TcpStream;
        public Socket socket;
        public int type = 0;//0 tcp ,1 ssl
        public string host;
        public DateTime lastRead = DateTime.UtcNow;



        public bool isTlsAuth = false;


        public Object StartTlsLock = new Object();
        public SemaphoreSlim StartTlsLockSemaphoreSlim = new SemaphoreSlim(1);

        public volatile ConcurrentDictionary<string, Object> PropsFlags = new ConcurrentDictionary<string, Object>();
        //public bool closed = false;

        public TcpProtocolInitStarter _TCPProtocolInitStarter;

        public ProtocolContext protocolContext;

        public void StartTls(byte[] tlshead = null)
        {
            lock (StartTlsLock)
            {
                // ((NetworkStream)stream).Flush();
                _TlsCustomStream.AddHead(tlshead);// remaining bytes from previous sending if there were some
                stream = TlsStream;
                ((SslStream)stream).AuthenticateAsServer(protocolContext.TlsCert, clientCertificateRequired: false, protocolContext.SslProtocol, checkCertificateRevocation: false);
            }

        }
        public void SendToClient(byte[] bytes)
        {
            lock (StartTlsLock)
            {
                if (stream.Equals(TcpStream))
                {
                    var strm = (NetworkStream)stream;
                    strm.Write(bytes, 0, bytes.Length);
                    strm.Flush();
                }
                else
                if (stream.Equals(TlsStream))
                {
                    var strm = (SslStream)stream;
                    strm.Write(bytes);
                    strm.Flush();
                }
            }
        }
        public async Task SendToClientAsync(byte[] bytes)
        {

                await StartTlsLockSemaphoreSlim.WaitAsync();
                try
                {
                    if (stream.Equals(TcpStream))
                    {
                        var strm = (NetworkStream)stream;
                        await strm.WriteAsync(bytes, 0, bytes.Length);
                        await strm.FlushAsync();
                    }
                    else
                    if (stream.Equals(TlsStream))
                    {
                        var strm = (SslStream)stream;
                        await strm.WriteAsync(bytes, 0, bytes.Length);
                        await strm.FlushAsync();
                    }
                }
                finally
                {
                    StartTlsLockSemaphoreSlim.Release();
                }

        }




    }


    public class TcpProtocolInitStarter : BackgroundService
    {

        //pool
        public readonly NioPool<MessageContext> MsgCtxPool;

        //TODO you can add OnSendCallBack
        public delegate void onAcceptDelegate(ClientContainer cnt);
        public delegate void onCloseDelegate(ClientContainer cnt);
        public AbstractHandler PipelineHead;

        public event onAcceptDelegate onAccept;
        public event onCloseDelegate onClose;
        



        public delegate void beforeNextReadDelegate(ProtocolContext pCtx, MessageContext msgCtx);
        public event beforeNextReadDelegate onBeforeNextRead;


        public delegate void beforeAcceptDelegate(Socket listener);
        public event beforeAcceptDelegate onBeforeAccept;

        public delegate void beforeNextAcceptDelegate(Socket listener);
        public event beforeNextAcceptDelegate onBeforeNextAccept;

        public delegate void justAcceptDelegate(Socket listener, Socket lastclient);
        public event justAcceptDelegate onJustAccept;


        public ILogger _logger;

        public ProtocolContext protocolContext;



        public readonly List<TaskContainer> _AsyncWorkers = new List<TaskContainer>();


        public Task removePresistentContext;
        public volatile bool removePresistentContextCancelationToken = false;

        public readonly IConfiguration _Configuration;

        //TLS
        public X509Certificate2 TlsCert;
        public bool isTlsImplicit = false;
        public System.Security.Authentication.SslProtocols SslProtocol = System.Security.Authentication.SslProtocols.Default;



        public ManualResetEvent allDone = new ManualResetEvent(false);


        protected virtual void onJustAcceptEvent(Socket listener, Socket lastclient)
        {

        }
        protected virtual void onBeforeNextAcceptEvent(Socket listener)
        {

        }


        protected virtual void onBeforeNextReadEvent(ProtocolContext pCtx, MessageContext msgCtx)
        {

        }

        protected virtual void onBeforeAcceptEvent(Socket listener)
        {

        }

        protected virtual void onAcceptEvent(ClientContainer cnt)
        {

        }

        protected virtual void onCloseEvent(ClientContainer cnt)
        {

        }


        public TcpProtocolInitStarter(ILogger logger, IConfiguration Configuration, ProtocolContext ctx,int poolSize=1000)
        {
             MsgCtxPool = new NioPool<MessageContext>(poolSize, null,(msgCtx) => {
                 msgCtx.ClientIP = null;
             });

            onJustAccept += onJustAcceptEvent;
            onBeforeNextAccept += onBeforeNextAcceptEvent;
            onBeforeAccept += onBeforeAcceptEvent;
            onBeforeNextRead += onBeforeNextReadEvent;
            onAccept += onAcceptEvent;
            onClose += onCloseEvent;

            _logger = logger;
            _Configuration = Configuration;

            protocolContext = ctx;

            TlsCert = ctx.TlsCert;
            isTlsImplicit = ctx.isTlsImplicit;
            SslProtocol = ctx.SslProtocol;

            protocolContext._TCPProtocolInitStarter = this;



        }

        public void ExecuteAsyncThreadMethod()
        {

            Socket listener = new Socket(AddressFamily.InterNetwork,
         SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(protocolContext.localEndPoint);
                listener.Listen(1000);

                try
                {
                    Object state = new Object[] { listener };
                    listener.BeginAccept(NewAcceptCallback, state);

                }
                catch (Exception ee)
                {
                    _logger.LogError($"Error while Accpeting socket {ee}");

                }




            }
            catch (Exception e)
            {
                _logger.LogError($"Error while binding socket {e}");
            }
                ;

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Thread t = new Thread(new ThreadStart(ExecuteAsyncThreadMethod));
            t.Start();

            return Task.CompletedTask;
        }

        private void NewAcceptCallback(IAsyncResult ar)
        {
            Object[] statearr = (Object[])ar.AsyncState;
            Socket listener = (Socket)statearr[0];


            Task.Run(() =>
            {
                try
                {
                    onBeforeAccept.Invoke(listener);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error on onBeforeAccept {e}");
                };
                Socket socket = null;
                try
                {
                    socket = listener.EndAccept(out var acceptBuffer, ar);


                    //can be used for ddos
                    try
                    {
                        onJustAccept.Invoke(listener, socket);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Error on onJustAccept {e}");
                    };


                    AcceptCallback(socket);



                }
                catch (Exception een) { _logger.LogInformation($"error on EndAccept {een}"); }


               ;
            });

            //can be used for throttle
            try
            {
                onBeforeNextAccept.Invoke(listener);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error on onBeforeNextAccept {e}");
            };


            Object state = new Object[] { listener };
            listener.BeginAccept(NewAcceptCallback, state);

        }




        public void AcceptCallback(Socket socket)
        {

            //  socket.NoDelay = true;
            //   socket.ReceiveTimeout = 100;

            var host = ((IPEndPoint)socket.RemoteEndPoint).ToString();

            var Cnt = new ClientContainer();

            Cnt.socket = socket;
            Cnt.host = host;


            // Cnt.closed = false;
            Cnt._TCPProtocolInitStarter = this;
            Cnt.protocolContext = protocolContext;
            Cnt.socket.ReceiveTimeout = 500;
            Cnt.socket.SendTimeout = 500;
            Cnt.TcpStream = new NetworkStream(socket);
            Cnt._TlsCustomStream = new TlsCustomStream((Stream)Cnt.TcpStream);
            Cnt.TlsStream = new SslStream((Stream)Cnt._TlsCustomStream);
            if (isTlsImplicit)
            {
                Cnt.type = 1;
                Cnt.stream = Cnt.TlsStream;
                _logger.LogInformation($"NIO 102 {Cnt.stream}");
                Cnt.isTlsAuth = false;

            }
            else
            {
                Cnt.type = 0;
                Cnt.stream = Cnt.TcpStream;
                _logger.LogInformation($"NIO 100 {Cnt.stream}");
            }










            _logger.LogInformation($"NIO 11 {Cnt.stream}");


            if (isTlsImplicit)
            {
                Cnt.isTlsAuth = false; ;
                try
                {

                    ((SslStream)Cnt.stream).AuthenticateAsServer(TlsCert, clientCertificateRequired: false, SslProtocol, checkCertificateRevocation: false);
                }
                catch (Exception etls)
                {

                    onClose.Invoke(Cnt);
                    Cnt.stream.Close();
                    Cnt.socket.Close(); Cnt = null;
                    _logger.LogInformation($"TLS failed for {Cnt.host}");
                    return;

                }
                Cnt.isTlsAuth = true;

            }





            try
            {
                onAccept.Invoke(Cnt);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error on AcceptHandler {e}");
            };




            StateObject stateObj = new StateObject();
            stateObj.workSocket = socket;
            Object state = new Object[] { socket, stateObj, Cnt };
            _logger.LogInformation($"NIO 2 {Cnt.stream}");
            Cnt.stream.BeginRead(stateObj.buffer, 0, StateObject.BufferSize,
              new AsyncCallback(ReceiveCallback), state);

        }
        private void ReceiveCallback(IAsyncResult ar)
        {
            // _logger.LogInformation(" start of  ReceiveCallback ");
            Object[] statearr = (Object[])ar.AsyncState;
            Socket socket = (Socket)statearr[0];
            StateObject state = (StateObject)statearr[1];
            ClientContainer cnt = (ClientContainer)statearr[2];
            //Task.Run(() =>
          //  {
                try
                {
                    int bytesRead = cnt.stream.EndRead(ar);
                    if (bytesRead > 0)
                    {
                        cnt.lastRead = DateTime.UtcNow;
                        // var buf = state.buffer.GetSegment(0, bytesRead).ToArray();
                        byte[] buf = null;
                        byte[] bufRef = null;
                        byte[] nextReadBuf = null;
                        var lng = 0;
                        var arrayPool = ArrayPool<byte>.Shared;
                        try
                        {
                            bufRef = arrayPool.Rent(bytesRead);
                            lng = bufRef.Length;
                            buf = bufRef;
                            nextReadBuf = bufRef;
                            Array.Copy(state.buffer, 0, buf, 0, bytesRead);
                            Array.Resize(ref buf, bytesRead);


                            //ThreadPool.QueueUserWorkItem(ReadThreadProcessNio, new Object[] { buf as Object, cnt as Object });
                            ReadThreadProcessNio(buf, cnt);

                            // Object state2 = new Object[] { socket, state, cnt };

                            var messageCtxItem = MsgCtxPool.Rent();
                            try
                            {
                                Array.Resize(ref nextReadBuf, bytesRead);
                                //var messageCtx = new MessageContext();
                                var messageCtx = messageCtxItem.Value;
                                messageCtx.bytes = nextReadBuf;
                                messageCtx.clientContainer = (ClientContainer)cnt;
                                messageCtx.ClientIP = ((IPEndPoint)messageCtx.clientContainer.socket.RemoteEndPoint).Address.ToString();
                                messageCtx.ClientPort = ((IPEndPoint)messageCtx.clientContainer.socket.RemoteEndPoint).Port.ToString();
                                onBeforeNextRead?.Invoke(cnt.protocolContext, messageCtx);
                            }
                            catch (Exception et)
                            {
                                _logger.LogError($"Error on beforeNextRead {et}");
                            }
                            finally 
                            {
                                messageCtxItem.Return();
                            }


                        }
                        catch (Exception e)
                        {

                        }
                        finally
                        {

                            Array.Resize(ref bufRef, lng);
                            arrayPool.Return(bufRef);
                        }

                        cnt.stream.BeginRead(state.buffer, 0, StateObject.BufferSize,
                         new AsyncCallback(ReceiveCallback), statearr);



                    }
                    else
                    {
                        _logger.LogInformation($"TCP client  closed  {cnt.host}");
                        try
                        {
                            onClose.Invoke(cnt);
                        }
                        catch (Exception e)
                        {
                            _logger.LogError($"Error on closinghandler {e}");
                        }
                        finally
                        {
                            cnt.stream.Close();
                            cnt.socket.Close(); cnt = null;

                        };
                    }
                }
                catch (Exception e)
                {
                    _logger.LogInformation($"TCP client  closed  {cnt.host} {e}");

                    try
                    {
                        onClose.Invoke(cnt);
                    }
                    catch (Exception ee)
                    {
                        _logger.LogError($"Error on closinghandler {ee}");
                    }
                    finally
                    {
                        cnt.stream.Close();
                        cnt.socket.Close(); cnt = null;
                    };



                }

            //});
            //_logger.LogInformation(" end of  ReceiveCallback ");
        }
        public void ReadThreadProcessNio(byte[] buffer, ClientContainer cnt)
        {
            var messageCtxItem = MsgCtxPool.Rent();
            try
            {
                //var stateArr = (Object[])state;
                //var buffer = (byte[])stateArr[0];
                //var messageCtx = new MessageContext();
                var messageCtx = messageCtxItem.Value;
                messageCtx.bytes = buffer;
                //messageCtx.clientContainer = (ClientContainer)stateArr[1];
                messageCtx.clientContainer = cnt;
                messageCtx.ClientIP = ((IPEndPoint)messageCtx.clientContainer.socket.RemoteEndPoint).Address.ToString();
                messageCtx.ClientPort = ((IPEndPoint)messageCtx.clientContainer.socket.RemoteEndPoint).Port.ToString();
                PipelineHead.NextHandle(protocolContext, messageCtx);
            }
            finally 
            {
                messageCtxItem.Return();
            }
              
        }

        public static TcpBuilder builder()
        {
            return new TcpBuilder();
        }

    }

    public class TcpBuilder
    {
        public ProtocolContext ptCtx;
        public ILogger logger;
        public AbstractHandler Head;
        public AbstractHandler Tail;
        public delegate void PipeBuilder(TcpBuilder tcp);
        private PipeBuilder pipeBuilder;
        private onAcceptDelegate acceptHandler;
        private onCloseDelegate closeHandler;


        public TcpBuilder withProtocolContext(ProtocolContext _ptCtx) 
        {
            ptCtx = _ptCtx;
            return this;
        }

        public TcpBuilder withLogger(ILogger _logger)
        {
            logger = _logger;
            return this;
        }


        public TcpBuilder withPipeBuilder(PipeBuilder _pipeBuilder)
        {
            pipeBuilder = _pipeBuilder;
            return this;
        }

        public TcpBuilder withAcceptHandler(onAcceptDelegate _acceptHandler)
        {
            acceptHandler = _acceptHandler;
            return this;
        }

        public TcpBuilder withCloseHandler(onCloseDelegate _closeHandler)
        {
            closeHandler = _closeHandler;
            return this;
        }


        public void build() 
        {
         TcpProtocolInitStarter initer=new TcpProtocolInitStarter(logger,null, ptCtx);
            pipeBuilder.Invoke(this);
            initer.PipelineHead = Head;
            initer.protocolContext.TailWriter = Tail;
            initer.onAccept += acceptHandler;
            initer.onClose += closeHandler;
            Task.Run(()=>{
                Thread t = new Thread(new ThreadStart(initer.ExecuteAsyncThreadMethod));
                t.Start();
            });

        }
    }

    public class StateObject
    {
        // Size of receive buffer.  
        public const int BufferSize = 1024;

        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];

        // Received data string.
        public StringBuilder sb = new StringBuilder();

        // Client socket.
        public Socket workSocket = null;


        //
        public NioItem<IPEndPoint> udpEndPointItem;
        public EndPoint udpEndPoint;






    }


}
