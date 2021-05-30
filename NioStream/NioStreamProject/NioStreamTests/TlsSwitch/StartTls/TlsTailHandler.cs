using Microsoft.Extensions.Logging;
using NioStream.Abstracts;
using NioStream.Pipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NioStream.Protocols.TESTS.TlsSwitch
{
    public class TlsTailHandler : Pipe.AbstractHandler
    {

        public TlsTailHandler(ILogger _logger, ProtocolContext prCtx) : base(_logger, prCtx)
        {

        }

        public override object NextHandle(ProtocolContext prCtx, MessageContext msgCtx)
        {

            _logger.LogInformation("TlsTailHandler recv");
            return null;
        }
        protected override void onBeforeNextReadEvent(ProtocolContext pCtx, MessageContext msgCtx)
        {
            var tlsind = doStartTlsCall(msgCtx.bytes);
            if (tlsind >= 0)
            {
                var tlsBytes = msgCtx.bytes.Skip(tlsind).ToArray();
                var ackBytes = Encoding.UTF8.GetBytes("ACK");
                msgCtx.bytes = ackBytes;
             //   base.PreviousHandle(msgCtx.clientContainer.protocolContext, msgCtx);
                if (tlsBytes.Length > 0)
                {
                    _logger.LogInformation($"*+++tlsind:{tlsind}++++{tlsBytes[0]}++++++++++++++++++++++++++**************++++++++++++++++++++++++++++++++++");
                }
                msgCtx.clientContainer.StartTls(tlsBytes);
                _logger.LogInformation("TlsTailHandler recv TLS start");

            }

        }


        public int doStartTlsCall(byte[] bytes)
        {
            //return startindex of tls stream
            var str = Encoding.UTF8.GetString(bytes).Trim();
            if (!str.Contains("SSLSTART")) return -1;

            var pat = PatternAt(bytes, Encoding.UTF8.GetBytes("SSLSTART"));
            return pat.FirstOrDefault() + 8;


        }

        public static IEnumerable<int> PatternAt(byte[] source, byte[] pattern)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
                {
                    yield return i;
                }
            }
        }
    }
}

