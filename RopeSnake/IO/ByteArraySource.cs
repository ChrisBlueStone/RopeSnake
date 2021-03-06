﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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

        public ByteArraySource(string filePath) : this(File.ReadAllBytes(filePath)) { }

        public ByteArraySource(int size) : this(new byte[size]) { }

        public ByteArraySource(byte[] array)
        {
            _array = array;
        }

        public byte GetByte(int offset)
        {
            return _array[offset];
        }

        public void SetByte(int offset, byte value)
        {
            _array[offset] = value;
        }
    }
}
