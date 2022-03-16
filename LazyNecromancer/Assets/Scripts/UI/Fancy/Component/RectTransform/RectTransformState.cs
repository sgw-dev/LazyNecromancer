using fancy.component.model;
using UnityEngine;

namespace fancy.component.recttransform
{
    [System.Serializable]
    public struct RectTransformState : IState
    {
        public Vector2 position;
        public Quaternion rotation;
        public Vector2 scale;

        public RectTransformState(RectTransform rectTransform)
        {
            position = rectTransform.anchoredPosition;
            rotation = rectTransform.rotation;
            scale = rectTransform.localScale;
        }
    }
}