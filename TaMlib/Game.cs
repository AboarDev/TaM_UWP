using System;
using System.Collections;
using System.Collections.Generic;

namespace TamLib
{
    public class Game
    {
        private List<Level> Levels;
        public Game ()
        {
            Levels = new List<Level>();
            Current = null;
        }

        public Level Current { get; private set; }
        public int LevelCount { get => Levels.Count; }
        public string CurrentLevelName { get => Current != null ? Current.Name : "No levels loaded"; }
        public int LevelWidth { get => Current != null ? Current.Width : 0; }
        public int LevelHeight { get => Current != null ? Current.Height : 0; }
        // Row Y, Column X
        public Position TheseusPosition { get => Current.TheseusPosition;  }
        public Position MinotaurPosition { get => Current.MinotaurPosition; }
        public Position ExitPosition { get => Current.ExitPosition;  }
        public Square[,] Squares { get => Current.Squares; }
        public bool HasMinotaurWon { get => ((Current.MinotaurPosition.Y == Current.TheseusPosition.Y) && (Current.MinotaurPosition.X == Current.TheseusPosition.X)); }
        public bool HasTheseusWon { get => ((Current.ExitPosition.Y == Current.TheseusPosition.Y) && (Current.ExitPosition.X == Current.TheseusPosition.X)); }
        public int MoveCount { get => Current.MoveCount; }

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
            return Current.AtPosition(new Position(y,x));
        }
        public bool MoveTheseus(Moves theDirection)
        {
            return Current.MoveTheseus(theDirection);
        }

        public void MoveMinotaur()
        {
            Current.MoveMinotaur();
        }

        public Move Undo ()
        {
            if (MoveCount > 0 && Current != null)
            {
                int index = MoveCount - 1;
                Move move = Current.AllMoves[index];
                Current.SetPositions(move);
                Current.AllMoves.RemoveAt(index);
                Current.MoveCount--;
                return move;
            }
            return new Move { TheseusMoved = false, MinotaurMoved = false };
        }

        public void SetMoves (List<Move> moves)
        {
            Current.AllMoves = moves;
        }

        public void AddMove (Position theseusOld, Position theseusNew, Position minotaurOld, Position minotaurNew, TimeSpan timeSpan )
        {
            bool theseusMoved = !theseusOld.Equals(theseusNew);
            bool minotaurMoved = !minotaurOld.Equals(minotaurNew);
            Move move = new Move { GameEnded = HasTheseusWon || HasMinotaurWon, TheseusMoved = theseusMoved, MinotaurMoved = minotaurMoved, Time = timeSpan };
            if (theseusMoved)
            {
                move.TheseusStart = theseusOld;
                move.TheseusEnd = theseusNew;
            }if (minotaurMoved)
            {
                move.MinotaurStart = minotaurOld;
                move.MinotaurEnd = minotaurNew;
            }
            Current.AllMoves.Add(move);
            int count = Current.AllMoves.Count;
        }
    }
}
