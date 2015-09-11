using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.IO;

namespace RopeSnake.Gba
{
    internal static class Lz77
    {
        internal static byte[] DecompLZ77(Source source, int address)
        {
            int temp;
            return DecompLZ77(source, address, out temp);
        }

        internal static byte[] DecompLZ77(Source source, int address, out int bytesRead)
        {
            int start = address;

            // Check for LZ77 signature
            if (source.ReadByte(address++) != 0x10)
                throw new Lz77Exception("The LZ77 header was missing.");

            // Read the block length
            int length = source.ReadByte(address++);
            length += (source.ReadByte(address++) << 8);
            length += (source.ReadByte(address++) << 16);
            byte[] output = new byte[length];

            int bPos = 0;
            while (bPos < length)
            {
                byte ch = source.ReadByte(address++);
                for (int i = 0; i < 8; i++)
                {
                    switch ((ch >> (7 - i)) & 1)
                    {
                        case 0:

                            // Direct copy
                            if (bPos >= length) break;
                            output[bPos++] = source.ReadByte(address++);
                            break;

                        case 1:

                            // Compression magic
                            int t = (source.ReadByte(address++) << 8);
                            t += source.ReadByte(address++);
                            int n = ((t >> 12) & 0xF) + 3;    // Number of bytes to copy

                            int o = (t & 0xFFF);

                            // Copy n bytes from bPos-o to the output
                            for (int j = 0; j < n; j++)
                            {
                                if (bPos >= length) break;
                                output[bPos] = output[bPos - o - 1];
                                bPos++;
                            }

                            break;

                        default:
                            break;
                    }
                }
            }

            bytesRead = address - start;
            return output;
        }

        internal static byte[] CompLZ77(byte[] data, bool vram)
        {
            return CompLZ77(data, 0, data.Length, vram);
        }

        internal static byte[] CompLZ77(byte[] data, int address, int length, bool vram)
        {
            int start = address;

            List<byte> obuf = new List<byte>();
            List<byte> tbuf = new List<byte>();
            int control = 0;

            // Let's start by encoding the signature and the length
            obuf.Add(0x10);
            obuf.Add((byte)(length & 0xFF));
            obuf.Add((byte)((length >> 8) & 0xFF));
            obuf.Add((byte)((length >> 16) & 0xFF));

            // VRAM bug: you can't reference the previous byte
            int distanceStart = vram ? 2 : 1;

            while ((address - start) < length)
            {
                tbuf.Clear();
                control = 0;
                for (int i = 0; i < 8; i++)
                {
                    bool found = false;

                    // First byte should be raw
                    if (address == start)
                    {
                        tbuf.Add(data[address++]);
                        found = true;
                    }
                    else if ((address - start) >= length)
                    {
                        break;
                    }
                    else
                    {
                        // We're looking for the longest possible string
                        // The farthest possible distance from the current address is 0x1000
                        int max_length = -1;
                        int max_distance = -1;

                        for (int k = distanceStart; k <= 0x1000; k++)
                        {
                            if ((address - k) < start) break;

                            int l = 0;
                            for (; l < 18; l++)
                            {
                                if (((address - start + l) >= length) ||
                                    (data[address - k + l] != data[address + l]))
                                {
                                    if (l > max_length)
                                    {
                                        max_length = l;
                                        max_distance = k;
                                    }
                                    break;
                                }
                            }

                            // Corner case: we matched all 18 bytes. This is
                            // the maximum length, so don't bother continuing
                            if (l == 18)
                            {
                                max_length = 18;
                                max_distance = k;
                                break;
                            }
                        }

                        if (max_length >= 3)
                        {
                            address += max_length;

                            // We hit a match, so add it to the output
                            int t = (max_distance - 1) & 0xFFF;
                            t |= (((max_length - 3) & 0xF) << 12);
                            tbuf.Add((byte)((t >> 8) & 0xFF));
                            tbuf.Add((byte)(t & 0xFF));

                            // Set the control bit
                            control |= (1 << (7 - i));

                            found = true;
                        }
                    }

                    if (!found)
                    {
                        // If we didn't find any strings, copy the byte to the output
                        tbuf.Add(data[address++]);
                    }
                }

                // Flush the temp buffer
                obuf.Add((byte)(control & 0xFF));
                obuf.AddRange(tbuf.ToArray());
            }

            return obuf.ToArray();
        }
    }

    public class Lz77Exception : Exception
    {
        public Lz77Exception(string message) : base(message) { }
    }
}
