using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;
using Tests.Utils.Arranges;

namespace Tests.PlayMode
{
    public class EnemySpawnerTests
    {
        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            var enemyPrefab = Resources.Load("Tests/Enemy");
            var enemySpawner = Create.Component<EnemySpawner>();
            enemySpawner.Constructor(enemyPrefab, 0, 1);
            yield return null;
            var spawnedEnemy = GameObject.FindWithTag("EnemyTest");
            var prefabOfThePawnedEnemy = PrefabUtility.GetPrefabParent(spawnedEnemy);
            Assert.AreEqual(enemyPrefab, prefabOfThePawnedEnemy);
        }

        /// <summary>
        /// Load EnemyTest
        /// Create EnemySpawner
        /// Mock Random 270
        /// Find EnemyTest
        /// Get ParentEnemyTest
        /// Assert EnemyTest ParentEnemyTest
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator WhenSpawnRandomEnemyShouldBeEnemyAndParentEnemy()
        {
            var enemyPrefab = Resources.Load("Tests/EnemyTest");
            var enemySpawner = Create.Component<EnemySpawner>();
            enemySpawner.Constructor(enemyPrefab, 0, 1);

            var random = Substitute.For<System.Random>();
            random.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(270);
            enemySpawner.Random = random;
            yield return null;
            var spawnedEnemy = GameObject.FindWithTag("EnemyTest");
            var prefabOfThePawnedEnemy = PrefabUtility.GetPrefabParent(spawnedEnemy);
            Assert.AreEqual(enemyPrefab, prefabOfThePawnedEnemy);
        }

        [UnityTest]
        public IEnumerator Instantiations_Occur_On_An_Interval()
        {
            var enemyPrefab = Resources.Load("Tests/Enemy");
            var enemySpawner = Create.Component<EnemySpawner>();
            enemySpawner.Constructor(enemyPrefab, 1, 1);
            yield return new WaitForSeconds(.75f);
            var spawnedEnemy = GameObject.FindWithTag("EnemyTest");
        
            Assert.IsNull(spawnedEnemy);
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var gameObject in GameObject.FindGameObjectsWithTag("EnemyTest"))
            {
                Object.Destroy(gameObject);
            }

            foreach (var gameObject in Object.FindObjectsOfType<EnemySpawner>()) 
            {
                Object.Destroy(gameObject);
            }
        }
    }
}
