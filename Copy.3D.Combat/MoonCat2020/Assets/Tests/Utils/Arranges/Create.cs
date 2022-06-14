using UnityEngine;

namespace Tests.Utils.Arranges
{
    public static class Create
    {
        public static T Component<T>() where T : Behaviour
        {
            return new GameObject().AddComponent<T>();
        }
    }
}