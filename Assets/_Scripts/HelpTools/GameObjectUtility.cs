using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Hexocracy.HelpTools
{
    public static class GameObjectUtility
    {
        public static List<T> FindObjectsOfInterfaceType<T>()
        {
            var list = new List<T>();
            foreach (var monoBeh in GameObject.FindObjectsOfType<MonoBehaviour>())
            {
                if (monoBeh.GetType().GetInterface(typeof(T).Name) != null)
                {
                    list.Add((T)(object)monoBeh);
                }
            }

            return list;
        }
    }
}
