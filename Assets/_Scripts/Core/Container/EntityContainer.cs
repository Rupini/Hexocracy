using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public abstract class EntityContainer<T> where T : IEntity
    {
        protected Dictionary<int, T> entityMap;

        public virtual event Action<T> OnAdd = delegate { };
        public virtual event Action<T> OnRemove = delegate { };

        protected EntityContainer()
        {
            entityMap = new Dictionary<int, T>();
        }

        public void Add(T entity)
        {
            entityMap[entity.EntityID] = entity;
        }

        public void Remove(int id)
        {
            entityMap.Remove(id);
        }

        public void Remove(T entity)
        {
            entityMap.Remove(entity.EntityID);
        }

        public List<T> GetAll()
        {
            return entityMap.Values.ToList();
        }

        public bool Exist(int id)
        {
            return entityMap.ContainsKey(id);
        }

        public T Get(int id)
        {
            if (entityMap.ContainsKey(id))
                return entityMap[id];
            else
                return default(T);
        }

        public void Clear()
        {
            entityMap.Clear();
        }
    }
}
