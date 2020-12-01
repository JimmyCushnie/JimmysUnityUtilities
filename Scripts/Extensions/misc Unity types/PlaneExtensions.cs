using UnityEngine;

namespace JimmysUnityUtilities
{
    public static class PlaneExtensions
    {
        public static Vector3 PointFromPositionToPlane(this Plane plane, Vector3 position)
        {
            Vector3 planePoint = plane.ClosestPointOnPlane(position);
            return planePoint - position;
        }
    }
}