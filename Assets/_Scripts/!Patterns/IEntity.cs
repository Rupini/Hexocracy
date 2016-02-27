using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public interface IEntity
    {
        int EntityID { get; }
        
        bool Destroyed { get; }
        
        void Destroy();
        
        event Action OnDestroy;
    }
}
