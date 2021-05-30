using Microsoft.Extensions.Logging;
using NioStream.Abstracts;
using NioStream.Defaults.Helpers;
using NioStream.Pipe;

namespace NioStream.Default
{
    public class TSloggingHandler : AbstractHandler
    {
        public TSloggingHandler(ILogger logger,ProtocolContext prCtx) : base(logger, prCtx)
        {

        }

        public override object NextHandle(ProtocolContext prCtx, MessageContext msgCtx)
        {
            var udp = msgCtx.UDP ? "UDP" : "TCP";
            _logger.LogInformation($"**************************{udp}**********************\n");
            var operationid = msgCtx.OperationId ?? "NOT PRESENT";
            _logger.LogInformation($"OPERATION ID {operationid}\n");
            if (!msgCtx.UDP) 
            {
                _logger.LogInformation($"READING FROM {msgCtx.clientContainer.host}:\n" + Helpers.FormatByteArray(msgCtx.bytes));
            }
            else
            {
                _logger.LogInformation($"READING FROM {msgCtx.UDPRempteEndPoint}:\n" + Helpers.FormatByteArray(msgCtx.bytes));
            }

            _logger.LogInformation("***************************************************\n");
            return base.NextHandle(prCtx, msgCtx);

        }

        public override object PreviousHandle(ProtocolContext prCtx, MessageContext msgCtx)
        {
            var udp = msgCtx.UDP ? "UDP" : "TCP";
            _logger.LogInformation($"**************************{udp}**********************\n");
            var operationid = msgCtx.OperationId ?? "NOT PRESENT";
            _logger.LogInformation($"OPERATION ID {operationid}\n");
            if (!msgCtx.UDP)
            {
                _logger.LogInformation($"WRITING TO {msgCtx.clientContainer.host}:\n" + Helpers.FormatByteArray(msgCtx.bytes));
            }
            else
            {
                _logger.LogInformation($"WRITING TO {msgCtx.UDPRempteEndPoint}:\n" + Helpers.FormatByteArray(msgCtx.bytes));
            }
            _logger.LogInformation("***************************************************\n");
            return base.PreviousHandle(prCtx, msgCtx);

        }
    }
   
}
