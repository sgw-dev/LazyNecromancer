using fancy.component.model;
using UnityEngine;

namespace fancy.component.image
{
    [CreateAssetMenu(fileName = "new Image Prop", menuName = "Fancy/ImageProp")]
    public class ImageProp : IProp
    {
        public ImageState state;
        public AnimationCurve colorCurve;

        public override IState GetState()
        {
            return state;
        }

        public override IComponent Initialize(GameObject gameObject)
        {
            return new ImageComponent(gameObject);
        }
    }
}
