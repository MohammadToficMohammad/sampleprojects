using System.Collections.Generic;
using System.Threading;

namespace NioStream.Abstracts
{
    public class BlockingQueue<T>
    {
        private Queue<T> m_Queue = new Queue<T>();
        public int Count => m_Queue.Count;
        public void Enqueue(T item)
        {
            lock (m_Queue)
            {
                m_Queue.Enqueue(item);
                Monitor.Pulse(m_Queue);
            }
        }

        public T Dequeue()
        {
            lock (m_Queue)
            {
                while (m_Queue.Count == 0)
                {
                    Monitor.Wait(m_Queue);
                }
                return m_Queue.Dequeue();
            }
        }
        bool closing;
        public void Close()
        {
            lock (m_Queue)
            {
                closing = true;
                Monitor.PulseAll(m_Queue);
            }
        }
        public bool TryDequeue(out T value)
        {


            lock (m_Queue)
            {
                while (m_Queue.Count == 0)
                {
                    if (closing)
                    {
                        value = default(T);
                        return false;
                    }
                    Monitor.Wait(m_Queue);
                }
                value = m_Queue.Dequeue();
                return true;
            }

           
        }
    }




   
}
