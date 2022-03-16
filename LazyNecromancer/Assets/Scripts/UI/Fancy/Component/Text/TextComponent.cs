using fancy.component.model;
using UnityEngine;
using UnityEngine.UI;

namespace fancy.component.text
{
    public class TextComponent : BaseComponent<Text, TextProp, TextState>
    {
        public TextComponent(GameObject gameObject) : base(gameObject)
        {
            origin = new TextState(baseComponent);
        }

        public override void StartEvent(IProp prop)
        {
            base.StartEvent(prop);
            from = new TextState(baseComponent);
        }

        public override void Evalulate(float t)
        {
            baseComponent.color = Color.Lerp(from.color, to.color, prop.colorCurve.Evaluate(t));
            baseComponent.fontSize = (int)Mathf.Lerp(from.fontSize, to.fontSize, prop.fontSizeCurve.Evaluate(t));
        }

        public override void EndEvent()
        {
            baseComponent.color = to.color;
            baseComponent.fontSize = to.fontSize;
        }
    }
}
