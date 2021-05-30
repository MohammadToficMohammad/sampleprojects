using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NioStream.Defaults.Helpers
{
    public static class Helpers
    {





        public static byte[] addByteToArrayHead(byte[] bArray, byte newByte)
        {
            byte[] newArray = new byte[bArray.Length + 1];
            bArray.CopyTo(newArray, 1);
            newArray[0] = newByte;
            return newArray;
        }

        public static byte[] addByteToArrayTail(byte[] bArray, byte newByte)
        {
            byte[] newArray = new byte[bArray.Length + 1];
            bArray.CopyTo(newArray, 0);
            newArray[bArray.Length] = newByte;
            return newArray;
        }




        public static byte[] addBytesToArrayHead(byte[] bArray, byte[] newBytes)
        {
            byte[] newArray = new byte[bArray.Length + newBytes.Length];
            bArray.CopyTo(newArray, newBytes.Length);
            newBytes.CopyTo(newArray, 0);
            return newArray;
        }

        public static byte[] addBytesToArrayTail(byte[] bArray, byte[] newBytes)
        {
            byte[] newArray = new byte[bArray.Length + newBytes.Length];
            bArray.CopyTo(newArray, 0);
            newBytes.CopyTo(newArray, bArray.Length);
            return newArray;
        }

        public static T[] Subset<T>(this T[] array, int start, int count)
        {
            T[] result = new T[count];
            Array.Copy(array, start, result, 0, count);
            return result;
        }

        public static T[] RemoveSubset<T>(this T[] array, int start, int count)
        {
            T[] result = new T[array.Length-count];
            Array.Copy(array, 0, result, 0, start);
            Array.Copy(array, start+count, result, start, array.Length - count-start);
            return result;
        }

        public static IEnumerable<int> PatternAt(byte[] source, byte[] pattern)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
                {
                    yield return i;
                }
            }
        }




        //--------------------------------------TSLOGGING HELPERS------------------------------------------
        static readonly char[] HexdumpTable = new char[256 * 4];
        static readonly string Newline = "\n";
        static readonly string[] Byte2Hex = new string[256];
        static readonly string[] HexPadding = new string[16];
        static readonly string[] BytePadding = new string[16];
        static readonly char[] Byte2Char = new char[256];
        static readonly string[] HexDumpRowPrefixes = new string[(int)((uint)65536 >> 4)];
        static readonly string[] Byte2HexPad = new string[256];
        static readonly string[] Byte2HexNopad = new string[256];

        public static string ByteToHexStringPadded(int value) => Byte2HexPad[value & 0xff];


       










        public static string getByteAsBitsString(byte b)
        {
            return Convert.ToString(b, 2).PadLeft(8, '0');
        }





        static Helpers() {
            {


                int iii;
                for (iii = 0; iii < 10; iii++)
                {
                    var buf = new StringBuilder(2);
                    buf.Append('0');
                    buf.Append(iii);
                    Byte2HexPad[iii] = buf.ToString();
                    Byte2HexNopad[iii] = (iii).ToString();
                }
                for (; iii < 16; iii++)
                {
                    var buf = new StringBuilder(2);
                    char c = (char)('A' + iii - 10);
                    buf.Append('0');
                    buf.Append(c);
                    Byte2HexPad[iii] = buf.ToString();
                    Byte2HexNopad[iii] = c.ToString(); /* String.valueOf(c);*/
                }
                for (; iii < Byte2HexPad.Length; iii++)
                {
                    var buf = new StringBuilder(2);
                    buf.Append(iii.ToString("X") /*Integer.toHexString(i)*/);
                    string str = buf.ToString();
                    Byte2HexPad[iii] = str;
                    Byte2HexNopad[iii] = str;
                }



                char[] digits = "0123456789abcdef".ToCharArray();
                for (int i = 0; i < 256; i++)
                {
                    HexdumpTable[i << 1] = digits[(int)((uint)i >> 4 & 0x0F)];
                    HexdumpTable[(i << 1) + 1] = digits[i & 0x0F];
                }

                // Generate the lookup table for byte-to-hex-dump conversion
                for (int i = 0; i < Byte2Hex.Length; i++)
                {
                    Byte2Hex[i] = ' ' +ByteToHexStringPadded(i);
                }

                // Generate the lookup table for hex dump paddings
                for (int i = 0; i < HexPadding.Length; i++)
                {
                    int padding = HexPadding.Length - i;
                    var buf = new StringBuilder(padding * 3);
                    for (int j = 0; j < padding; j++)
                    {
                        buf.Append("   ");
                    }
                    HexPadding[i] = buf.ToString();
                }

                // Generate the lookup table for byte dump paddings
                for (int i = 0; i < BytePadding.Length; i++)
                {
                    int padding = BytePadding.Length - i;
                    var buf = new StringBuilder(padding);
                    for (int j = 0; j < padding; j++)
                    {
                        buf.Append(' ');
                    }
                    BytePadding[i] = buf.ToString();
                }

                // Generate the lookup table for byte-to-char conversion
                for (int i = 0; i < Byte2Char.Length; i++)
                {
                    if (i <= 0x1f || i >= 0x7f)
                    {
                        Byte2Char[i] = '.';
                    }
                    else
                    {
                        Byte2Char[i] = (char)i;
                    }
                }

                // Generate the lookup table for the start-offset header in each row (up to 64KiB).
                for (int i = 0; i < HexDumpRowPrefixes.Length; i++)
                {
                    var buf = new StringBuilder(12);
                    buf.Append(Environment.NewLine);
                    buf.Append((i << 4 & 0xFFFFFFFFL | 0x100000000L).ToString("X2"));
                    buf.Insert(buf.Length - 9, '|');
                    buf.Append('|');
                    HexDumpRowPrefixes[i] = buf.ToString();
                }
            }
        }
        public static string FormatByteArray(byte[] msg)
        {
          
            int length = msg.Length;
            if (length == 0)
            {
                return "EMPTY BUFFER LOGGED";
            }
            else
            {
                int rows = length / 16 + (length % 15 == 0 ? 0 : 1) + 4;
                var buf = new StringBuilder( 2 + 10 + 1 + 2 + rows * 80);

                buf.Append(": ").Append(length).Append('B').Append('\n');
                DoPrettyHexDump(buf, msg);

                return buf.ToString();
            }
        }

        static void AppendHexDumpRowPrefix(StringBuilder dump, int row, int rowStartIndex)
        {
            if (row < HexDumpRowPrefixes.Length)
            {
                dump.Append(HexDumpRowPrefixes[row]);
            }
            else
            {
                dump.Append(Environment.NewLine);
                dump.Append((rowStartIndex & 0xFFFFFFFFL | 0x100000000L).ToString("X2"));
                dump.Insert(dump.Length - 9, '|');
                dump.Append('|');
            }
        }
        public static void DoPrettyHexDump(StringBuilder dump,byte[] msg)
        {
           
            if (msg.Length == 0)
            {
             //   return "EMPTY BUFFER LOGGED";
            }
            var length = msg.Length;

            dump.Append(
                "               +-------------------------------------------------+" +
                Newline + "               |  0  1  2  3  4  5  6  7  8  9  a  b  c  d  e  f |" +
                Newline + "      +--------+-------------------------------------------------+----------------+");

            int startIndex = 0;
            int fullRows = (int)((uint)length >> 4);
            int remainder = length & 0xF;

            // Dump the rows which have 16 bytes.
            for (int row = 0; row < fullRows; row++)
            {
                int rowStartIndex = (row << 4) + startIndex;

                // Per-row prefix.
                AppendHexDumpRowPrefix(dump, row, rowStartIndex);

                // Hex dump
                int rowEndIndex = rowStartIndex + 16;
                for (int j = rowStartIndex; j < rowEndIndex; j++)
                {
                    dump.Append(Byte2Hex[msg[j]]);
                }
                dump.Append(" |");

                // ASCII dump
                for (int j = rowStartIndex; j < rowEndIndex; j++)
                {
                    dump.Append(Byte2Char[msg[j]]);
                }
                dump.Append('|');
            }

            // Dump the last row which has less than 16 bytes.
            if (remainder != 0)
            {
                int rowStartIndex = (fullRows << 4) + startIndex;
                AppendHexDumpRowPrefix(dump, fullRows, rowStartIndex);

                // Hex dump
                int rowEndIndex = rowStartIndex + remainder;
                for (int j = rowStartIndex; j < rowEndIndex; j++)
                {
                    dump.Append(Byte2Hex[msg[j]]);
                }
                dump.Append(HexPadding[remainder]);
                dump.Append(" |");

                // Ascii dump
                for (int j = rowStartIndex; j < rowEndIndex; j++)
                {
                    dump.Append(Byte2Char[msg[j]]);
                }
                dump.Append(BytePadding[remainder]);
                dump.Append('|');
            }

            dump.Append(Newline + "      +--------+-------------------------------------------------+----------------+");
        }

       
    }
    
}
