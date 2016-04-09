using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public interface IEditorBehaviour<T> where T : IEntity
    {
        T ToGameInstance();

        T GameInstance { get; }
    }
}
