using System.Collections.Generic;
using URandom = UnityEngine.Random;

namespace Hexocracy.HelpTools
{
    public static class Extensions
    {
        public static T GetRandom<T>(this List<T> list)
        {
            return list[URandom.Range(0, list.Count)];
        }
    }
}
