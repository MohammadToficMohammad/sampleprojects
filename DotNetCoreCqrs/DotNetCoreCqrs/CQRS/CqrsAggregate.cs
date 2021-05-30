using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static DotNetCoreCqrs.CQRS.ICqrsAggregate;


//commands handler
namespace DotNetCoreCqrs.CQRS
{
    public class CqrsAggregate : ICqrsAggregate
    {
        public  event EventHandlerDelegate<EventData> carAdded;
        public  event EventHandlerDelegate<EventData> carDeleted;

       
        //simple eventsourcing queue 
        public static ConcurrentQueue<CqrsCommand> eventStore = new ConcurrentQueue<CqrsCommand>();

        public CqrsAggregate() 
        {
           
        }
        public async Task<object> HandleAsync(CqrsCommand cmd) 
        {
            eventStore.Enqueue(cmd);

            switch (cmd.type) 
            {
                case CQRSCOMMANDTYPE.ADDCAR:
                    Task<object> result1=carAdded?.Invoke(this,cmd.eventData);
                    await result1;
                    break;
                case CQRSCOMMANDTYPE.DELETECAR:
                    Task<object> result2 = carDeleted?.Invoke(this, cmd.eventData);
                    await result2;
                    break;
            }
                
            
            return null;
        }



    }

    public enum CQRSCOMMANDTYPE
    {
        ADDCAR,
        DELETECAR
    }


}
