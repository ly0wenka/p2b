using UnityEngine;

namespace Assets.Scripts.Direction
{
    public static class Rotation
    {
        public static Vector3 RotateRight90() => 90 * Vector3.forward;
        public static Vector3 RotateLeft90() => 90 * Vector3.back;
        public static Vector3 Rotate180() => 180 * Vector3.back;
        public static Vector3 Rotate0() => Vector3.zero;
        
    }
}
