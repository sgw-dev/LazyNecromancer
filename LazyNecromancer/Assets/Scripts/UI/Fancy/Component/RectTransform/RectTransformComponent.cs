using fancy.component.model;
using UnityEngine;

namespace fancy.component.recttransform
{
    public class RectTransformComponent : BaseComponent<RectTransform, RectTransformProp, RectTransformState>
    {
        public RectTransformComponent(GameObject gameObject) : base(gameObject)
        {
            origin = new RectTransformState(baseComponent);
        }

        public override void StartEvent(IProp prop)
        {
            base.StartEvent(prop);
            from = new RectTransformState(baseComponent);
            if (!prop.goToOrigin)
            {
                to.position += origin.position;
            }
        }

        public override void Evalulate(float time)
        {
            baseComponent.anchoredPosition = Vector2.Lerp(from.position, to.position, prop.positionCurve.Evaluate(time));
            baseComponent.rotation = Quaternion.Lerp(from.rotation, to.rotation, prop.rotationCurve.Evaluate(time));
            baseComponent.localScale = Vector2.Lerp(from.scale, to.scale, prop.scaleCurve.Evaluate(time));
        }

        public override void EndEvent()
        {
            baseComponent.anchoredPosition = to.position;
            baseComponent.rotation = to.rotation;
            baseComponent.localScale = to.scale;
        }
    }
}