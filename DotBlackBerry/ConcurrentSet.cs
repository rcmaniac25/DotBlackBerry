using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace BlackBerry
{
    internal class ConcurrentSet<T> : ISet<T>
    {
        private ConcurrentDictionary<T, T> data;

        public ConcurrentSet()
        {
            data = new ConcurrentDictionary<T, T>();
        }

        public bool Add(T item)
        {
            if (data.ContainsKey(item))
            {
                return false;
            }
            ((IDictionary<T, T>)data).Add(item, item);
            return true;
        }

        #region Set functions

        public void ExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void UnionWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        #endregion

        void ICollection<T>.Add(T item)
        {
            ((IDictionary<T, T>)data).Add(item, item);
        }

        public void Clear()
        {
            data.Clear();
        }

        public bool Contains(T item)
        {
            return data.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            var index = arrayIndex;
            foreach(var kv in data)
            {
                array[index++] = kv.Key;
            }
        }

        public T[] ToArray()
        {
            var res = new T[data.Count];
            CopyTo(res, 0);
            return res;
        }

        public int Count
        {
            get { return data.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return ((IDictionary<T, T>)data).Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return data.Select(kv => kv.Key).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
