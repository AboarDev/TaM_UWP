namespace TaM
{
    public class Square
    {
        public bool Top;
        public bool Right;
        public bool Bottom;
        public bool Left;
        public bool Minotaur;
        public bool Theseus;
        public bool Exit;

        public Square(bool top, bool left, bool bottom, bool right, bool hasMinotuar,
bool hasTheseus, bool isExit)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
            Minotaur = hasMinotuar;
            Theseus = hasTheseus;
            Exit = isExit;
        }
    }
}