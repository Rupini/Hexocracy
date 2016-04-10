using UnityEngine;

namespace Hexocracy.Core
{
    public interface IActor : IEntity
    {
        Player Owner { get; }

        bool Active { get; }

        bool Activate();

        bool Deactivate();

        Vector3 Position { get; }

        Figure Figure { get; }
    }
}
