using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    public abstract class CachedMonoBehaviour : MonoBehaviour
    {
        public Transform t { get; protected set; }
        public GameObject go { get; protected set; }
        public Renderer r { get; protected set; }

        protected virtual void Awake()
        {
            t = transform;
            go = gameObject;
            r = GetComponent<Renderer>();
        }
    }
}
