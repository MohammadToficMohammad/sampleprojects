using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NioStream.Default
{
    public class CustomBlockingCollection<T> : BlockingCollection<T>
    {
        public void Enqueue(T item)
        {
           base.Add(item);
        }

        public bool TryDequeue(out T value)
        {
            value = base.Take();
           
            return true;
        }
    }
}
