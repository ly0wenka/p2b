using Startup;
using UnityEngine;

namespace Tests.Utils.Arranges
{
    public static class Find
    {
        public static MainMenu MainMenu()
        {
            return GameObject.FindWithTag("MainCamera").GetComponent<MainMenu>();
        }
    }
}