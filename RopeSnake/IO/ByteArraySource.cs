using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace RopeSnake.IO
{
    public sealed class ByteArraySource : Source
    {
        private byte[] _array;

        public override int Length
        {
            get { return _array.Length; }
        }

        public ByteArraySource(byte[] array)
        {
            _array = array;
        }

        public ByteArraySource(int size)
        {
            _array = new byte[size];
        }

        public override byte ReadByte(int offset)
        {
            return _array[offset];
        }

        public override void WriteByte(int offset, byte value)
        {
            _array[offset] = value;
        }

        public string ComputeSHA256() => ComputeSHA256(0, _array.Length);

        public string ComputeSHA256(int offset, int count)
        {
            SHA256Managed sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(_array, offset, count);
            return string.Join("", hash.Select(b => b.ToString("x2")));
        }
    }
}
