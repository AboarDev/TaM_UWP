using System;
using System.Collections.Generic;
using System.Text;

namespace TaM
{
    public struct Position
    {
        //similar to https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct
        public Position(int y, int x)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public override string ToString() => $"({X}, {Y})";
    }
}
