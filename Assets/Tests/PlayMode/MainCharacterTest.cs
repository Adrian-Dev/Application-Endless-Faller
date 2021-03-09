using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MainCharacterTest
    {
        [UnityTest]
        public IEnumerator MoveLeftTest()
        {
            GameObject gameObject = new GameObject();
            MainCharacter mainCharacter = gameObject.AddComponent<MainCharacter>();

            mainCharacter.Move(Vector3.left);

            Assert.AreEqual(Vector3.left, mainCharacter.transform.position);

            yield return null;
        }

        [UnityTest]
        public IEnumerator MoveRightTest()
        {
            GameObject gameObject = new GameObject();
            MainCharacter mainCharacter = gameObject.AddComponent<MainCharacter>();

            mainCharacter.Move(Vector3.right);

            Assert.AreEqual(Vector3.right, mainCharacter.transform.position);

            yield return null;
        }

        [UnityTest]
        public IEnumerator MoveUpTest()
        {
            GameObject gameObject = new GameObject();
            MainCharacter mainCharacter = gameObject.AddComponent<MainCharacter>();

            mainCharacter.Move(Vector3.up);

            Assert.AreEqual(Vector3.up, mainCharacter.transform.position);

            yield return null;
        }

        [UnityTest]
        public IEnumerator MoveDownTest()
        {
            GameObject gameObject = new GameObject();
            MainCharacter mainCharacter = gameObject.AddComponent<MainCharacter>();

            mainCharacter.Move(Vector3.down);

            Assert.AreEqual(Vector3.down, mainCharacter.transform.position);

            yield return null;
        }
    }
}
