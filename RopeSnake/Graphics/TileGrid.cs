using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.Graphics
{
    public class TileGrid : IEnumerable<TileProperties>
    {
        protected TileProperties[,] _elements;

        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public int TileWidth { get; protected set; }
        public int TileHeight { get; protected set; }

        public virtual TileProperties this[int x, int y]
        {
            get { return _elements[x, y]; }
            set { _elements[x, y] = value; }
        }

        protected TileGrid(int width, int height, int tileWidth, int tileHeight)
        {
            _elements = new TileProperties[width, height];
            Width = width;
            Height = height;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
        }

        public static TileGrid Create(int width, int height, int tileWidth, int tileHeight) =>
            Create<TileProperties>(width, height, tileWidth, tileHeight);

        public static TileGrid Create<T>(int width, int height, int tileWidth, int tileHeight) where T : TileProperties =>
            Create(width, height, tileWidth, tileHeight, () => new TileProperties());

        public static TileGrid Create<T>(int width, int height, int tileWidth, int tileHeight, Func<T> tileGridElementCreator)
            where T : TileProperties
        {
            TileGrid map = new TileGrid(width, height, tileWidth, tileHeight);

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    map._elements[x, y] = tileGridElementCreator();

            return map;
        }

        public virtual TileProperties GetElement(int x, int y) => _elements[x, y];

        public virtual void SetElement(int x, int y, TileProperties value) => _elements[x, y] = value;

        public virtual IEnumerator<TileProperties> GetEnumerator()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    yield return _elements[x, y];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
