using System.Collections.Generic;
using System.Linq;
using URandom = UnityEngine.Random;

namespace Hexocracy.HelpTools
{
    public static class Extensions
    {
        public static T GetRandom<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                return list[URandom.Range(0, list.Count)];
            }
            else
            {
                return default(T);
            }
        }

        public static T GetRandom<T>(this List<T> list, out int index)
        {
            if (list.Count > 0)
            {
                index = URandom.Range(0, list.Count);
                return list[index];
            }
            else
            {
                index = -1;
                return default(T);
            }
        }

        public static List<T> Mix<T>(this List<T> list)
        {
            var mixedSet= new HashSet<T>();

            while (mixedSet.Count < list.Count)
            {
                var item = list.GetRandom();

                if(!mixedSet.Contains(item))
                {
                    mixedSet.Add(item);
                }
            }

            return mixedSet.ToList();
        }
    }
}
