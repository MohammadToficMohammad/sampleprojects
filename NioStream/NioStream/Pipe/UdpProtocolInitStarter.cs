
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NioStream.Abstracts;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NioStream.Pipe.Pools;

namespace NioStream.Pipe
{
    public class UdpProtocolInitStarter : BackgroundService
    {
        //pool
        public readonly NioPool<MessageContext> MsgCtxPool;

        //pool
        public readonly NioPool<IPEndPoint> IPEndPointPool;


        public AbstractHandler PipelineHead;
        public ILogger _logger;
        public ProtocolContext protocolContext;
        public readonly List<TaskContainer> _AsyncWorkers = new List<TaskContainer>();
        public Task removePresistentContext;
        public volatile bool removePresistentContextCancelationToken = false;
        public readonly IConfiguration _Configuration;


        public delegate void BeforeUDPNextReadDelegate(byte[] buffer, Socket socket);
        public BeforeUDPNextReadDelegate beforeNextRead;

        public delegate void BeforeUDPFirstReadDelegate();
        public BeforeUDPFirstReadDelegate beforeFirstRead;



        public UdpProtocolInitStarter(ILogger logger, IConfiguration Configuration, ProtocolContext ctx, int poolSize = 1000)
        {
            MsgCtxPool = new NioPool<MessageContext>(poolSize, null, (msgCtx) => {
                msgCtx.ClientIP = null;
            });

            IPEndPointPool = new NioPool<IPEndPoint>(poolSize, new object[] { IPAddress.Any, 0 }, (ipend) =>
            {
                ipend.Address = IPAddress.Any;
                ipend.Port = 0;
            });

            _logger = logger;
            _Configuration = Configuration;

            protocolContext = ctx;

        }

        void ExecuteAsyncThreadMethod()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            try
            {
                listener.Bind(protocolContext.localUdpEndPoint);
                // listener.NoDelay = true;
                // listener.ReceiveTimeout = 100;
                Task.Run(() => {
                    try
                    {
                        var endpointItem = IPEndPointPool.Rent();
                        var flags = SocketFlags.None;
                        StateObject state = new StateObject();
                        state.udpEndPointItem = endpointItem;
                        state.udpEndPoint = endpointItem.Value;
                        state.workSocket = listener;
                        try
                        {
                            beforeFirstRead?.Invoke();
                        }
                        catch (Exception et)
                        {
                            _logger.LogError($"Error on UDP beforeFirstRead {et}");
                        }
                        listener.BeginReceiveMessageFrom(state.buffer, 0, state.buffer.Length, flags, ref state.udpEndPoint, new AsyncCallback(ReceiveCallback), state);
                    }
                    catch (Exception ee)
                    {
                        _logger.LogError($"Error while first udp starting {ee.ToString()}");
                    }
                });


            }
            catch (Exception e)
            {
                _logger.LogError($"Error while first udp starting {e.ToString()}");
            }


        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Thread t = new Thread(new ThreadStart(ExecuteAsyncThreadMethod));
            t.Start();
            return Task.CompletedTask;
        }


        private void ReceiveCallback(IAsyncResult ar)
        {
            //_logger.LogInformation(" start of  ReceiveCallback ");
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;
           // Task.Run(() =>
           //    { 

                try
                {
                    var flags = SocketFlags.None;
                    EndPoint endpoint = state.udpEndPoint;
                    int bytesRead = client.EndReceiveMessageFrom(ar, ref flags, ref endpoint, out var info);
                    if (bytesRead > 0)
                    {
                        //  var bufArr = state.buffer.GetSegment(0, bytesRead).ToArray();
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


                            try
                            {
                                ReadThreadProcess(buf, client, endpoint);
                            }
                            finally
                            {

                                state.udpEndPointItem.Return();
                            }


                            try
                            {
                                Array.Resize(ref nextReadBuf, bytesRead);
                                beforeNextRead?.Invoke(nextReadBuf, client);
                            }
                            catch (Exception et)
                            {
                                _logger.LogError($"Error on UDP beforeFirstRead {et}");
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

                        var endpointItem = IPEndPointPool.Rent();
                        state.udpEndPointItem = endpointItem;
                        state.udpEndPoint = endpointItem.Value;
                        client.BeginReceiveMessageFrom(state.buffer, 0, state.buffer.Length, flags, ref state.udpEndPoint, new AsyncCallback(ReceiveCallback), state);
                    }
                    else
                    {
                        _logger.LogInformation($"udp  closed  {client.RemoteEndPoint}");
                        client.Close();
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"udp  closed  {e.ToString()} {client.RemoteEndPoint}");
                    client.Close();

                }

            
            // _logger.LogInformation(" end of  ReceiveCallback ");
        }

        public void ReadThreadProcess(byte[] buf, Socket client, EndPoint endPoint)
        {
            try
            {
                //var stateArr = (object[])state;
                // var buf = (byte[])stateArr[0];
                //  var socket = (Socket)stateArr[1];
                //var endPoint = (IPEndPoint)stateArr[2];

                var messageCtxItem = MsgCtxPool.Rent();
                try
                {
                    var bufTrimed = buf.TrimEnd();
                    //var messageCtx = new MessageContext();
                    var messageCtx = messageCtxItem.Value;
                    messageCtx.bytes = bufTrimed;
                    messageCtx.UDP = true;
                    messageCtx.UdpSocket = client;
                    messageCtx.UDPRempteEndPoint = (IPEndPoint)endPoint;


                    PipelineHead.NextHandle(protocolContext, messageCtx);
                }
                finally
                {
                    messageCtxItem.Return();
                }


            }
            catch (Exception ee)
            {
                _logger.LogError($"Error while reading message {ee.ToString()} ");
            }

        }





    }


}
