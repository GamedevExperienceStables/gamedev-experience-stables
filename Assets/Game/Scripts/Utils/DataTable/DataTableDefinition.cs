using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils.DataTable
{
    public abstract class DataTableDefinition : ScriptableObject
    {
        protected const string MENU_PATH = "DataTable/";
    }

    public abstract partial class DataTableDefinition<T> : DataTableDefinition where T : DataTableItemDefinition
    {
        [SerializeField]
        protected List<T> items = new();

        public IReadOnlyList<T> Items => items.AsReadOnly();

        public bool TryGetValue(string id, out T found)
        {
            foreach (T item in items)
            {
                if (item.Id != id)
                    continue;

                found = item;
                return true;
            }

            found = null;
            return false;
        }
    }
}