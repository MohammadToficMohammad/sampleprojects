using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NioStream.Abstracts;
using NioStream.Default;
using NioStream.Pipe;
using System.Net;


namespace NioStream.Protocols.TESTS.Udp
{

    public class EchoProtocolContext : ProtocolContext
    {


        public EchoProtocolContext() : base()
        {


            //localEndPoint = IPEndPoint.Parse("10.0.0.119:313");
             localUdpEndPoint = IPEndPoint.Parse("10.0.0.119:313");


            //tls configs
            // TlsCert = new X509Certificate2(@"C:\Users\Mohammad\Desktop\sslnetty\ssl\opensslbins\outputs\tscert.pfx", "password");
            TlsCert = null;
            isTlsImplicit = false;

        }

    }
    public class UdpEchoInitStarter : UdpProtocolInitStarter
    {
        public UdpEchoInitStarter(ILogger<EchoInitStarter> logger, IConfiguration Configuration) : base(logger, Configuration, new EchoProtocolContext())
        {
            _logger.LogInformation("EchoInitStarter started...");

            //like this we can store cutsom defined props
            //((EchoProtocolContext)protocolContext).MyCustomProp=value ;

          
            //define handlers
            var loggerhandler = new TSloggingHandler(_logger,protocolContext);
            var echoHandler = new UdpEchoHandler(_logger,protocolContext);

            //connect the pipe
            loggerhandler.SetNext(echoHandler);
            echoHandler.SetPrevious(loggerhandler);

            //define pipe head
            PipelineHead = loggerhandler;

            //define pipe default writer
            protocolContext.TailWriter = echoHandler;


        


        }


        
    }
}
