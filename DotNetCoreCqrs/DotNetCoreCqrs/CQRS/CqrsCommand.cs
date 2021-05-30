using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreCqrs.CQRS
{
    public class CqrsCommand
    {
        public EventData eventData { get; set; }

        public CQRSCOMMANDTYPE type { get; set; }


        public CqrsCommand(EventData _eventData, CQRSCOMMANDTYPE _type)
        {
            eventData = _eventData;
            type = _type;
        }
    }

    
}
