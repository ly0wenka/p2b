using UnityEngine;

namespace Tests.Utils
{
    public static class Wait
    {
        public static readonly WaitForSeconds For100MilliSeconds = new WaitForSeconds(0.1f);
        public static readonly WaitForSeconds For3Seconds = new WaitForSeconds(3);
        public static readonly WaitForSeconds For5Seconds = new WaitForSeconds(5);
        public static readonly WaitForSeconds For20Seconds = new WaitForSeconds(20);
    }
}