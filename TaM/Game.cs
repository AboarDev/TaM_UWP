using System.Collections.Generic;

namespace TaM
{
    public class Game
    {
        private List<Level> Levels;
        Level Current;
        public Game (){
            Levels = new List<Level>();
            Current = null;
        }
        public int LevelCount { get => Levels.Count; }
        public string CurrentLevelName { get => Current != null ? Current.Name : "No levels loaded"; }
        public int LevelWidth { get => Current != null ? Current.Width : 0; }
        public int LevelHeight { get => Current != null ? Current.Height : 0; }

        public bool HasMinotaurWon { get => ((Current.MinotuarRow == Current.TheseusRow) && (Current.MinitaurColumn == Current.TheseusColumn)); }
        public bool HasTheseusWon { get => ((Current.ExitRow == Current.TheseusRow) && (Current.ExitColumn == Current.TheseusColumn)); }

        public int MoveCount { get => Current.Moves; }
        public void AddLevel(string name, int width, int height, string data)
        {
            Level theLevel = new Level(name, width, height, data);
            if(data.Length >= 16)
            {
                theLevel.MakeLevel();
            }
            Levels.Add(theLevel);
            Current = theLevel;
        }
        public List<string> LevelNames()
        {
            List<string> result = new List<string>();
            foreach (Level l in Levels)
            {
                result.Add(l.ToString());
            }
            return result;
        }
        public void SetLevel(string name)
        {
            foreach (Level l in Levels)
            {
                if (l.Name == name)
                {
                    Current = l;
                }
            }
        }
        public Square WhatIsAt (int y, int x)
        {
            return Current.AtPosition(y,x);
        }
        public void MoveTheseus(Moves theDirection)
        {
            switch (theDirection)
            {
                case Moves.UP:
                    Current.MoveTheseus(Directions.UP);
                    break;
                case Moves.DOWN:
                    Current.MoveTheseus(Directions.DOWN);
                    break;
                case Moves.LEFT:
                    Current.MoveTheseus(Directions.LEFT);
                    break;
                case Moves.RIGHT:
                    Current.MoveTheseus(Directions.RIGHT);
                    break;
                case Moves.PAUSE:
                    Current.Pause();
                    break;
            }
        }

        public void MoveMinotaur()
        {
            Current.MoveMinotaur();
        }
    }
}
