using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace StrEdit
{
    public partial class mainForm : Form
    {
        byte[] header;
        byte[][] indexes;
        byte[][] strings;
        byte[] footer;
        long stringCount;
        
        public mainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            FileStream rawStream;

            //rawStream = File.OpenRead("List_FamilyModel.str");
            //rawStream = File.OpenRead("List_CarMake.str");
            rawStream = File.OpenRead("Data_Car.str");
            //rawStream = File.OpenRead("Events.str");
            //rawStream = File.OpenRead("List_PartManufacturer.str");

            //byte[] header = new byte[rawStream.Length];
            header = new byte[0x2A];

            rawStream.Read(header, 0, 0x2A);

            byte[] rawStringCount = new byte[2];

            rawStream.Read(rawStringCount, 0, 2);

            Console.WriteLine("test");
            stringCount = rawStringCount[0] * 256 + rawStringCount[1];

            //Data_Car fix
            //stringCount += 5;
            //Events, PartsManu fix
            //stringCount += 2;

            Console.WriteLine("Strings expected: {0}", stringCount);

            bool notfound = true;

            // loop through the indexes to count them until finding the end of table index
            // the 255 is just infinite loop protection
            /*for (int i = 0; i <= stringCount + 255 && notfound; i++)
            {
                byte[] tempidx;
                tempidx = new byte[6];
                rawStream.Read(tempidx, 0, 6);
                // if we find the FF FF end of table index, fix the stringcount
                if (tempidx[0] == 255 && tempidx[1] == 255)
                {
                    notfound = false;
                    if (i != stringCount)
                    {
                        stringCount = i-1;
                    }
                }
            }
            // if we didn't find an end of table index, abort abort
            if (notfound)
            {
                return;
            }*/

            // reset filestream back to the start of the index section
            rawStream.Seek(0x2C,SeekOrigin.Begin);

            stringCount -= 1;

            indexes = new byte[stringCount+2][];

            // one more index than string, to include the end index
            for (int i = 0; i <= stringCount+1; i++)
            {
                indexes[i] = new byte[6];
                rawStream.Read(indexes[i],0,6);
            }

            strings = new byte[stringCount][];

            for (int i = 0; i < stringCount; i++)
            {
                int length = 2 * (   (indexes[i + 1][3] * 256 * 256 + indexes[i + 1][4] * 256 + indexes[i + 1][5])
                                   - (indexes[i][3]     * 256 * 256 + indexes[i][4]     * 256 + indexes[i][5]));
                /*int length;
                int b1 = indexes[i + 1][3] * 256 * 256;
                int b2 = indexes[i + 1][4] * 256;
                int b3 = indexes[i + 1][5];
                int i1 = indexes[i][3] * 256 * 256;
                int i2 = indexes[i][4] * 256;
                int i3 = indexes[i][5];
                int bt = b1 + b2 + b3;
                int it = i1 + i2 + i3;
                length = 2*(bt-it);*/
                
                strings[i] = new byte[length];
                rawStream.Read(strings[i], 0, length);
            }

            DataTable data = new DataTable();
            data.Columns.Add("Index", typeof(string));
            data.Columns.Add("String", typeof(string));

            for (int i = 0; i < stringCount; i++)
            {
                // Data_Car indexes are prefixed with CD 49 in the DB for whatever reason
                ulong a = 0xCDul * 256 * 256 * 256;
                ulong b = 0x49ul * 256 * 256;
                ulong c = (ulong)indexes[i][0] * 256 + (ulong)indexes[i][1];
                data.Rows.Add("_&" + (a+b+c), System.Text.Encoding.BigEndianUnicode.GetString(strings[i]));
            }

            editGrid.DataSource = data;

            long remainingSize = rawStream.Length - rawStream.Position;
            footer = new byte[remainingSize];
            for (int i = 0; i < remainingSize; i++)
            {
                footer[i] = (byte)rawStream.ReadByte();
            }

            FileStream output;
            output = File.OpenWrite("new_1.str");
            output.Write(header, 0, header.Length);
        }
    }
}