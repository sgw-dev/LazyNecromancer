using fancy.component.model;
using UnityEngine;

namespace fancy.component.text
{
    [CreateAssetMenu(fileName = "new Text Prop", menuName = "Fancy/TextProp")]
    public class TextProp : IProp
    {
        public TextState state;
        public AnimationCurve colorCurve;
        public AnimationCurve fontSizeCurve;

        public override IState GetState()
        {
            return state;
        }

        public override IComponent Initialize(GameObject gameObject)
        {
            return new TextComponent(gameObject);
        }
    }
}
