using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SavingSystemTest
    {
        [Test]
        public void SavingSystemMethodTest()
        {
            string fileName = "TempUnitTestFile.bin";

            HighScoreData highScoreDataSaved = new HighScoreData();         
            SavingSystem<HighScoreData>.Save(highScoreDataSaved, fileName);

            HighScoreData highScoreDataLoaded = SavingSystem<HighScoreData>.Load(fileName);
            
            Assert.AreEqual(highScoreDataSaved, highScoreDataLoaded);
        }
    }
}
