using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreCqrs.CQRS
{
    public class CqrsQuery
    {
        public EventData eventData { get; set ;}

        public CQRSQUERYTYPE type { get; set; }


        public CqrsQuery(EventData _eventData, CQRSQUERYTYPE _type) 
        {
            eventData = _eventData;
            type = _type;
        }
    }

    
}
