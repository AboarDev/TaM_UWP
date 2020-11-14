using System;
using System.Collections.Generic;
using System.Text;

namespace TamLib
{
    public struct Position
    {
        //similar to https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct
        public Position(int y, int x)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString() => $"{Y,0:D2}{X,0:D2}";
    }
}
