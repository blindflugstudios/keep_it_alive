using UnityEngine;

namespace KeepItAlive.Science
{
    public static class PhysicsUtils
    {
        public static bool Overlap(this Bounds bounds, Bounds other)
        {
            var vec = other.center - bounds.center;
            return (Mathf.Abs(vec.x) <= (bounds.extents.x + other.extents.x)
                    && Mathf.Abs(vec.y) <= (bounds.extents.y + other.extents.y)
                    && Mathf.Abs(vec.z) <= (bounds.extents.z + other.extents.z)) ;
        }
    }
}