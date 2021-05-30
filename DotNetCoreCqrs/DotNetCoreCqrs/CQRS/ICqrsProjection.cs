using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreCqrs.CQRS
{
    public interface ICqrsProjection
    {
        public Task<T> HandleAsync<T>(CqrsQuery query) where T : class;
    }
}
