using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NioStream.Abstracts
{
    public abstract class ProtocolProcess
    {

        public ILogger _logger;
        public ProtocolContext _protocolContext;
        public volatile bool _cancellationToken;

        public ProtocolProcess(ILogger Logger, ProtocolContext protocolContext, bool cancellationToken)
        {
            _logger = Logger;
            _protocolContext = protocolContext;
            _cancellationToken = cancellationToken;
        }


        public abstract Task Process();
    }
}
