using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NioStream.Abstracts;
using NioStream.Default;
using NioStream.Pipe;
using NioStream.Protocols.TESTS.TlsSwitch;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NioStream.Protocols.TESTS
{

    public class TlsStartProtocolContext : ProtocolContext
    {


        public TlsStartProtocolContext() : base()
        {


            localEndPoint = IPEndPoint.Parse("192.168.99.1:313");
            // localUdpEndPoint = IPEndPoint.Parse("10.2.0.21:3210");

            var cert = Directory.GetCurrentDirectory() + @"\cert.pfx";
            Debug.WriteLine("cuuent dic" +cert);
            TlsCert = new X509Certificate2(cert, "password");
            isTlsImplicit = false;
            SslProtocol = System.Security.Authentication.SslProtocols.Tls12;
        }

    }
    public class TlsStartInitStarter : TcpProtocolInitStarter
    {
        public TlsStartInitStarter(ILogger<TlsStartInitStarter> logger, IConfiguration Configuration) : base(logger, Configuration, new TlsStartProtocolContext())
        {
            _logger.LogInformation("TlsStartInitStarter started...");


            //define handlers
            var loggerhandler = new TSloggingHandler(_logger,protocolContext);
            var tlstailHandler = new TlsTailHandler(_logger,protocolContext);


            //connect the pipe
            loggerhandler.SetNext(tlstailHandler);
            tlstailHandler.SetPrevious(loggerhandler);

            //define pipe head
            PipelineHead = loggerhandler;

            //define pipe default writer
            protocolContext.TailWriter = tlstailHandler;

        }

 

        protected override void onAcceptEvent(ClientContainer cnt)
        {
            _logger.LogInformation($"New TEST client {cnt.host}");

        }

        protected override void onCloseEvent(ClientContainer cnt)
        {

            _logger.LogInformation($"closed TEST client {cnt.host}");
        }
    }


    /*TO TEST MAKE NEW PROJECT WITH
      
    static void Main(string[] args)
        {
            string server = "10.2.0.21";
            TcpClient client = new TcpClient(server, 313);

            using (SslStream sslStream = new SslStream(client.GetStream(), false,
                new RemoteCertificateValidationCallback(ValidateServerCertificate), null))
            {
                client.GetStream().Write(System.Text.Encoding.UTF8.GetBytes("HELLO"));
                Task.Delay(TimeSpan.FromSeconds(3)).Wait();
                 client.GetStream().Write(System.Text.Encoding.UTF8.GetBytes("SSLSTART"));
                //   byte[] buf = new byte[100];
                 //  client.GetStream().ReadTimeout = 500;
                 // client.GetStream().Read(buf);
                 //  client.GetStream().Flush();
                //  Debug.WriteLine(Encoding.UTF8.GetString(buf));
        
                sslStream.AuthenticateAsClient(server);
                // This is where you read and send data
                Task.Delay(TimeSpan.FromSeconds(3)).Wait();
                sslStream.Write(System.Text.Encoding.UTF8.GetBytes("TLS SUCCESSs"));
            }
            client.Close();
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate,
        X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

     string server = "192.168.99.1";
            Task.Delay(TimeSpan.FromSeconds(3)).Wait();
            Console.WriteLine("starting...");
            for (int i=0;i<500;i++) 
            {
                try {
                TcpClient client = new TcpClient(server, 313);

                using (SslStream sslStream = new SslStream(client.GetStream(), false,
                    new RemoteCertificateValidationCallback(ValidateServerCertificate), null))
                {
                    client.GetStream().Write(System.Text.Encoding.UTF8.GetBytes("HELLO"));
                     //   client.GetStream().FlushAsync().Wait();
                        //Task.Delay(TimeSpan.FromMilliseconds(100)).Wait();
                        client.GetStream().Write(System.Text.Encoding.UTF8.GetBytes("SSLSTART"));
                       // client.GetStream().FlushAsync().Wait();
                        
                            var buf = new byte[100];
                           client.GetStream().ReadTimeout = 500;
                           client.GetStream().WriteTimeout = 500;
                           client.GetStream().Read(buf);
        


                        //   Debug.WriteLine(Encoding.UTF8.GetString(buf));
                        sslStream.ReadTimeout = 4000;
                    sslStream.AuthenticateAsClient(server);
                    // This is where you read and send data
                  //  Task.Delay(TimeSpan.FromSeconds(3)).Wait();
                    sslStream.Write(System.Text.Encoding.UTF8.GetBytes("TLS SUCCESSs"));
                        client.GetStream().FlushAsync().Wait();
                    }
                client.Close();
                client.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine("error occured");
                }
            }
            

     */
}
