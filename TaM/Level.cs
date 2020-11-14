using System.Collections.Generic;
using System;

namespace TaM
{
    class Level
    {
        public int Moves;
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
        }

        public void MakeLevel () {
            int i;
            int len = 4;
            int count = 0;
            Dictionary<string, int> minotaur = new Dictionary<string, int>();
            Dictionary<string, int> theseus = new Dictionary<string, int>();
            Dictionary<string, int> exit = new Dictionary<string, int>();
            Func<Dictionary<string, int>, string, int> GetKey = (dict, key) => (dict.TryGetValue(key, out int val)) ? val : 0;
            Func<string, int> GetInt = inp => (int.TryParse(inp, out int val)) ? val : 0;
            for (i = 0; i < 15; i += 5)
            {
                //minotaur, theseus, exit
                //row
                string theSubstring;
                theSubstring = Data.Substring(i, len / 2);
                int posY = GetInt(theSubstring);
                //column
                theSubstring = Data.Substring(i + len / 2, len / 2);
                int posX = GetInt(theSubstring);
                switch (count)
                {
                    case 0:
                        //0 = y, 1 = x
                        minotaur["Y"] = posY;
                        minotaur["X"] = posX;
                        break;
                    case 1:
                        theseus["Y"] = posY;
                        theseus["X"] = posX;
                        break;
                    case 2:
                        exit["Y"] = posY;
                        exit["X"] = posX;
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
                bool isMinotaur = (y == GetKey(minotaur,"Y") && x == GetKey(minotaur, "X")) ? true : false;
                bool isTheseus = (y == GetKey(theseus, "Y") && x == GetKey(theseus, "X")) ? true : false;
                bool isExit = (y == GetKey(exit, "Y") && x == GetKey(exit, "X")) ? true : false;
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

        public Square AtPosition (int y, int x)
        {
            if (y < Height && x < Width)
            {
                return Squares[y,x];
            }
            return null;
        }

        private Position FindTME(bool minotaur, bool theseus, bool exit)
        {
            for(int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Square theSquare = Squares[y, x];
                    if (minotaur && Squares[y, x].Minotaur)
                    {
                        return new Position(y, x);
                    }
                    if (exit && Squares[y, x].Exit)
                    {
                        return new Position(y, x);
                    }
                    if (theSquare != null && theseus && Squares[y, x].Theseus)
                    {
                        return new Position(y, x);
                    }
                    if (theSquare == null)
                    {
                        Console.WriteLine(y);
                        Console.WriteLine(x);
                    }
                }
            }
            return new Position();
        }

        public int TheseusRow { get => FindTME(false,true,false).Y; }
        public int TheseusColumn { get => FindTME(false, true, false).X; }
        public int MinotuarRow { get => FindTME(true, false, false).Y; }
        public int MinitaurColumn { get => FindTME(true, false, false).X; }
        public int ExitRow { get => FindTME(false, false, true).Y; }
        public int ExitColumn { get => FindTME(false, false, true).X; }
        public int MoveCount { get => Moves; }
        public void MoveTheseus(Directions direction)
        {
            int MoveX = 0;
            int MoveY = 0;
            switch (direction)
            {
                case Directions.UP:
                    MoveX = 0;
                    MoveY = -1;
                    break;
                case Directions.DOWN:
                    MoveX = 0;
                    MoveY = 1;
                    break;
                case Directions.LEFT:
                    MoveX = -1;
                    MoveY = 0;
                    break;
                case Directions.RIGHT:
                    MoveX = 1;
                    MoveY = 0;
                    break;
            }
            Square oldPos = Squares[TheseusRow, TheseusColumn];
            Square newPos = Squares[TheseusRow + MoveY, TheseusColumn + MoveX];
            bool canMove = CanMove(true,direction,oldPos,newPos);
            if (canMove)
            {
                oldPos.Theseus = false;
                newPos.Theseus = true;
                Moves++;
            }
        }
        public void Pause ()
        {
            Moves++;
        }
        private bool CanMove(bool isTheseus, Directions direction, Square oldPos, Square newPos)
        {
            bool canMove = false;
            switch (direction)
            {
                case Directions.UP:
                    if (!(oldPos.Top || newPos.Bottom))
                    {
                        canMove = true;
                    }
                    break;
                case Directions.DOWN:
                    if (!(oldPos.Bottom || newPos.Top))
                    {
                        canMove = true;
                    }
                    break;
                case Directions.LEFT:
                    if (!(oldPos.Left || newPos.Right))
                    {
                        canMove = true;
                    }
                    break;
                case Directions.RIGHT:
                    if (!(oldPos.Right || newPos.Left))
                    {
                        canMove = true;
                    }
                    break;
            }
            return canMove;
        }
        public void MoveMinotaur()
        {
            Square theseusPos = Squares[TheseusRow, TheseusColumn];
            Square minotaurPos = Squares[MinotuarRow, MinitaurColumn];
            int diffY = TheseusRow - MinotuarRow; // rows, y axis
            int diffX = TheseusColumn - MinitaurColumn; // columns, x axis
            int toMove;
            bool canMove = true;
            Directions direction;
            if ((diffX != 0 && diffY != 0) || diffX != 0)
            {
                toMove = diffX > 0 ? 1 : -1;
                direction = diffX > 0 ? Directions.RIGHT : Directions.LEFT;
                Square newMinotaurPos = Squares[MinotuarRow, MinitaurColumn + toMove];
                canMove = CanMove(false, direction, minotaurPos, newMinotaurPos);
                if (canMove)
                {
                    newMinotaurPos.Minotaur = true;
                    minotaurPos.Minotaur = false;
                }
                else if (!canMove && diffY != 0)
                {
                    toMove = diffY > 0 ? 1 : -1;
                    direction = diffY > 0 ? Directions.DOWN : Directions.UP;
                    newMinotaurPos = Squares[MinotuarRow + toMove, MinitaurColumn];
                    canMove = CanMove(false, direction, minotaurPos, newMinotaurPos);
                    if (canMove)
                    {
                        newMinotaurPos.Minotaur = true;
                        minotaurPos.Minotaur = false;
                    }

                }
            }
            else if (diffY != 0)
            {
                toMove = diffY > 0 ? 1 : -1;
                direction = diffY > 0 ? Directions.DOWN : Directions.UP;
                Square newMinotaurPos = Squares[MinotuarRow + toMove, MinitaurColumn];
                canMove = CanMove(false, direction, minotaurPos, newMinotaurPos);
                if (canMove)
                {
                    newMinotaurPos.Minotaur = true;
                    minotaurPos.Minotaur = false;
                }
            }
        }

        //public void MoveMinotaur()
        //{
        //    Square theseusPos = Squares[TheseusRow, TheseusColumn];
        //    Square minotaurPos = Squares[MinotuarRow, MinitaurColumn];
        //    int diffY = TheseusRow - MinotuarRow; // rows, y axis
        //    int diffX = TheseusColumn - MinitaurColumn; // columns, x axis
        //    int toMove;
        //    bool canMove = true;
        //    Directions direction;
        //    Square newMinotaurPos = null;
        //    if ((diffX != 0 && diffY != 0) || diffX != 0)
        //    {
        //        toMove = diffX > 0 ? 1 : -1;
        //        direction = diffX > 0 ? Directions.RIGHT : Directions.LEFT;
        //        newMinotaurPos = Squares[MinotuarRow, MinitaurColumn + toMove];
        //        canMove = CanMove(false, direction, minotaurPos, newMinotaurPos);
        //        if (!canMove && diffY != 0)
        //        {
        //            toMove = diffY > 0 ? 1 : -1;
        //            direction = diffY > 0 ? Directions.DOWN : Directions.UP;
        //            newMinotaurPos = Squares[MinotuarRow + toMove, MinitaurColumn];
        //            canMove = CanMove(false, direction, minotaurPos, newMinotaurPos);

        //        }
        //    }
        //    else if (diffY != 0)
        //    {
        //        toMove = diffY > 0 ? 1 : -1;
        //        direction = diffY > 0 ? Directions.DOWN : Directions.UP;
        //        newMinotaurPos = Squares[MinotuarRow + toMove, MinitaurColumn];
        //        canMove = CanMove(false, direction, minotaurPos, newMinotaurPos);
        //    }
        //    if (canMove)
        //    {
        //        newMinotaurPos.Minotaur = true;
        //        minotaurPos.Minotaur = false;
        //    }
        //}

        public override string ToString()
        {
            return Name;
        }
    }
}
