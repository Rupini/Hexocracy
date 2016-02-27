using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.HelpTools
{
    public class ReflectionBind<T>
    {
        private Func<T> getter;

        public ReflectionBind(object target, string property)
        {
            getter = (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), target, property);
        }

        public T InvokeGetter()
        {
            return getter();
        }
    }
}
