using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexocracy
{
    [Serializable]
    public struct Index2D
    {
        public static implicit operator int(Index2D index)
        {
            return index.GetHashCode();
        }

        private const int Y_FACTOR = 1000;

        private int x;
        private int y;
        private int hashCode;

        public int X { get { return x; } }

        public int Y { get { return y; } }

        public Index2D(int x, int y)
        {
            this.x = x;
            this.y = y;
            hashCode = x + y * 1000;
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return obj is Index2D && GetHashCode() == obj.GetHashCode();
        }

        public Index2D Offset(int x, int y)
        {
            return new Index2D(X + x, Y + y);
        }

        public override string ToString()
        {
            return "X = " + x + " Y = " + y;
        }
    }
}
