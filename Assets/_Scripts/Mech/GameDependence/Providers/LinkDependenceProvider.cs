using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Mech
{
    public abstract class LinkDependenceProvider<TLink, T>
    {
        protected object mainObject;

        protected Dictionary<string, LinkDependece<TLink, T>> dependencies;
        protected Dictionary<TLink, T> container;

        public LinkDependenceProvider(object mainObject, Dictionary<TLink, T> container)
        {
            this.mainObject = mainObject;
            this.container = container;

            dependencies = new Dictionary<string, LinkDependece<TLink, T>>();
        }

        public LinkDependenceProvider(Dictionary<TLink, T> container) :
            this(null, container)
        {
        }

        public LinkDependece<TLink, T> Get(Enum link)
        {
            return Get(link.ToString());
        }

        public LinkDependece<TLink, T> Get(string link)
        {
            return dependencies[link];
        }
    }
}
