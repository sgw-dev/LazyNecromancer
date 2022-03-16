using fancy.component.model;
using UnityEngine;
using UnityEngine.UI;

namespace fancy.component.text
{
    [System.Serializable]
    public class TextState : IState
    {
        public Color color;
        public int fontSize;

        public TextState(Text text)
        {
            color = text.color;
            fontSize = text.fontSize;
        }
    }
}
