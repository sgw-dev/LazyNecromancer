using UnityEngine;

namespace fancy.component.model
{
    public abstract class BaseComponent<Type, Prop, State> : IComponent where Prop : IProp
    {
        protected Type baseComponent;
        public Prop prop;
        protected State to;
        protected State from;
        protected State origin;

        public BaseComponent(GameObject gameObject)
        {
            baseComponent = gameObject.GetComponent<Type>();
        }

        public virtual void StartEvent(IProp prop)
        {
            this.prop = (Prop)prop;
            if (prop.setAsOrigin)
            {
                origin = (State)prop.GetState();
            }
            to = prop.goToOrigin ? origin : (State)prop.GetState();
        }

        public abstract void Evalulate(float t);

        public abstract void EndEvent();
    }
}