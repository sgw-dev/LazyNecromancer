using fancy.component.model;
using UnityEngine;

namespace fancy.component.gameobject
{
    [System.Serializable]
    public struct GameObjectState : IState
    {
        public bool isActive;

        public GameObjectState(GameObject gameObject)
        {
            isActive = gameObject.activeInHierarchy;
        }
    }
}
