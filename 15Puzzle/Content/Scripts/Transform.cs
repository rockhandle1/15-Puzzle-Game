using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _15Puzzle
{
    internal class Transform
    {
        protected Vector2 Position = Vector2.Zero;
        protected Vector2 Scale = new Vector2(0.9f, 0.9f);
        public Vector2 position
        {
            get { return Position; }
            set { Position = value; }
        }
        public Vector2 scale
        {
            get { return Scale; }
            set { Scale = value; }
        }
    }
}
