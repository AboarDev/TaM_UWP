using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamLib
{
    public struct Move
    {
        public bool GameEnded { get; set; }
        public bool MinotaurMoved { get; set; }
        public bool TheseusMoved { get; set; }
        public Position TheseusStart { get; set; }
        public Position TheseusEnd { get; set; }
        public Position MinotaurStart { get; set; }
        public Position MinotaurEnd { get; set; }
        public TimeSpan Time { get; set; }
    }
}
