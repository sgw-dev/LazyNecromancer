using fancy.component.model;
using UnityEngine;

namespace fancy.component.gameobject
{
    [CreateAssetMenu(fileName = "new GameObject Prop", menuName = "Fancy/GameObjectProp")]
    public class GameObjectProp : IProp
    {
        public GameObjectState state;
        public bool disableAfter;

        public override IState GetState()
        {
            return state;
        }

        public override IComponent Initialize(GameObject gameObject)
        {
            return new GameObjectComponent(gameObject);
        }
    }
}
