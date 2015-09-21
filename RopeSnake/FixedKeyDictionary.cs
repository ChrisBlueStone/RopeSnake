using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RopeSnake
{
    public class FixedKeyDictionary<K, V> : IDictionary<K, V>
    {
        private IDictionary<K, V> dict = new Dictionary<K, V>();

        public FixedKeyDictionary(params K[] keys) : this((IEnumerable<K>)keys) { }

        public FixedKeyDictionary(IEnumerable<K> keys)
        {
            foreach (K key in keys)
                dict.Add(key, default(V));
        }

        public V this[K key]
        {
            get
            {
                return dict[key];
            }

            set
            {
                dict[key] = value;
            }
        }

        public int Count
        {
            get
            {
                return dict.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return dict.IsReadOnly;
            }
        }

        public ICollection<K> Keys
        {
            get
            {
                return dict.Keys;
            }
        }

        public ICollection<V> Values
        {
            get
            {
                return dict.Values;
            }
        }

        public void Add(KeyValuePair<K, V> item)
        {
            throw new InvalidOperationException();
        }

        public void Add(K key, V value)
        {
            throw new InvalidOperationException();
        }

        public void Clear()
        {
            throw new InvalidOperationException();
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return dict.Contains(item);
        }

        public bool ContainsKey(K key)
        {
            return dict.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            dict.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return dict.GetEnumerator();
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            throw new InvalidOperationException();
        }

        public bool Remove(K key)
        {
            throw new InvalidOperationException();
        }

        public bool TryGetValue(K key, out V value)
        {
            return dict.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dict.GetEnumerator();
        }
    }
}
