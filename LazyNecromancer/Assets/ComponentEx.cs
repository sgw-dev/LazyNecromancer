using System.Collections.Generic;

namespace UnityEngine
{
    public static class ComponentEX
    {

        public static T[] GetComponentsInDirectChildren<T>(this Transform parent, bool includeInactive = false) where T : Component
        {
            List<T> tmpList = new List<T>();
            GetComponentsInDirectChildren<T>(parent, tmpList, includeInactive);
            return tmpList.ToArray();
        }

        public static void GetComponentsInDirectChildren<T>(this Transform parent, List<T> list, bool includeInactive = false) where T : Component
        {
            foreach (Transform transform in parent)
            {
                if (includeInactive || parent.gameObject.activeInHierarchy)
                {
                    list.AddRange(transform.GetComponents<T>());
                }
            }
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

        public static void SafeDestroy(this Component parent)
        {
            if (parent == null)
            {
                return;
            }
            if (Application.isEditor)
            {
                Object.DestroyImmediate(parent.gameObject);
                return;
            }
            Object.Destroy(parent.gameObject);
        }
    }
}
