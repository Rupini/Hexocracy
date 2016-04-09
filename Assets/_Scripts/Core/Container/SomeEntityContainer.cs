using Hexocracy.HelpTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy.Core
{
    [RawPrototype]
    [GameService(GameServiceType.Container)]
    public class SomeEntityContainer : EntityContainer<IEntity>
    {
        public override void InitializeContent()
        {
            GameObjectUtility.FindObjectsOfInterfaceType<IEntityEditor>().ForEach(editor => 
            {
                var entity = editor.ToGameEntity();
                entities[entity.EntityID] = entity;
            });

            base.InitializeContent();
        }
    }
}
