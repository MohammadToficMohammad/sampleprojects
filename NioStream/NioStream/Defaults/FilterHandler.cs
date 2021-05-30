using Microsoft.Extensions.Logging;
using NioStream.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NioStream.Default
{
    public class FilterHandler : Pipe.AbstractHandler
    {
        public FilterHandler(ILogger logger, ProtocolContext prCtx) : base(logger, prCtx)
        {

        }
    }
}
