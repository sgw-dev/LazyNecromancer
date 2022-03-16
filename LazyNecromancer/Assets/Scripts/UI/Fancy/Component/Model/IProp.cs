using UnityEngine;

namespace fancy.component.model
{
    public abstract class IProp : ScriptableObject
    {
        public bool setAsOrigin;
        public bool goToOrigin;

        public abstract IComponent Initialize(GameObject gameObject);
        public abstract IState GetState();
    }
}