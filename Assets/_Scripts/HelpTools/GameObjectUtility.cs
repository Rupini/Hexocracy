using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Hexocracy.HelpTools
{
    public class GameObjectUtility
    {

        //private List<Type> monoTypes;

        //private GameObjectUtility()
        //{
        //    monoTypes = GameServices.GetHexocracyTypes().Where(type =>
        //        {
        //            Type currType = type.BaseType;
        //            while (currType != typeof(object))
        //            {
        //                if (currType == typeof(MonoBehaviour))
        //                {
        //                    return true;
        //                }
        //                currType = currType.BaseType;
        //            }

        //            return false;
        //        }).ToList();
        //}

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
