using System.IO;

namespace GT1Decompress
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }

            using (FileStream input = new FileStream(args[0], FileMode.Open))
            {
                using (FileStream output = new FileStream(args[0] + "_decompressed", FileMode.Create))
                {
                    Decompress(input, output);
                }
            }
        }

        static void Decompress(Stream compressed, Stream output)
        {
            byte ch;
            byte Temp;
            byte Temp1;
            byte Temp2;
            int i;
            int jump;
            int size;
            int flag;
            byte[] b = new byte[258];
            long Temp_Offset;
            long Last_Offset;

            flag = 1;
            compressed.Position = 0;

            while (compressed.Position < compressed.Length)
            {
                if (flag == 1)
                {
                    Temp = (byte)compressed.ReadByte();
                    flag = Temp | 0x100;
                }
                ch = (byte)compressed.ReadByte();

                if ((flag & 1) != 1)
                {
                    output.WriteByte(ch);
                }
                else
                {
                    Temp1 = (byte)compressed.ReadByte();
                    if ((Temp1 & 0x80) != 0x80)
                    {
                        jump = (Temp1 & 0x7F) + 1;
                    }
                    else
                    {
                        Temp2 = (byte)compressed.ReadByte();
                        jump = ((Temp1 & 0x7F) * 256) + Temp2 + 1;
                    }
                    size = ch + 3;
                    Temp_Offset = output.Position;
                    output.Position = output.Position - jump;
                    Last_Offset = output.Position;
                    for (i = 0; i < size; i++)
                    {
                        b[i] = (byte)output.ReadByte();
                        if (output.Position >= output.Length)
                        {
                            output.Position = Last_Offset;
                        }
                    }
                    output.Position = Temp_Offset;
                    output.Write(b, 0, size);
                }
                flag = flag >> 1;
            }
        }
    }
}
