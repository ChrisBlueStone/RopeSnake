using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace RopeSnake.IO
{
    public sealed class ByteArraySource : ISource
    {
        private byte[] _array;

        public int Length
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

        public byte GetByte(int offset)
        {
            return _array[offset];
        }

        public void SetByte(int offset, byte value)
        {
            _array[offset] = value;
        }

        public string ComputeSHA256(int offset, int count)
        {
            SHA256Managed sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(_array, offset, count);
            return string.Join("", hash.Select(b => b.ToString("x2")));
        }
    }
}
