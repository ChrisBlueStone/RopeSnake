using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.Graphics
{
    public class Tile : IEnumerable<byte>
    {
        protected byte[,] _pixels;

        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public virtual byte this[int x, int y]
        {
            get { return _pixels[x, y]; }
            set { _pixels[x, y] = value; }
        }

        public Tile(int width, int height)
        {
            _pixels = new byte[width, height];
            Width = width;
            Height = height;
        }

        public virtual IEnumerator<byte> GetEnumerator()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    yield return _pixels[x, y];
        }

        public virtual byte GetPixel(int x, int y) => this[x, y];

        public virtual void SetPixel(int x, int y, byte value) => this[x, y] = value;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
