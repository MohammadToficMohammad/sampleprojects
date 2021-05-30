using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NioStream.Pipe
{
    public class TaskContainer
    {
        public volatile Task task;
        public volatile bool cancellationToken = false;

    }
}
