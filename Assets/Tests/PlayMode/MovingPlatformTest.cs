using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MovingPlatformTest
    {
        [UnityTest]
        public IEnumerator MoveLeftTest()
        {
            GameObject gameObject = new GameObject();
            MovingPlatform movingPlatform = gameObject.AddComponent<MovingPlatform>();

            movingPlatform.Move(Vector3.left);

            Assert.AreEqual(Vector3.left, movingPlatform.transform.position);

            yield return null;
        }

        [UnityTest]
        public IEnumerator MoveRightTest()
        {
            GameObject gameObject = new GameObject();
            MovingPlatform movingPlatform = gameObject.AddComponent<MovingPlatform>();

            movingPlatform.Move(Vector3.right);

            Assert.AreEqual(Vector3.right, movingPlatform.transform.position);

            yield return null;
        }

        [UnityTest]
        public IEnumerator MoveUpTest()
        {
            GameObject gameObject = new GameObject();
            MovingPlatform movingPlatform = gameObject.AddComponent<MovingPlatform>();

            movingPlatform.Move(Vector3.up);

            Assert.AreEqual(Vector3.up, movingPlatform.transform.position);

            yield return null;
        }

        [UnityTest]
        public IEnumerator MoveDownTest()
        {
            GameObject gameObject = new GameObject();
            MovingPlatform movingPlatform = gameObject.AddComponent<MovingPlatform>();

            movingPlatform.Move(Vector3.down);

            Assert.AreEqual(Vector3.down, movingPlatform.transform.position);

            yield return null;
        }
    }
}
