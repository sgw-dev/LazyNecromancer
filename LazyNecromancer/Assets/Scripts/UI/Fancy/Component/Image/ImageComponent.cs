using fancy.component.model;
using UnityEngine;
using UnityEngine.UI;

namespace fancy.component.image
{
    public class ImageComponent : BaseComponent<Image, ImageProp, ImageState>
    {
        public ImageComponent(GameObject gameObject) : base(gameObject)
        {
            origin = new ImageState(baseComponent);
        }

        public override void StartEvent(IProp prop)
        {
            base.StartEvent(prop);
            from = new ImageState(baseComponent);
        }

        public override void Evalulate(float t)
        {
            baseComponent.color = Color.Lerp(from.color, to.color, prop.colorCurve.Evaluate(t));
        }

        public override void EndEvent()
        {
            baseComponent.color = to.color;
        }
    }
}
