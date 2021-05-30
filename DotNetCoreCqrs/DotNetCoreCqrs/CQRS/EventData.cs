using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreCqrs.CQRS
{
    public class EventData
    {
        public IList<object> data { get; set; } = new List<object>();

        public EventData(IList<object> _data) 
        {
            data = _data;
        }
    }
}
