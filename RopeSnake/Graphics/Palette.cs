using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.Graphics
{
    public class Palette : IEnumerable<Color>
    {
        protected Color[,] _colors;

        public int PaletteCount { get; protected set; }
        public int ColorCount { get; protected set; }

        public Palette(int paletteCount, int colorCount)
        {
            _colors = new Color[paletteCount, colorCount];
            PaletteCount = paletteCount;
            ColorCount = colorCount;
        }

        public virtual Color GetColor(int paletteIndex, int colorIndex) => _colors[paletteIndex, colorIndex];

        public virtual void SetColor(int paletteIndex, int colorIndex, Color value) => _colors[paletteIndex, colorIndex] = value;

        public IEnumerator<Color> GetEnumerator()
        {
            for (int i = 0; i < PaletteCount; i++)
                for (int j = 0; j < ColorCount; j++)
                    yield return _colors[i, j];
        }

        public IEnumerable<Color> EnumeratePalette(int paletteIndex)
        {
            for (int i = 0; i < ColorCount; i++)
                yield return _colors[paletteIndex, i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
