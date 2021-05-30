using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NioStream.Abstracts;
using NioStream.Default;
using NioStream.Pipe;
using NioStream.Protocols.TESTS.TlsSwitch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace NioStream.Protocols.TESTS
{
    //client must reconnect with the same ip and source port if not handler must handle the low level switch
    public class TlsDisconnectSwitchContext : ProtocolContext
    {
        public  bool DisconnectSwitchTLS = false;

        public TlsDisconnectSwitchContext() : base()
        {


            localEndPoint = IPEndPoint.Parse("192.168.99.1:313");
            // localUdpEndPoint = IPEndPoint.Parse("10.2.0.21:3210");

            //tls configs
            TlsCert = new X509Certificate2(@"C:\Users\Mohammad\Desktop\sslnetty\ssl\opensslbins\outputs\tscert.pfx", "password");
            isTlsImplicit = false;
        }

    }
    public class TlsDisconnectSwitchInitStarter : TcpProtocolInitStarter
    {
        public TlsDisconnectSwitchInitStarter(ILogger<TlsDisconnectSwitchInitStarter> logger, IConfiguration Configuration) : base(logger, Configuration, new TlsDisconnectSwitchContext())
        {
            _logger.LogInformation("TlsDisconnectSwitchInitStarter started...");

       

           

            //define handlers
            var loggerhandler = new TSloggingHandler(_logger,protocolContext);
            var tlsSwitchHandler = new TlsDisconnectSwitchHandler(_logger, protocolContext);
            var testHandler = new EchoHandler(_logger, protocolContext);

            //connect the pipe
            loggerhandler.SetNext(tlsSwitchHandler).SetNext(testHandler);
            testHandler.SetPrevious(tlsSwitchHandler).SetPrevious(loggerhandler);

            //define pipe head
            PipelineHead = loggerhandler;

            //define pipe default writer
            protocolContext.TailWriter = testHandler;


           
        }


        protected override void onAcceptEvent(ClientContainer cnt) 
        {
            _logger.LogInformation($"New TEST client {cnt.host}");
            if (((TlsDisconnectSwitchContext)protocolContext).DisconnectSwitchTLS)// change it by contexts maps as you like for every client 
            {
                cnt.StartTls();
            }


        }

        protected override void onCloseEvent(ClientContainer cnt)
        {

            _logger.LogInformation($"closed TEST client {cnt.host}");
        }
    }
}
