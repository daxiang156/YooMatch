using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class BezierUtility
    {
        public static Vector3  CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2,float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 p = uu * p0;
            p += 2 * u * t * p1;
            p += tt * p2;

            return p;
        }
    }
}