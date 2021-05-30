using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NioStream.Abstracts;
using NioStream.Default;
using NioStream.Pipe;
using System.Net;


namespace NioStream.Protocols.TESTS
{

    public class EchoProtocolContext : ProtocolContext
    {


        public EchoProtocolContext() : base()
        {


            localEndPoint = IPEndPoint.Parse("10.0.0.249:313");
            // localUdpEndPoint = IPEndPoint.Parse("10.2.0.21:3210");


            //tls configs
            // TlsCert = new X509Certificate2(@"C:\Users\Mohammad\Desktop\sslnetty\ssl\opensslbins\outputs\tscert.pfx", "password");
            TlsCert = null;
            isTlsImplicit = false;

        }

    }
    public class EchoInitStarter : TcpProtocolInitStarter
    {
        public EchoInitStarter(ILogger<EchoInitStarter> logger, IConfiguration Configuration) : base(logger, Configuration, new EchoProtocolContext())
        {
            _logger.LogInformation("EchoInitStarter started...");

            //like this we can store cutsom defined props
            //((EchoProtocolContext)protocolContext).MyCustomProp=value ;

          
            //define handlers
            var loggerhandler = new TSloggingHandler(_logger,protocolContext);
            var echoHandler = new EchoHandler(_logger,protocolContext);

            //connect the pipe
            loggerhandler.SetNext(echoHandler);
            echoHandler.SetPrevious(loggerhandler);

            //define pipe head
            PipelineHead = echoHandler;

            //define pipe default writer
            protocolContext.TailWriter = echoHandler;


        


        }


        protected override void onAcceptEvent(ClientContainer cnt) 
        {
            _logger.LogInformation($"New TEST client {cnt.host}");
        
        }


        //can be called several time for one close
        protected override void onCloseEvent(ClientContainer cnt)
        {

            _logger.LogInformation($"closed TEST client {cnt.host}");
        }
    }
}


/*
 load test
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TcpLoadTest
{
    class Program
    {
        public static int numberOfThreads = 200;
        static void Main(string[] args)
        {

            for (int i=0;i< numberOfThreads;i++) 
            {
                Task.Run(async ()=> 
                {

                    TcpClient client = new TcpClient("10.0.0.249", 313);

                    // Translate the passed message into ASCII and store it as a Byte array.
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes("HelloEcho");

                    // Get a client stream for reading and writing.
                    //  Stream stream = client.GetStream();

                    NetworkStream stream = client.GetStream();
                    while (true) {
                      await  Task.Delay(TimeSpan.FromMilliseconds(10));

                    // Send the message to the connected TcpServer.
                    stream.Write(data, 0, data.Length);

                    

                    // Receive the TcpServer.response.

                    // Buffer to store the response bytes.
                    data = new Byte[256];

                    // String to store the response ASCII representation.
                    String responseData = String.Empty;

                    // Read the first batch of the TcpServer response bytes.
                    Int32 bytes = stream.Read(data, 0, data.Length);

                        // responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                        // Console.WriteLine("Received: {0}", responseData);
                    }
                    // Close everything.
                    stream.Close();
                    client.Close();

                });
            }


            Console.ReadLine();
        }
    }
}

 
 
 */