using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    public interface IEditorBehaviour
    {
        void InitGameInstance();
    }

    public interface IEditorBehaviour<T> : IEditorBehaviour
    {
        T ToGameInstance();
    }
}
