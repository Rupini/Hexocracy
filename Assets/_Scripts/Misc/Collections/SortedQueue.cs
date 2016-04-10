using System;
using System.Collections.Generic;

namespace Hexocracy.HelpTools
{
    public class SortedQueue<T> where T : class
    {
        private Comparison<T> comparison;
        public List<T> allItems;
        private List<T> cleanItems;


        public SortedQueue(Comparison<T> comparison)
        {
            this.comparison = comparison;
            allItems = new List<T>();
            cleanItems = new List<T>();
        }

        public void Push(T newItem)
        {
            allItems.Add(newItem);
            
            bool inserted = false;
            for (int i = 0; i < cleanItems.Count; i++)
            {
                if (comparison(cleanItems[i], newItem) >= 0)
                {
                    cleanItems.Insert(i, newItem);
                    inserted = true;
                    break;
                }
            }

            if (!inserted) cleanItems.Add(newItem);
        }

        public T Pop()
        {
            if (cleanItems.Count > 0)
            {
                var item = cleanItems[0];
                cleanItems.RemoveAt(0);
                return item;
            }
            else
                return default(T);
        }


        public void Change(Predicate<T> equality, Action<T> changing)
        {
            var item = cleanItems.Find(equality);
            changing(item);
            cleanItems.Sort(comparison);
        }

        public T FindInAllItems(Predicate<T> condition)
        {
            allItems.Sort(comparison);
            return allItems.Find(condition);
        }

        public void Clear()
        {
            allItems.Clear();
            cleanItems.Clear();
        }

    }
}
