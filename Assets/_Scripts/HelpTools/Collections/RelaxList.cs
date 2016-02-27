using System;
using System.Collections.Generic;

namespace Hexocracy.HelpTools
{
    public class RelaxList<T, TValue> where T : IRelaxable<TValue>
    {
        private Dictionary<TValue, T> allItems;
        private List<T> cleanItems;
        private Comparison<T> comparison;

        public RelaxList()
            : this(null)
        {
        }

        public RelaxList(Comparison<T> comparison)
        {
            allItems = new Dictionary<TValue, T>();
            cleanItems = new List<T>();
            this.comparison = comparison == null ? (item1, item) => { return item1.Cost - item.Cost; } : comparison;
        }

        public void Clear()
        {
            allItems.Clear();
            cleanItems.Clear();
        }

        public void Push(T newItem)
        {
            allItems.Add(newItem.Value, newItem);

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

        public void Relax(TValue value, int newPriority)
        {
            allItems[value].Cost = newPriority;
            cleanItems.Sort(comparison);
        }

        public T Find(Predicate<T> condition)
        {
            var items = new List<T>(allItems.Values);
            items.Sort(comparison);

            return items.Find(condition);
        }

        public List<T> FindAll(Predicate<T> condition)
        {
            var items = new List<T>(allItems.Values);
            items.Sort(comparison);

            var minPriority = items.Find(condition).Cost;

            return items.FindAll((item) => { return condition(item) && item.Cost == minPriority; });
        }
    }
}
