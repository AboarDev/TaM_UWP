using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TaM;

namespace TaMTests
{
    [TestClass]
    public class ExtraTestsThatMikeDidntWrite
    {
        Game game;
        [TestMethod, TestCategory("MinotaurMoving")]
        public void MinotaurMovesLEFTWhenTheseusIsLEFTAndDOWN()
        {
            game = new Game();
            game.AddLevel("CentredMinotaurWithThesesusBottomLeftIn7by7", 8, 8,
            "0303 0606 0001"
            + " 1001 1000 1000 1000 1000 1000 1100"
            + " 0001 0000 0000 0000 0000 0000 0100"
            + " 0001 0000 0000 0000 0000 0000 0100"
            + " 0001 0000 0000 0000 0000 0000 0100"
            + " 0001 0000 0000 0000 0000 0000 0100"
            + " 0001 0000 0000 0000 0000 0000 0100"
            + " 0011 0010 0010 0010 0010 0010 0110");
            game.MoveMinotaur();
            game.MoveMinotaur();
            bool expectedMinotaurAtOrigin = false;
            bool expectedMinotaurAtDestination = true;
            bool[] expected = { expectedMinotaurAtOrigin,
expectedMinotaurAtDestination };
            bool acutualMinotaurAtOrigin = game.WhatIsAt(3, 3).Minotaur;
            bool actualMinotaurAtDestination = game.WhatIsAt(3, 1).Minotaur;
            bool[] actual = { acutualMinotaurAtOrigin,
actualMinotaurAtDestination };
            CollectionAssert.AreEqual(expected, actual);
        }
        [TestMethod, TestCategory("MinotaurMoving")]
        public void MinotaurMovesRIGHTWhenTheseusIsRIGHTAndDOWN()
        {
            game = new Game();
            game.AddLevel("CentredMinotaurWithThesesusBottomLeftIn7by7", 8, 8,
            "0303 0505 0001"
            + " 1001 1000 1000 1000 1000 1000 1100"
            + " 0001 0000 0000 0000 0000 0000 0100"
            + " 0001 0000 0000 0000 0000 0000 0100"
            + " 0001 0000 0000 0000 0000 0000 0100"
            + " 0001 0000 0000 0000 0000 0000 0100"
            + " 0001 0000 0000 0000 0000 0000 0100"
            + " 0011 0010 0010 0010 0010 0010 0110");
            game.MoveMinotaur();
            game.MoveMinotaur();
            bool expectedMinotaurAtOrigin = false;
            bool expectedMinotaurAtDestination = true;
            bool[] expected = { expectedMinotaurAtOrigin,
expectedMinotaurAtDestination };
            bool acutualMinotaurAtOrigin = game.WhatIsAt(3, 3).Minotaur;
            bool actualMinotaurAtDestination = game.WhatIsAt(3, 5).Minotaur;
            bool[] actual = { acutualMinotaurAtOrigin,
actualMinotaurAtDestination };
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}