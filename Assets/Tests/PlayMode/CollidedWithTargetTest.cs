using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CollidedWithTargetTest
    {
        [UnityTest]
        public IEnumerator CollidedWithTargetMethodTest()
        {
            GameObject gameObject = new GameObject();

            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;

            CollidedWithTarget collidedWithTarget = gameObject.AddComponent<CollidedWithTarget>();
            collidedWithTarget.ResetCollided();
            collidedWithTarget.SetTargetTag("Untagged"); // default target tag value


            GameObject gameObjectTarget = new GameObject();

            gameObjectTarget.AddComponent<BoxCollider>();

            Rigidbody rigidbody = gameObjectTarget.AddComponent<Rigidbody>();
            rigidbody.isKinematic = true;

            yield return new WaitForSecondsRealtime(0.1f);

            Assert.AreEqual(true, collidedWithTarget.Collided);

            yield return null;
        }


    }
}
