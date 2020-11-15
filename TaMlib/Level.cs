using System.Collections.Generic;
using System;

namespace TamLib
{
    public class Level
    {
        public string Name;
        public int Width;
        public int Height;
        public string Data;
        public Square[,] Squares;
        public Level(string name, int width, int height, string data)
        {
            Name = name;
            Width = width;
            Height = height;
            Data = data;
            Squares = new Square[Height,Width];
            AllMoves = new List<Move>();
        }

        public void MakeLevel () {
            int i;
            int len = 4;
            int count = 0;
            Func<string, int> GetInt = inp => (int.TryParse(inp, out int val)) ? val : 0;
            for (i = 0; i < 15; i += 5)
            {
                // 1: minotaur, 2: theseus, 3: exit.
                // row.
                string theSubstring;
                theSubstring = Data.Substring(i, len / 2);
                int posY = GetInt(theSubstring);
                // column.
                theSubstring = Data.Substring(i + len / 2, len / 2);
                int posX = GetInt(theSubstring);
                switch (count)
                {
                    case 0:
                        // remove findTME 3 new lines (can be removed)
                        // 0 = y, 1 = x.
                        MinotaurPosition = new Position(posY, posX);
                        break;
                    case 1:
                        TheseusPosition = new Position(posY, posX);
                        break;
                    case 2:
                        ExitPosition = new Position(posY, posX);
                        break;
                }
                count++;
            }
            int y = 0;
            int x = 0;
            for (int n = i; n < Data.Length; n += 5)
            {
                bool Top = Convert.ToBoolean(GetInt(Data.Substring(n, 1)));
                bool Right = Convert.ToBoolean(GetInt(Data.Substring(n + 1, 1)));
                bool Bottom = Convert.ToBoolean(GetInt(Data.Substring(n + 2, 1)));
                bool Left = Convert.ToBoolean(GetInt(Data.Substring(n + 3, 1)));
                bool isMinotaur = (y == MinotaurPosition.Y && x == MinotaurPosition.X) ? true : false;
                bool isTheseus = (y == TheseusPosition.Y && x == TheseusPosition.X) ? true : false;
                bool isExit = (y == ExitPosition.Y && x == ExitPosition.X) ? true : false;
                Squares[y,x] = new Square(Top, Left, Bottom, Right, isMinotaur, isTheseus, isExit);
                if (x == Squares.GetLength(1) - 1 && y < Squares.GetLength(0) - 1)
                {
                    y++;
                    x = 0;
                    continue;
                }
                if (x < Squares.GetLength(1) -1 )
                {
                    x++;
                }
            }
        }

        public Square AtPosition (Position position)
        {
            if (position.Y < Height && position.X < Width && position.Y >= 0 && position.X >= 0)
            {
                return Squares[position.Y, position.X];
            }
            return null;
        }
        
        public List<Move> AllMoves { get; set; }
        public Position TheseusPosition { get; set; }
        public Position MinotaurPosition { get; set; }
        public Position ExitPosition { get; set; }
        public int MoveCount { get; set; }

        public void SetPositions (Move move)
        {
            if (move.TheseusMoved)
            {
                Square theseusPosStart = Squares[move.TheseusStart.Y,move.TheseusStart.X];
                Square theseusPosEnd = Squares[move.TheseusEnd.Y, move.TheseusEnd.X];
                theseusPosStart.Theseus = true;
                theseusPosEnd.Theseus = false;
            }
            if (move.MinotaurMoved)
            {
                Square minotaurPosStart = Squares[move.MinotaurStart.Y, move.MinotaurStart.X];
                Square minotaurPosEnd = Squares[move.MinotaurEnd.Y, move.MinotaurEnd.X];
                minotaurPosStart.Minotaur = true;
                minotaurPosEnd.Minotaur = false;
            }
        }
        public bool MoveTheseus(Moves direction)
        {
            int MoveX = 0;
            int MoveY = 0;
            switch (direction)
            {
                case Moves.UP:
                    MoveX = 0;
                    MoveY = -1;
                    break;
                case Moves.DOWN:
                    MoveX = 0;
                    MoveY = 1;
                    break;
                case Moves.LEFT:
                    MoveX = -1;
                    MoveY = 0;
                    break;
                case Moves.RIGHT:
                    MoveX = 1;
                    MoveY = 0;
                    break;
                case Moves.PAUSE:
                    MoveCount++;
                    return true;
            }
            Square oldPos = AtPosition(TheseusPosition);
            // fixed nullpointerexception cause by trying to access out of range cell from original model
            bool canMove = false;
            // 1 new line
            Position position = new Position(TheseusPosition.Y + MoveY, TheseusPosition.X + MoveX);
            Square newPos = AtPosition(position);
            if (newPos != null)
            {
                // Can query in range
                canMove = CanMove(true, direction, oldPos, newPos);
            }
            if (canMove)
            {
                oldPos.Theseus = false;
                newPos.Theseus = true;
                MoveCount++;
                // 1 new line
                TheseusPosition = position;
                return true;
            }
            return false;
        }
        private bool CanMove(bool isTheseus, Moves direction, Square oldPos, Square newPos)
        {
            switch (direction)
            {
                case Moves.UP:
                    if (!(oldPos.Top || newPos.Bottom))
                    {
                        return true;
                    }
                    break;
                case Moves.DOWN:
                    if (!(oldPos.Bottom || newPos.Top))
                    {
                        return true;
                    }
                    break;
                case Moves.LEFT:
                    if (!(oldPos.Left || newPos.Right))
                    {
                        return true;
                    }
                    break;
                case Moves.RIGHT:
                    if (!(oldPos.Right || newPos.Left))
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }
        public void MoveMinotaur()
        {
            Square theseusPos = AtPosition(TheseusPosition);
            Square minotaurPos = AtPosition(MinotaurPosition);
            int diffY = TheseusPosition.Y - MinotaurPosition.Y; // rows, y axis
            int diffX = TheseusPosition.X - MinotaurPosition.X; // columns, x axis
            bool canMove = true;
            Moves direction;
            Square newMinotaurPos = null;
            // 1 new line
            Position position = new Position();
            if ((diffX != 0 && diffY != 0) || diffX != 0)
            {
                int toMove = diffX > 0 ? 1: -1;
                direction = diffX > 0 ? Moves.RIGHT : Moves.LEFT;
                // 1 new line
                position = new Position(MinotaurPosition.Y, MinotaurPosition.X + toMove);
                newMinotaurPos = Squares[MinotaurPosition.Y, MinotaurPosition.X + toMove];
                canMove = CanMove(false, direction, minotaurPos, newMinotaurPos);
                if (!canMove && diffY != 0)
                {
                    toMove = diffY > 0 ? 1 : -1;
                    direction = diffY > 0 ? Moves.DOWN : Moves.UP;
                    // 1 new line
                    position = new Position(MinotaurPosition.Y + toMove, MinotaurPosition.X);
                    newMinotaurPos = Squares[MinotaurPosition.Y + toMove, MinotaurPosition.X];
                    canMove = CanMove(false, direction, minotaurPos, newMinotaurPos);

                }
            }
            else if (diffY != 0)
            {
                int toMove = diffY > 0 ? 1 : -1;
                direction = diffY > 0 ? Moves.DOWN : Moves.UP;
                position = new Position(MinotaurPosition.Y + toMove, MinotaurPosition.X);
                newMinotaurPos = Squares[MinotaurPosition.Y + toMove, MinotaurPosition.X];
                canMove = CanMove(false, direction, minotaurPos, newMinotaurPos);
            }
            if (canMove)
            {
                // 1 new line
                MinotaurPosition = position;
                newMinotaurPos.Minotaur = true;
                minotaurPos.Minotaur = false;
            }
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
