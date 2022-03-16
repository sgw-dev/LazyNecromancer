using fancy.component.model;
using UnityEngine;
using UnityEngine.UI;

namespace fancy.component.image
{
    [System.Serializable]
    public class ImageState : IState
    {
        public Color color;

        public ImageState(Image image)
        {
            color = image.color;
        }
    }
}
