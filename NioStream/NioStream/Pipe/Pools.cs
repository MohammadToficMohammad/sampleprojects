using NioStream.Abstracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NioStream.Pipe.Pools
{


    public class NioPool<T>
    {
        public delegate void CleanCallDelegate(T value);
        public CleanCallDelegate CleanCall;
        private readonly int PoolSize;
        internal ConcurrentBag<NioItem<T>> itemsBag = new ConcurrentBag<NioItem<T>>();
        internal ConcurrentQueue<NioItem<T>> vacantIndexes = new ConcurrentQueue<NioItem<T>>(); //https://stackoverflow.com/questions/9981637/concurrentqueue-holds-objects-reference-or-value-out-of-memory-exception
        public NioPool(int _PoolSize, object[] args = null, CleanCallDelegate _CleanCall = null)
        {
            PoolSize = _PoolSize;
            CleanCall = _CleanCall;
            for (int i = 0; i < PoolSize; i++)
            {
                var item = new NioItem<T>(i,this,args);
            }
        }



        public NioItem<T> Rent()
        {
            Debug.WriteLine($"Pool vacancies for {typeof(T)} is {vacantIndexes.Count}");

            if (vacantIndexes.TryDequeue(out var x))
            {
                return x;
            };
            throw new Exception("all pool rented");
        }

    }


    public class NioItem<T>
    {
        internal readonly int Index;
        public readonly T Value;
        internal readonly NioPool<T> Pool;
        internal NioItem(int _Index, NioPool<T> _Pool, object[] args = null)
        {
            Index = _Index;
            Pool = _Pool;
            Value = args == null ? Activator.CreateInstance<T>() : (T)Activator.CreateInstance(typeof(T), args);
            Pool.itemsBag.Add(this);
            Pool.vacantIndexes.Enqueue(this);
        }

        public void Return()
        {
            Pool.CleanCall?.Invoke(Value);
            Pool.vacantIndexes.Enqueue(this);
        }
    }


}
