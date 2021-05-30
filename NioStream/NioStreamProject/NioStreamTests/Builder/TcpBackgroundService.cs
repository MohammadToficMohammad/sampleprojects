using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NioStream.Abstracts;
using NioStream.Default;
using NioStream.Pipe;
using NioStream.Protocols.TESTS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace NioStreamProject.NioStreamTests.Builder
{
    public class EchoTcpProtocolContext : ProtocolContext
    {


        public EchoTcpProtocolContext() : base()
        {


            localEndPoint = IPEndPoint.Parse("10.0.0.119:313");
            // localUdpEndPoint = IPEndPoint.Parse("10.2.0.21:3210");


            //tls configs
            // TlsCert = new X509Certificate2(@"C:\Users\Mohammad\Desktop\sslnetty\ssl\opensslbins\outputs\tscert.pfx", "password");
            TlsCert = null;
            isTlsImplicit = false;

        }

    }



    public class TcpBackgroundService : BackgroundService
    {
        public ILogger<TcpBackgroundService> logger;
        public TcpBackgroundService(ILogger<TcpBackgroundService> _logger)
        {
            logger = _logger;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            TcpProtocolInitStarter.builder().withLogger(logger)
            .withProtocolContext(new EchoTcpProtocolContext())
            .withPipeBuilder((builder) =>
            {
                //define handlers
                var loggerhandler = new TSloggingHandler(builder.logger, builder.ptCtx);
                var echoHandler = new EchoHandler(builder.logger, builder.ptCtx);

                //connect the pipe
                loggerhandler.SetNext(echoHandler);
                echoHandler.SetPrevious(loggerhandler);

                //define pipe head
                builder.Head = loggerhandler;

                //define pipe default writer
                builder.Tail = echoHandler;

            }).build();



            return Task.CompletedTask;
        }
    }
}
