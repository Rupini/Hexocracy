using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public abstract class EntityContainer<T> where T : IEntity
    {
        protected Dictionary<int, T> entities;

        public virtual event Action<IEnumerable<T>> OnAdd = delegate { };
        public virtual event Action<IEnumerable<T>> OnRemove = delegate { };

        protected EntityContainer()
        {
            entities = new Dictionary<int, T>();
        }

        public void Add(T entity)
        {
            Add(new[] { entity });
        }

        public virtual void Add(IEnumerable<T> entityCollection)
        {
            foreach (var entity in entityCollection)
            {
                entities[entity.EntityID] = entity;
            }
            OnAdd(entityCollection);
        }

        public void Remove(T entity)
        {
            Remove(new[] { entity });
        }

        public virtual void Remove(IEnumerable<T> entityCollection)
        {
            foreach (var entity in entityCollection)
            {
                entities.Remove(entity.EntityID);
            }
            OnRemove(entityCollection);
        }

        public List<T> GetAll()
        {
            return entities.Values.ToList();
        }

        public bool Exist(int id)
        {
            return entities.ContainsKey(id);
        }

        public T Get(int id)
        {
            if (entities.ContainsKey(id))
                return entities[id];
            else
                return default(T);
        }

        public void Clear()
        {
            entities.Clear();
        }

        public int Count
        {
            get
            {
                return entities.Count;
            }
        }
    }
}
