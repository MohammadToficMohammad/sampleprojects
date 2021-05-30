using Microsoft.Extensions.Logging;
using NioStream.Abstracts;
using NioStream.Pipe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NioStream.Protocols.TESTS.TlsSwitch
{
    
    public class TlsDisconnectSwitchHandler : Pipe.AbstractHandler
    {

        public TlsDisconnectSwitchHandler(ILogger _logger, ProtocolContext prCtx) : base(_logger, prCtx)
        {

        }

        public override object NextHandle(ProtocolContext prCtx, MessageContext msgCtx)
        {
            if (Encoding.UTF8.GetString(msgCtx.bytes).Trim().Equals("SSLSTART")) 
            {
                ((TlsDisconnectSwitchContext)prCtx).DisconnectSwitchTLS = true;// change it by contexts maps as you like for every client 
                msgCtx.bytes = Encoding.UTF8.GetBytes("SSLSWITCH ACK");
                base.PreviousHandle(prCtx, msgCtx);
                msgCtx.clientContainer.stream.Close();
                msgCtx.clientContainer.socket.Close();
                msgCtx.clientContainer = null;
                return null;
            }

            if (Encoding.UTF8.GetString(msgCtx.bytes).Trim().Equals("SSLSTOP"))
            {

                ((TlsDisconnectSwitchContext)prCtx).DisconnectSwitchTLS = false;// change it by contexts maps as you like for every client 
                msgCtx.bytes = Encoding.UTF8.GetBytes("SSLSWITCH ACK");
                base.PreviousHandle(prCtx, msgCtx);
                msgCtx.clientContainer.stream.Close();
                msgCtx.clientContainer.socket.Close();
                msgCtx.clientContainer = null;
                return null;
            }
            return base.NextHandle(prCtx, msgCtx);
        }

    }
}
/*
             try
             {


                  if (!msgCtx.clientContainer.isTlsAuth)
                  {
                      msgCtx.clientContainer.TlsSwitchFlag = true; ;
                      msgCtx.clientContainer.stream = new SslStreamCommon((Stream)msgCtx.clientContainer.stream);
                      try
                      {
                          _logger.LogInformation($"TLS handshake start");
                          var tlsCertificate = new X509Certificate2(@"C:\Users\Mohammad\Desktop\sslnetty\ssl\opensslbins\outputs\tscert.pfx", "password");
                          ((SslStream)msgCtx.clientContainer.stream).AuthenticateAsServer(tlsCertificate, clientCertificateRequired: false, checkCertificateRevocation: false);
                          _logger.LogInformation($"TLS handshake start 2");
                      }
                      catch (Exception etls)
                      {
                          _logger.LogInformation($"TLS handshake failed");
                          return null;
                      }

                      msgCtx.clientContainer.isTlsAuth = true;
                  }

                  return base.Handle(prCtx, msgCtx);
              }
              catch (Exception ee)
              {
                  _logger.LogError($"Test handler  error {ee.Message}"); return null;
              };




             
     

      */
