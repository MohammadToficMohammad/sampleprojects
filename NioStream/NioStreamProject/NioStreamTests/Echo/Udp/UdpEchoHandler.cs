using Microsoft.Extensions.Logging;
using NioStream.Abstracts;
using NioStream.Defaults.Helpers;
using NioStream.Pipe;
using System;


namespace NioStream.Protocols.TESTS.Udp
{
    public class UdpEchoHandler : Pipe.AbstractHandler
    {

        public UdpEchoHandler(ILogger _logger,ProtocolContext prtCtx) : base(_logger, prtCtx)
        {

        }

        public override object NextHandle(ProtocolContext prCtx, MessageContext msgCtx)
        {
            try
            {

               // msgCtx.bytes = Helpers.addByteToArrayTail(msgCtx.bytes,0x07);
                //pass the message to the previous for sending 
                return base.PreviousHandle(prCtx, msgCtx);

                //pass the message to the next 
                // return base.NextHandle(prCtx, msgCtx);
            }
            catch (Exception ee)
            {
                _logger.LogError($"Test handler  error {ee.Message}"); return null;
            };
        }


        protected override void onAcceptEvent(ClientContainer cnt)
        {
            _logger.LogInformation($"New TEST client {cnt.host} from handler");

        }


        //can be called several time for one close
        protected override void onCloseEvent(ClientContainer cnt)
        {

            _logger.LogInformation($"closed TEST client {cnt.host} from handler");
        }

    }
}
