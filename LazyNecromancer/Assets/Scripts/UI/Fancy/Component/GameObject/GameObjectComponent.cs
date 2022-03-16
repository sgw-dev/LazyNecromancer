using fancy.component.model;
using UnityEngine;

namespace fancy.component.gameobject
{
    public class GameObjectComponent : BaseComponent<GameObject, GameObjectProp, GameObjectState>
    {
        public GameObjectComponent(GameObject gameObject) : base(gameObject)
        {
            baseComponent = gameObject;
            origin = new GameObjectState(gameObject);
        }

        public override void StartEvent(IProp prop)
        {
            base.StartEvent(prop);
            if (to.isActive)
            {
                baseComponent.SetActive(true);
            }
        }

        public override void EndEvent()
        {
            if (prop.disableAfter)
            {
                baseComponent.SetActive(false);
            }
        }

        public override void Evalulate(float t)
        {

        }
    }
}
