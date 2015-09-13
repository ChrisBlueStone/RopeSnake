using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.Graphics
{
    public class TileSet : IEnumerable<Tile>
    {
        protected Tile[] _tiles;

        public virtual int Length { get; }

        public virtual Tile this[int index]
        {
            get { return _tiles[index]; }
            set { _tiles[index] = value; }
        }

        protected TileSet(int length)
        {
            _tiles = new Tile[length];
            Length = length;
        }

        public virtual Tile GetTile(int index) => _tiles[index];

        public virtual void SetTile(int index, Tile value) => _tiles[index] = value;

        public static TileSet Create(int width, int height, int length) => Create(width, height, length, () => new Tile(width, height));

        public static TileSet Create<T>(int width, int height, int length, Func<T> tileCreator) where T : Tile
        {
            TileSet tileSet = new TileSet(length);

            for (int i = 0; i < length; i++)
                tileSet._tiles[i] = tileCreator();

            return tileSet;
        }

        public virtual IEnumerator<Tile> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
                yield return _tiles[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
