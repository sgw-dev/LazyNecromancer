using fancy.component.model;
using UnityEngine;

namespace fancy.component.recttransform
{
    [CreateAssetMenu(fileName = "new RectTransform Prop", menuName = "Fancy/RectTransformProp")]
    public class RectTransformProp : IProp
    {
        public RectTransformState state;
        public AnimationCurve positionCurve;
        public AnimationCurve rotationCurve;
        public AnimationCurve scaleCurve;

        public override IState GetState()
        {
            return state;
        }

        public override IComponent Initialize(GameObject gameObject)
        {
            return new RectTransformComponent(gameObject);
        }
    }
}