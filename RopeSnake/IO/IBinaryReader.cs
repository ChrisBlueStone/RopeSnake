namespace RopeSnake.IO
{
    public interface IBinaryReader
    {
        int Position { get; set; }
        byte[] ReadByteArray(int size);
        ByteArraySource ReadByteArraySource(int size);

        int ReadInt();
        byte ReadByte();
        sbyte ReadSByte();
        short ReadShort();
        string ReadString();
        string ReadString(int maxLength);
        uint ReadUInt();
        ushort ReadUShort();

        int PeekInt();
        byte PeekByte();
        sbyte PeekSByte();
        short PeekShort();
        uint PeekUInt();
        ushort PeekUShort();
    }
}