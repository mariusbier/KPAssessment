using System.Collections;
using System.Collections.Concurrent;

namespace UserServiceAPI.Helpers
{
    public class CachedDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private ConcurrentDictionary<TKey, CachedEntry<TValue>> m_elements = new ConcurrentDictionary<TKey, CachedEntry<TValue>>();
        private bool m_entriesCanExpire = true;
        private TimeSpan m_timeToKeep;
        private int m_maxEntries;

        public CachedDictionary(int maxEntries = 1000)
        {
            m_entriesCanExpire = false;
            m_maxEntries = maxEntries;
        }

        public CachedDictionary(TimeSpan timeToKeep, int maxEntries = 10000)
        {
            m_timeToKeep = timeToKeep;
            m_maxEntries = maxEntries;
        }

        public void Clear()
        {
            m_elements.Clear();
        }

        public bool Remove(TKey key)
        {
            return m_elements.TryRemove(key, out _);
        }

        public bool ContainsKey(TKey key)
        {
            return m_elements.ContainsKey(key);
        }

        public bool RemoveByValue(TValue value)
        {
            if (m_elements.Values.Any(v => v.Equals(value)))
            {
                var keyValuePairs = m_elements.Where(kvp => kvp.Value.Value.Equals(value)).ToList();
                foreach (var keyValuePair in keyValuePairs)
                {
                    Remove(keyValuePair.Key);
                }

                return true;
            }

            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        protected class CachedEntry<T>
        {
            public T Value;
            public DateTime ExpiryTime;

            public bool IsStale
            {
                get { return DateTime.Now >= ExpiryTime; }
            }

            public CachedEntry(T Value, TimeSpan TimeToKeep)
            {
                this.Value = Value;
                this.ExpiryTime = DateTime.Now + TimeToKeep;
            }
        }
    }
}
