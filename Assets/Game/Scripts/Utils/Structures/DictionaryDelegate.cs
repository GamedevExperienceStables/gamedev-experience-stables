using System;
using System.Collections.Generic;

namespace Game.Utils.Structures
{
    public class DictionaryDelegate<TKey, TDelegate> where TDelegate : Delegate
    {
        private readonly Dictionary<TKey, TDelegate> _eventTable = new();

        public TDelegate this[TKey key] => _eventTable.TryGetValue(key, out TDelegate callback) ? callback : null;

        public void AddListener(TKey key, TDelegate callback)
        {
            if (_eventTable.TryGetValue(key, out TDelegate value))
            {
                _eventTable[key] = (TDelegate)Delegate.Combine(value, callback);
                return;
            }

            _eventTable.Add(key, callback);
        }

        public void RemoveListener(TKey key, TDelegate callback)
        {
            if (!_eventTable.ContainsKey(key))
                return;

            _eventTable[key] = (TDelegate)Delegate.Remove(_eventTable[key], callback);

            if (_eventTable[key] is null)
                _eventTable.Remove(key);
        }
    }
}