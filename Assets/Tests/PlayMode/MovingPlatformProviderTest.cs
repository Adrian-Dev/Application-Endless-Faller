using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MovingPlatformProviderTest
    {
        [UnityTest]
        public IEnumerator MovingPlatformProviderOneItemTest()
        {
            GameObject gameObject0 = new GameObject();
            MovingPlatform movingPlatform = gameObject0.AddComponent<MovingPlatform>();

            GameObject gameObject1 = new GameObject();
            MovingPlatformProvider movingPlatformProvider = gameObject1.AddComponent<MovingPlatformProvider>();

            List<MovingPlatform> movingPlatformList = new List<MovingPlatform>();
            movingPlatformList.Add(movingPlatform);
            movingPlatformProvider.InjectDependencies(movingPlatformList);

            // Since there is only one platform in the list, it should return the same reference to the one added previously
            MovingPlatform randomMovingPlatform = movingPlatformProvider.GetRandomPlatform();

            Assert.AreEqual(movingPlatform, randomMovingPlatform);

            yield return null;
        }

        [UnityTest]
        public IEnumerator MovingPlatformProviderEmptyTest()
        {
            GameObject gameObject = new GameObject();
            MovingPlatformProvider movingPlatformProvider = gameObject.AddComponent<MovingPlatformProvider>();

            List<MovingPlatform> movingPlatformList = new List<MovingPlatform>();
            movingPlatformProvider.InjectDependencies(movingPlatformList);

            // Since there are no platforms in the list, it should return a null reference withouth break.
            MovingPlatform randomMovingPlatform = movingPlatformProvider.GetRandomPlatform();

            Assert.AreEqual(null, randomMovingPlatform);

            yield return null;
        }


    }
}
