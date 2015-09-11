using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.IO
{
    public class BinaryReader : SourceReader
    {
        public BinaryReader(Source source) : base(source) { }

        public byte[] ReadByteArray(int size)
        {
            byte[] bytes = ReadByteArray(Position, size);
            Position += size;
            return bytes;
        }

        public byte[] ReadByteArray(int offset, int size)
        {
            byte[] bytes = new byte[size];
            for (int i = 0; i < size; i++)
                bytes[i] = ReadByte(offset++);
            return bytes;
        }

        public ByteArraySource ReadByteArraySource(int size)
        {
            ByteArraySource source = ReadByteArraySource(Position, size);
            Position += size;
            return source;
        }

        public ByteArraySource ReadByteArraySource(int offset, int size)
        {
            return new ByteArraySource(ReadByteArray(offset, size));
        }

        public sbyte ReadSByte() => ReadSByte(Position++);

        public sbyte ReadSByte(int offset) => (sbyte)ReadByte(offset);

        public ushort ReadUShort()
        {
            ushort value = ReadUShort(Position);
            Position += 2;
            return value;
        }

        public ushort ReadUShort(int offset)
        {
            return (ushort)(ReadByte(offset) | (ReadByte(offset + 1) << 8));
        }

        public short ReadShort() => (short)ReadUShort();

        public short ReadShort(int offset) => (short)ReadUShort(offset);

        public uint ReadUInt()
        {
            uint value = ReadUInt(Position);
            Position += 4;
            return value;
        }

        public uint ReadUInt(int offset)
        {
            return (uint)(ReadByte(offset) | (ReadByte(offset + 1) << 8) |
                (ReadByte(offset + 2) << 16) | (ReadByte(offset + 3) << 24));
        }

        public int ReadInt() => (int)ReadInt();

        public int ReadInt(int offset) => (int)ReadUInt(offset);

        public string ReadString()
        {
            string value = ReadString(Position);
            Position += value.Length + 1;
            return value;
        }

        public string ReadString(int offset)
        {
            StringBuilder sb = new StringBuilder();

            byte ch;
            while ((ch = ReadByte(offset++)) != 0)
            {
                sb.Append((char)ch);
            }

            return sb.ToString();
        }

        public string ReadString(int offset, int maxLength)
        {
            StringBuilder sb = new StringBuilder(maxLength);

            byte ch;
            int counter = 0;
            while ((ch = ReadByte(offset++)) != 0 && (counter++ < maxLength))
            {
                sb.Append((char)ch);
            }

            return sb.ToString();
        }
    }
}
