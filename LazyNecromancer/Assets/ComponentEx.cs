using System.Collections.Generic;

namespace UnityEngine
{
    public static class ComponentEX
    {
        public static T[] GetComponentsInDirectChildren<T>(this Transform parent, bool includeInactive = false) where T : Component
        {
            List<T> tmpList = new List<T>();
            foreach (Transform transform in parent)
            {
                if (includeInactive || parent.gameObject.activeInHierarchy)
                {
                    tmpList.AddRange(transform.GetComponents<T>());
                }
            }
            return tmpList.ToArray();
        }

        public static T GetComponentInDirectChildren<T>(this Transform parent, int index, bool includeInactive = false) where T : Component
        {
            foreach (Transform transform in parent)
            {
                if (includeInactive || parent.gameObject.activeInHierarchy)
                {
                    T[] tmp = transform.GetComponents<T>();
                    if (tmp.Length > index)
                    {
                        return tmp[index];
                    }
                    index -= tmp.Length;
                }
            }
            return null;
        }
    }
}
