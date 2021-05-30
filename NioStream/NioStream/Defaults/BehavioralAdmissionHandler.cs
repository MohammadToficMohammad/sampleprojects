using Microsoft.Extensions.Logging;
using NioStream.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NioStream.Default
{
    public class BehavioralAdmissionHandler : Pipe.AbstractHandler
    {
        public  BehavioralAdmissionHandler(ILogger logger, ProtocolContext prCtx) : base(logger, prCtx)
        {

        }
    }
}
