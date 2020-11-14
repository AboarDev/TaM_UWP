using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TaM;

namespace TaMTests
{
    [TestClass]
    public class GameHoldsLevelTests
    {
        Game game;

        public void MakeEmptyGame()
        {
            game = new Game();
        }
        [TestMethod, TestCategory("0Levels")]
        public void EmptygameHasLevelCountOf0()
        {
            MakeEmptyGame();
            Assert.AreEqual(0, game.LevelCount);
        }


        [TestMethod, TestCategory("0Levels")]
        public void EmptyGameHasHeight0()
        {
            MakeEmptyGame();
            Assert.AreEqual(0, game.LevelHeight);
        }



        [TestMethod, TestCategory("0Levels")]
        public void EmptyGameHasWidth0()
        {
            MakeEmptyGame();
            Assert.AreEqual(0, game.LevelWidth);
        }

        [TestMethod, TestCategory("0Levels")]
        public void EmptyGameHasLevelNameOf_no_levels_loaded()
        {
            MakeEmptyGame();
            string expectedLevelName = "No levels loaded";
            string actualLevelName = game.CurrentLevelName;
            Assert.AreSame(expectedLevelName, actualLevelName);
        }

        [TestMethod, TestCategory("0Levels")]
        public void EmptyGameHasEmptyNamesList()
        {
            MakeEmptyGame();
            int actualNumberOfNames = game.LevelNames().Count;
            Assert.AreEqual(0, actualNumberOfNames);
        }
        void MakeGameWithOneLevel()
        {
            game = new Game();
            game.AddLevel("level 1", 2, 3, "doesnotmatter");
        }
        [TestMethod, TestCategory("1level")]
        public void GameWithOneLevelHasLevelCountOf1()
        {
            MakeGameWithOneLevel();
            Assert.AreEqual(1, game.LevelCount);
        }
        [TestMethod, TestCategory("1level")]
        public void GameWithOneLevelHasHeightOfLevel()
        {
            MakeGameWithOneLevel();
            Assert.AreEqual(3, game.LevelHeight);
        }

        [TestMethod, TestCategory("1level")]
        public void GameWithOneLevelHasWidthOfLevel()
        {
            MakeGameWithOneLevel();
            Assert.AreEqual(2, game.LevelWidth);
        }

        [TestMethod, TestCategory("1level")]
        public void GameWithOneLevelHasLevelName()
        {
            MakeGameWithOneLevel();
            string expectedLevelName = "level 1";
            string actuallevelName = game.CurrentLevelName;
            Assert.AreSame(expectedLevelName, actuallevelName);
        }
        public void GameWithOneLevelHasSingleEntryNamesList()
        {
            MakeEmptyGame();
            int actualNumberOfNames = game.LevelNames().Count;
            Assert.AreEqual(1, actualNumberOfNames);
        }
        void MakeGameWithThreeLevels()
        {
            game = new Game();
            game.AddLevel("level 1", 1, 1, "doesnotmatter");
            game.AddLevel("level 2", 2, 2, "doesnotmatter");
            game.AddLevel("level 3", 3, 3, "doesnotmatter");
        }

        [TestMethod, TestCategory("3levels")]
        public void GameWithThreeLevelsHasLevelCountOf3()
        {
            MakeGameWithThreeLevels();
            int expectedLevelCount = 3;
            int actualLevelCount = game.LevelCount;
            Assert.AreEqual(expectedLevelCount, actualLevelCount);
        }
        [TestMethod, TestCategory("3levels")]
        public void GameWithThreeLevelsHasHeightOfLastLevel()
        {
            MakeGameWithThreeLevels();
            Assert.AreEqual(3, game.LevelHeight);
        }

        [TestMethod, TestCategory("3levels")]
        public void GameWithThreeLevelsHasWidthOflastLevel()
        {
            MakeGameWithThreeLevels();
            Assert.AreEqual(3, game.LevelWidth);
        }

        [TestMethod, TestCategory("3levels")]
        public void GameWithThreeLevelsHasLastLevelName()
        {
            MakeGameWithThreeLevels();
            string expectedLevelName = "level 3";
            string actuallevelName = game.CurrentLevelName;
            Assert.AreSame(expectedLevelName, actuallevelName);
        }
        public void GameWithThreeLevelsHasThreeEntryNamesList()
        {
            MakeGameWithThreeLevels();
            int actualNumberOfNames = game.LevelNames().Count;
            Assert.AreEqual(3, actualNumberOfNames);
        }



        public void GameWithThreeLevelsHasCorrectNamesList()
        {
            MakeGameWithThreeLevels();
            List<string> actualNames = game.LevelNames();
            List<string> expectedNames = new List<string>();
            expectedNames.Add("level 1");
            expectedNames.Add("level 2");
            expectedNames.Add("level3");
            Assert.AreSame(expectedNames, actualNames);
        }
        public void GameWithThreeLevelsCanChangeCurrentLevel()
        {
            MakeGameWithThreeLevels();
            string expectedName = "Level 2";
            game.SetLevel("level 2");
            string actualName = game.CurrentLevelName;
            Assert.AreSame(expectedName, actualName);
        }

        public void GameWithThreeLevelsDoesNotChangeCurrentLevelIfNameInvalid()
        {
            MakeGameWithThreeLevels();
            string expectedName = "Level 3";
            game.SetLevel("level 666");
            string actualName = game.CurrentLevelName;
            Assert.AreSame(expectedName, actualName);
        }
    }
}
