using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.Graphics
{
    public struct Color : IEquatable<Color>
    {
        public readonly int Argb;

        public byte B { get { return (byte)(Argb & 0xFF); } }

        public byte G { get { return (byte)((Argb >> 8) & 0xFF); } }

        public byte R { get { return (byte)((Argb >> 16) & 0xFF); } }

        public byte A { get { return (byte)((Argb >> 24) & 0xFF); } }

        public Color(int argb)
        {
            Argb = argb;
        }

        public Color(byte r, byte g, byte b) : this(r, g, b, 255) { }

        public Color(byte r, byte g, byte b, byte a)
        {
            Argb = b | (g << 8) | (r << 16) | (a << 24);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType())
                return false;

            Color other = (Color)obj;
            return Equals(other);
        }

        public override int GetHashCode() => Argb.GetHashCode();

        public bool Equals(Color other) => Argb == other.Argb;

        public static bool operator ==(Color left, Color right) => left.Equals(right);

        public static bool operator !=(Color left, Color right) => !left.Equals(right);
    }
}
