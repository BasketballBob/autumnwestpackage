using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public class DictionarySave<TKey, TValue>
    {
        [SerializeField]
        private List<TKey> _keys = new List<TKey>();
        [SerializeField]
        private List<TValue> _values = new List<TValue>();

        public DictionarySave(Dictionary<TKey, TValue> dict)
        {
            foreach (KeyValuePair<TKey, TValue> pair in dict)
            {
                _keys.Add(pair.Key);
                _values.Add(pair.Value);
            }
        }

        public Dictionary<TKey, TValue> GetDictionary()
        {
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            for (int i = 0; i < _keys.Count; i++)
            {
                dict.Add(_keys[i], _values[i]);
            }

            return dict;
        }

        public void SetValue(TKey key, TValue value)
        {
            for (int i = 0; i < _keys.Count; i++)
            {
                if (key.Equals(_keys[i]))
                {
                    _values[i] = value;
                    return;
                }
            }

            _keys.Add(key);
            _values.Add(value);
        }
    }
}
