using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public abstract class EntityContainer<T> where T : IEntity
    {
        protected Dictionary<int, T> entities;

        public virtual event Action<T> OnAdd = delegate { };
        public virtual event Action<T> OnRemove = delegate { };

        protected EntityContainer()
        {
            entities = new Dictionary<int, T>();
        }

        public void Add(T entity)
        {
            entities[entity.EntityID] = entity;
        }

        public void Remove(int id)
        {
            entities.Remove(id);
        }

        public void Remove(T entity)
        {
            entities.Remove(entity.EntityID);
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
    }
}
