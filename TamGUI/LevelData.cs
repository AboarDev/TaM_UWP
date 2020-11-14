using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamLib;
using Newtonsoft.Json;

namespace TamGui
{
    public class LevelData
    {
        [JsonConstructor]
        public LevelData (string name, int width, int height, string schema, List<Move> moves)
        {
            Name = name;
            Width = width;
            Height = height;
            Schema = schema;
            Moves = moves;
        }

        public LevelData ()
        {
        
        }

        public LevelData(string name, int width, int height, string schema)
        {
            Name = name;
            Width = width;
            Height = height;
            Schema = schema;
            //Moves = null;
        }

        public LevelData(Level level,Position theseus, Position minotaur, Position exit)
        {
            Name = level.Name;
            Width = level.Width;
            Height = level.Height;
            //Moves = level.AllMoves;
            Schema = $"{minotaur} {theseus} {exit} {level.Data.Remove(0, 15)}";
        }

        public string Name { get; set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        public string Schema { get; private set; }
        public List<Move> Moves { get; set; }
    }
}
