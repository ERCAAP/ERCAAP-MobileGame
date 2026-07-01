using UnityEngine;

namespace LunaGames.Extentions
{
    public static class TransformPlus
    {
        public static void DestroyChildren(this Transform parent)
        {
            foreach (Transform child in parent) Object.Destroy(child.gameObject);
        }

        public static Vector3 EditValues(this Vector3 original, float? x = null, float? y = null, float? z = null)
        {
            original.x = x.HasValue ? x.Value : original.x;
            original.y = y.HasValue ? y.Value : original.y;
            original.z = z.HasValue ? z.Value : original.z;
            return original;
        }
        public static void Move(this Transform original, float? x = null, float? y = null, float? z = null)
        {
            Vector3 position = original.position;

            position.x = x.HasValue ? x.Value : position.x;
            position.y = y.HasValue ? y.Value : position.y;
            position.z = z.HasValue ? z.Value : position.z;

            original.position = position;
        }

        public static Bounds GetBounds<T>(this GameObject parent) where T : Renderer
        {
            Bounds bounds;

            if (parent.transform.childCount > 0) bounds = new Bounds(parent.transform.GetChild(0).position, Vector3.zero);
            else bounds = new Bounds(parent.transform.position, Vector3.zero);

            foreach (T renderer in parent.GetComponentsInChildren<T>())
            {
                bounds.Encapsulate(renderer.bounds);
            }

            return bounds;
        }
    }
}