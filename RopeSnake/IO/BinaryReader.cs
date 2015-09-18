using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.IO
{
    public class BinaryReader : IBinaryReader
    {
        private ISource source;

        private Func<ushort> ushortReader;
        private Func<uint> uintReader;

        public int Position { get; set; }

        public BinaryReader(ISource source) : this(source, false) { }

        public BinaryReader(ISource source, bool bigEndian)
        {
            this.source = source;

            if (bigEndian == true)
            {
                ushortReader = ReadUShortBigEndian;
                uintReader = ReadUIntBigEndian;
            }
            else
            {
                ushortReader = ReadUShortLittleEndian;
                uintReader = ReadUIntLittleEndian;
            }
        }

        public byte[] ReadByteArray(int size)
        {
            byte[] bytes = new byte[size];
            for (int i = 0; i < size; i++)
                bytes[i] = source.GetByte(Position++);
            return bytes;
        }

        public ByteArraySource ReadByteArraySource(int size)
        {
            return new ByteArraySource(ReadByteArray(size));
        }

        public byte ReadByte() => source.GetByte(Position++);

        public sbyte ReadSByte() => (sbyte)ReadByte();

        public ushort ReadUShort() => ushortReader();

        private ushort ReadUShortLittleEndian() => (ushort)(ReadByte() | (ReadByte() << 8));

        private ushort ReadUShortBigEndian() => (ushort)((ReadByte() << 8) | ReadByte());

        public short ReadShort() => (short)ReadUShort();

        public uint ReadUInt() => uintReader();

        private uint ReadUIntLittleEndian() => (uint)(ReadByte() | (ReadByte() << 8) |
                (ReadByte() << 16) | (ReadByte() << 24));

        private uint ReadUIntBigEndian() => (uint)((ReadByte() << 24) | (ReadByte() << 16) |
            (ReadByte() << 8) | ReadByte());

        public int ReadInt() => (int)ReadUInt();

        public int ReadInt(int offset) => (int)ReadUInt();

        public string ReadString()
        {
            StringBuilder sb = new StringBuilder();

            byte ch;
            while ((ch = ReadByte()) != 0)
            {
                sb.Append((char)ch);
            }

            return sb.ToString();
        }

        public string ReadString(int maxLength)
        {
            StringBuilder sb = new StringBuilder(maxLength);

            byte ch;
            int counter = 0;
            int oldPosition = Position;

            while ((ch = ReadByte()) != 0 && (counter++ < maxLength))
            {
                sb.Append((char)ch);
            }

            // Ensure that the position is always incremented by maxLength
            Position = oldPosition + maxLength;

            return sb.ToString();
        }
    }
}
