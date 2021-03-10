using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PoolingSystemTest
    {
        [Test]
        public void AddItemTest()
        {
            GameObject gameObject = new GameObject();
            MovingPlatform movingPlatform = gameObject.AddComponent<MovingPlatform>();

            List<MovingPlatform> list = new List<MovingPlatform>();
            list.Add(movingPlatform);

            PoolingSystem<MovingPlatform> pool = new PoolingSystem<MovingPlatform>(list);

            MovingPlatform pooledInstance = pool.GetRandomItem();

            Assert.AreEqual(movingPlatform, pooledInstance);
        }

        [Test]
        public void ReleaseItemTest()
        {
            GameObject gameObject = new GameObject();
            MovingPlatform movingPlatform = gameObject.AddComponent<MovingPlatform>();

            List<MovingPlatform> list = new List<MovingPlatform>();
            list.Add(movingPlatform);

            PoolingSystem<MovingPlatform> pool = new PoolingSystem<MovingPlatform>(list);

            MovingPlatform randomMovingPlatform = pool.GetRandomItem();
            pool.ReleaseItem(randomMovingPlatform);

            randomMovingPlatform = pool.GetRandomItem();

            Assert.AreEqual(movingPlatform, randomMovingPlatform);
        }

        [Test]
        public void EmptyPoolTest()
        {
            List<MovingPlatform> list = new List<MovingPlatform>();
            PoolingSystem<MovingPlatform> pool = new PoolingSystem<MovingPlatform>(list);

            MovingPlatform pooledInstance = pool.GetRandomItem();

            Assert.AreEqual(default(MovingPlatform), pooledInstance);
        }
    }
}
