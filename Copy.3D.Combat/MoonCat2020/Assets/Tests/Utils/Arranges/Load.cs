using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tests.Utils.Arranges
{
    public static class Load
    {
        public static IEnumerator SceneAsync(string sceneName)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName);
            while (!operation.isDone)
            {
                Debug.Log(message: $"{operation.progress:P}");
                yield return null;
            }
        }
    }
}