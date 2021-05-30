using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DotNetCoreCqrs.CQRS.CqrsAggregate;

namespace DotNetCoreCqrs.CQRS
{
    public interface ICqrsAggregate
    {
        public Task<object> HandleAsync(CqrsCommand cmd);

        public event EventHandlerDelegate<EventData> carAdded;
        public event EventHandlerDelegate<EventData> carDeleted;


        //EventHandler with Task so we can await it
        public delegate Task<object> EventHandlerDelegate<TEventArgs>(object sender, TEventArgs e);
    }
}
