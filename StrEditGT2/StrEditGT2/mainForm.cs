using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CsvHelper;

namespace StrEdit
{
    public partial class mainForm : Form
    {
        byte[] header;
        UInt32 stringCount;
        byte[][] indexes;
        byte[][] strings;
        byte[] footer;
        byte[] colourCounts;
        byte[][] headerData;
        byte[][][] colourThumbnails;
        byte[][] colourNames;
        ushort colourStringCount;
        Dictionary<ushort, string> colourStrings;
        ushort colourIndexCount;
        byte[] carColourHeader;
        ushort[] carColourIndexes;
        ushort[][] colourStringIndexes;

        public mainForm()
        {
            InitializeComponent();
        }

        private UInt16 readUInt16(FileStream fileToRead)
        {
            byte[] rawValue = new byte[2];
            fileToRead.Read(rawValue, 0, 2);
            return (UInt16)(rawValue[1] * 256 + rawValue[0]);
        }

        private UInt32 readUInt32(FileStream fileToRead)
        {
            byte[] rawValue = new byte[4];
            fileToRead.Read(rawValue, 0, 4);
            return (UInt32)(rawValue[3] * 256 * 256 * 256 + rawValue[2] * 256 * 256 + rawValue[1] * 256 + rawValue[0]);
        }

        private byte[] convertUInt16ToBytes(UInt16 valueToConvert)
        {
            byte[] byteArray = new byte[2];
            byteArray[1] = (byte)((valueToConvert / 256) % 256);
            byteArray[0] = (byte)(valueToConvert % 256);
            return byteArray;
        }

        private byte[] convertUInt32ToBytes(UInt32 valueToConvert)
        {
            byte[] byteArray = new byte[4];
            byteArray[3] = (byte)((valueToConvert / 256 / 256 / 256) % 256);
            byteArray[2] = (byte)((valueToConvert / 256 / 256) % 256);
            byteArray[1] = (byte)((valueToConvert / 256) % 256);
            byteArray[0] = (byte) (valueToConvert % 256);
            return byteArray;
        }

        private void writeUInt16(FileStream fileToWrite, UInt16 valueToWrite)
        {
            byte[] bytesToWrite = convertUInt16ToBytes(valueToWrite);
            fileToWrite.Write(bytesToWrite, 0, 2);
        }

        private void writeUInt32(FileStream fileToWrite, UInt32 valueToWrite)
        {
            byte[] bytesToWrite = convertUInt32ToBytes(valueToWrite);
            fileToWrite.Write(bytesToWrite, 0, 4);
        }

        private void readFile(string filename)
        {
            FileStream rawStream;
            rawStream = File.OpenRead(filename);

            header = new byte[0x4];
            rawStream.Read(header, 0, 0x4);

            //0x04 0x05 - number of strings?
            //0x06 0x07 - blank

            stringCount = readUInt32(rawStream);
            
            Console.WriteLine("Strings expected: {0}", stringCount);

            indexes = new byte[stringCount][];
            colourCounts = new byte[stringCount];
            headerData = new byte[stringCount][];

            for (int i = 0; i < stringCount; i++)
            {
                indexes[i] = new byte[6];
                rawStream.Read(indexes[i], 0, 6);
                headerData[i] = new byte[2];
                rawStream.Read(headerData[i], 0, 2);
                int colourCount = headerData[i][0] & 0x3C;
                colourCount = colourCount >> 2;
                colourCounts[i] = (byte)(colourCount + 1);
            }

            strings = new byte[stringCount][];
            colourThumbnails = new byte[stringCount][][];
            colourNames = new byte[stringCount][];

            for (int i = 0; i < stringCount; i++)
            {
                rawStream.Position = (indexes[i][5] * 256) + indexes[i][4];
                int coloursLength = colourCounts[i];
                colourThumbnails[i] = new byte[coloursLength][];
                for (int j = 0; j < coloursLength; j++)
                {
                    colourThumbnails[i][j] = new byte[2];
                    rawStream.Read(colourThumbnails[i][j], 0, 2);
                }
                colourNames[i] = new byte[coloursLength];
                rawStream.Read(colourNames[i], 0, coloursLength);
                byte[] rawLength = new byte[1];
                rawStream.Read(rawLength, 0, 1);
                int length = rawLength[0] + 1;
                strings[i] = new byte[length];
                rawStream.Read(strings[i], 0, length);
            }

            DataTable data = new DataTable();
            data.Columns.Add("Index", typeof(string));
            data.Columns.Add("String", typeof(string));

            for (int i = 0; i < stringCount; i++)
            {
                int bodyID = (indexes[i][3] * 256 * 256 * 256) + (indexes[i][2] * 256 * 256) + (indexes[i][1] * 256) + (indexes[i][0]);
                data.Rows.Add(string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", indexes[i][3], indexes[i][2], indexes[i][1], indexes[i][0]), Encoding.ASCII.GetString(strings[i]).TrimEnd('\0').Replace(((char)0x7F).ToString(), "[R]"));
            }

            editGrid.DataSource = data;

            long remainingSize = rawStream.Length - rawStream.Position;
            footer = new byte[remainingSize];
            for (int i = 0; i < remainingSize; i++)
            {
                footer[i] = (byte)rawStream.ReadByte();
            }

            rawStream.Close();

            readColourIndexFile(".carcolor");
            readColourFile(".cclatain");
        }

        private void readColourIndexFile(string filename)
        {
            FileStream rawStream;
            rawStream = File.OpenRead(filename);

            carColourHeader = new byte[8];
            rawStream.Read(carColourHeader, 0, 8);

            colourIndexCount = (ushort)(readUInt16(rawStream) / 2);
            // Reset position later as the first string index is what we're using to figure out how many there are - i.e. the string data can only start after all of the indices

            carColourIndexes = new ushort[colourIndexCount];
            colourStringIndexes = new ushort[colourIndexCount][];

            for (ushort i = 0; i < colourIndexCount; i++)
            {
                rawStream.Position = 8 + (i * 2);
                carColourIndexes[i] = readUInt16(rawStream);
                ushort endIndex = readUInt16(rawStream);

                if (endIndex < carColourIndexes[i])
                {
                    endIndex = (ushort)rawStream.Length;
                }

                rawStream.Position = carColourIndexes[i];

                ushort indexCount = (ushort)((endIndex - carColourIndexes[i]) / 2);
                colourStringIndexes[i] = new ushort[indexCount];

                for (ushort j = 0; j < indexCount; j++)
                {
                    colourStringIndexes[i][j] = readUInt16(rawStream);
                }
            }

            rawStream.Close();
        }

        private void readColourFile(string filename)
        {
            FileStream rawStream;
            rawStream = File.OpenRead(filename);
            
            colourStringCount = (ushort)(readUInt16(rawStream) / 2);
            // Reset position later as the first string index is what we're using to figure out how many there are - i.e. the string data can only start after all of the indices
            
            colourStrings = new Dictionary<ushort, string>();

            for (ushort i = 0; i < colourStringCount; i++)
            {
                rawStream.Position = i * 2;
                ushort index = readUInt16(rawStream);
                rawStream.Position = index;

                byte current = 0x00;
                string colourName = "";

                do
                {
                    current = (byte)rawStream.ReadByte();
                    if (current != 0x00)
                    {
                        colourName += (char)current;
                    }
                }
                while (current != 0x00);

                colourStrings.Add(i, colourName);
            }
            
            rawStream.Close();
        }

        private void writeColourThumbnailCSV(string filename)
        {
            File.Delete(filename);

            TextWriter output = new StreamWriter(File.Create(filename), Encoding.UTF8);
            CsvWriter csv = new CsvWriter(output);
            csv.Configuration.QuoteAllFields = true;

            for (int i = 0; i < stringCount; i++)
            {
                for (int j = 0; j < colourCounts[i]; j++)
                {
                    csv.WriteField(string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", indexes[i][3], indexes[i][2], indexes[i][1], indexes[i][0]));
                    csv.WriteField(Encoding.ASCII.GetString(strings[i]).TrimEnd('\0').Replace(((char)0x7F).ToString(), "[R]"));

                    // Some sort of general reference to the colour, e.g. 0x31 for white, 0x34 for silver, and 0x77 for dark purple
                    csv.WriteField(string.Format("{0:X2}", colourNames[i][j]));

                    byte[] colourThumbnail = colourThumbnails[i][j];
                    int colourValue = (colourThumbnail[1] * 256) + colourThumbnail[0];

                    int R = colourValue & 0x1F;
                    int G = (colourValue >> 5) & 0x1F;
                    int B = (colourValue >> 10) & 0x1F;
                    
                    csv.WriteField((R * 8).ToString() + "," + (G * 8).ToString() + "," + (B * 8).ToString());

                    ushort colourIndex = colourStringIndexes[i][j];
                    csv.WriteField(colourStrings[colourIndex]);

                    csv.NextRecord();
                }
            }

            output.Close();
        }

        private void readColourCSV(string filename)
        {
            TextReader input = new StreamReader(File.OpenRead(filename), Encoding.UTF8);

            CsvReader csv = new CsvReader(input);

            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = false;

            var newColourNames = new Dictionary<string, ushort>();
            ushort stringNumber = 0;
            string previousCarID = "";
            int carNumber = 0;
            List<List<ushort>> newColourNumbers = new List<List<ushort>>();
            List<ushort> currentColourNumbers = new List<ushort>();
            List<List<byte>> newShortColours = new List<List<byte>>();
            List<byte> currentShortColours = new List<byte>();
            List<List<string>> newColourRGBs = new List<List<string>>();
            List<string> currentColourRGBs = new List<string>();

            while (csv.Read())
            {
                string carID = csv.GetField(0);

                if (carID != previousCarID)
                {
                    previousCarID = carID;
                    carNumber++;
                    if (carNumber > 1)
                    {
                        newShortColours.Add(currentShortColours);
                        currentShortColours = new List<byte>();
                        newColourRGBs.Add(currentColourRGBs);
                        currentColourRGBs = new List<string>();
                        newColourNumbers.Add(currentColourNumbers);
                        currentColourNumbers = new List<ushort>();
                    }
                }

                byte shortColour = byte.Parse(csv.GetField(2), System.Globalization.NumberStyles.HexNumber);
                currentShortColours.Add(shortColour);

                string rgb = csv.GetField(3);
                currentColourRGBs.Add(rgb);

                string colourName = csv.GetField(4);

                string[] test = new string[40];

                if (!newColourNames.ContainsKey(colourName))
                {
                    newColourNames.Add(colourName, stringNumber++);
                }

                currentColourNumbers.Add(newColourNames[colourName]);
            }
            newShortColours.Add(currentShortColours);
            newColourRGBs.Add(currentColourRGBs);
            newColourNumbers.Add(currentColourNumbers);

            input.Close();

            writeColourNamesFile(".cclatain", newColourNames);
            writeColourMappingFile(".carcolor", newColourNumbers);
            updateColourInfo(newColourRGBs, newShortColours);
        }

        private void writeColourNamesFile(string filename, Dictionary<string, ushort> strings)
        {
            File.Delete(filename + "_out");
            FileStream output;
            output = File.OpenWrite(filename + "_out");

            output.Position = strings.Count * 2;
            int i = 0;

            foreach (KeyValuePair<string, ushort> colourName in strings)
            {
                long eofPos = output.Position;
                output.Position = i * 2;
                output.WriteByte((byte)eofPos);
                output.WriteByte((byte)(eofPos / 256));
                output.Position = eofPos;

                byte[] colourNameBytes = Encoding.ASCII.GetBytes((colourName.Key + "\0").ToCharArray());
                output.Write(colourNameBytes, 0, colourNameBytes.Length);
                i++;
            }

            output.Close();
        }

        private void writeColourMappingFile(string filename, List<List<ushort>> allColourNumbers)
        {
            File.Delete(filename + "_out");
            FileStream output;
            output = File.OpenWrite(filename + "_out");

            output.Write(new byte[] { 0x43, 0x43, 0x4F, 0x4C, 0x30, 0x30, 0x00, 0x00 }, 0, 8);

            output.Position = (strings.Length * 2) + 8;
            int i = 0;

            foreach (List<ushort> colourNumbers in allColourNumbers)
            {
                long eofPos = output.Position;
                output.Position = (i * 2) + 8;
                output.WriteByte((byte)eofPos);
                output.WriteByte((byte)(eofPos / 256));
                output.Position = eofPos;

                foreach (ushort colourNumber in colourNumbers)
                {
                    writeUInt16(output, colourNumber);
                }

                i++;
            }

            output.Close();
        }

        private void updateColourInfo(List<List<string>> allRGBs, List<List<byte>> allShortColours)
        {
            int i = 0;

            colourThumbnails = new byte[allRGBs.Count][][];
            colourNames = new byte[allRGBs.Count][];

            foreach (List<string> rgbs in allRGBs)
            {
                colourThumbnails[i] = new byte[rgbs.Count][];
                colourNames[i] = new byte[allShortColours[i].Count];

                for (int j = 0; j < rgbs.Count; j++)
                {
                    string rgb = rgbs[j];

                    string[] rgbParts = rgb.Split(',');

                    int R = (int.Parse(rgbParts[0]) / 8) & 0x1F;
                    int G = ((int.Parse(rgbParts[1]) / 8) & 0x1F) << 5;
                    int B = ((int.Parse(rgbParts[2]) / 8) & 0x1F) << 10;

                    int bgr555 = R + G + B;

                    colourThumbnails[i][j] = new byte[2];
                    colourThumbnails[i][j][0] = (byte)bgr555;
                    colourThumbnails[i][j][1] = (byte)(bgr555 / 256);

                    colourNames[i][j] = allShortColours[i][j];
                }
                
                int colourCount = rgbs.Count - 1;
                colourCount = colourCount << 2;
                colourCount = colourCount & 0x3C;
                headerData[i][0] = (byte)colourCount;
                
                i++;
            }
        }

        private void menuReadFile(string filename)
        {
            FileStream rawStream;
            rawStream = File.OpenRead(filename);

            header = new byte[0x8];
            rawStream.Read(header, 0, 0x8);

            //0x08 0x09 - number of strings

            stringCount = readUInt16(rawStream);

            Console.WriteLine("Strings expected: {0}", stringCount);

            strings = new byte[stringCount][];

            for (int i = 0; i < stringCount; i++)
            {
                int length = (readUInt16(rawStream) + 1) * 2;
                strings[i] = new byte[length];
                rawStream.Read(strings[i], 0, length);
            }

            DataTable data = new DataTable();
            data.Columns.Add("Index", typeof(string));
            data.Columns.Add("String", typeof(string));

            for (int i = 0; i < stringCount; i++)
            {
                data.Rows.Add(i, Encoding.Unicode.GetString(strings[i]).TrimEnd('\0').Replace(((char)0x7F).ToString(), "[R]"));
            }

            editGrid.DataSource = data;

            long remainingSize = rawStream.Length - rawStream.Position;
            footer = new byte[remainingSize];
            for (int i = 0; i < remainingSize; i++)
            {
                footer[i] = (byte)rawStream.ReadByte();
            }

            rawStream.Close();
        }

        private void rebuildStrings()
        {
            stringCount = (uint)(editGrid.RowCount - 1);
            indexes = new byte[stringCount][];
            strings = new byte[stringCount][];
            byte[][] headerDataCopy = headerData;
            headerData = new byte[stringCount][];
            for(int i = 0; i < stringCount; i++)
            {
                // write the current string
                DataGridViewRow row = editGrid.Rows[i];
                string outstring = row.Cells["String"].Value.ToString();
                outstring = outstring.Replace("[R]", ((char)0x7F).ToString());
                outstring += "\0";
                byte[] outstringbytes = Encoding.ASCII.GetBytes(outstring.ToCharArray());
                strings[i] = outstringbytes;

                // write the index pointer for this string
                string outindex = row.Cells["Index"].Value.ToString();
                //outindex = long.Parse(outindex).ToString("X").Substring(4);
                indexes[i] = new byte[6];
                indexes[i][5] = 0xAD; // obvious placeholders
                indexes[i][4] = 0xDE;
                indexes[i][3] = byte.Parse(outindex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                indexes[i][2] = byte.Parse(outindex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                indexes[i][1] = byte.Parse(outindex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                indexes[i][0] = byte.Parse(outindex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

                if (i < headerDataCopy.Length)
                {
                    headerData[i] = headerDataCopy[i];
                }
                else
                {
                    headerData[i] = new byte[2];
                }
            }
        }

        private void menuRebuildStrings()
        {
            stringCount = (uint)(editGrid.RowCount - 1);
            strings = new byte[stringCount][];
            for (int i = 0; i < stringCount; i++)
            {
                // write the current string
                DataGridViewRow row = editGrid.Rows[i];
                string outstring = row.Cells["String"].Value.ToString();
                outstring += "\0";
                byte[] outstringbytes = Encoding.Unicode.GetBytes(outstring.ToCharArray());
                strings[i] = outstringbytes;
            }
        }

        private void writeFile(string filename)
        {
            readColourCSV("test.csv");

            File.Delete(filename + "_out");
            FileStream output;
            output = File.OpenWrite(filename + "_out");
            output.Write(header, 0, header.Length);
            writeUInt32(output, stringCount);

            for (int i = 0; i < indexes.Length; i++)
            {
                output.Write(indexes[i], 0, indexes[i].Length);
                output.Write(headerData[i], 0, headerData[i].Length);
            }
            for (int i = 0; i < strings.Length; i++)
            {
                long eofPos = output.Position;
                output.Position = (i * 8) + 12;
                output.WriteByte((byte)eofPos);
                output.WriteByte((byte)(eofPos / 256));
                output.Position = eofPos;
                foreach (byte[] colourThumbnail in colourThumbnails[i])
                {
                    output.Write(colourThumbnail, 0, colourThumbnail.Length);
                }
                output.Write(colourNames[i], 0, colourNames[i].Length);
                byte[] stringLength = new byte[] { (byte)(strings[i].Length - 1) };
                output.Write(stringLength, 0, stringLength.Length);
                output.Write(strings[i], 0, strings[i].Length);
            }

            output.Close();
        }

        private void menuWriteFile(string filename)
        {
            //File.Delete(filename);
            FileStream output;
            output = File.OpenWrite(filename + "_out");
            output.Write(header, 0, header.Length);
            writeUInt16(output, (ushort)stringCount);
            
            foreach (byte[] newstring in strings)
            {
                int calcLen = (newstring.Length - 1) / 2;
                byte[] len = { (byte)calcLen, 0x00 };
                output.Write(len, 0, 2);
                output.Write(newstring, 0, newstring.Length);
            }
            output.Write(footer, 0, footer.Length);

            output.Close();
        }

        private void writeCSV(string filename)
        {
            File.Delete(filename);
            //TextWriter output = File.CreateText(filename);

            TextWriter output = new StreamWriter(File.Create(filename), Encoding.UTF8);

            CsvWriter csv = new CsvWriter(output);

            csv.Configuration.QuoteAllFields = true;

            for (int i = 0; i < editGrid.RowCount - 1; i++)
            {
                DataGridViewRow row = editGrid.Rows[i];

                foreach(DataGridViewCell x in row.Cells)
                {
                    csv.WriteField(x.Value.ToString());
                }

                //csv.WriteField(row.Cells["Index"].Value.ToString());
                //csv.WriteField(row.Cells["String"].Value.ToString());
                csv.NextRecord();

                //output.WriteLine(row.Cells["String"].Value.ToString());
            }
            
            //csv.WriteRecords(editGrid.Rows);

            output.Close();

            writeColourThumbnailCSV("test.csv");
        }

        private void readCSV(string filename)
        {
            TextReader input = new StreamReader(File.OpenRead(filename), Encoding.UTF8);

            CsvReader csv = new CsvReader(input);

            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = false;

            editGrid.DataSource = null;
            editGrid.Refresh();

            DataTable data = new DataTable();
            data.Columns.Add("Index", typeof(string));
            data.Columns.Add("String", typeof(string));

            while(csv.Read())
            {
                data.Rows.Add(csv.GetField(0), csv.GetField(1));
            }

            editGrid.DataSource = data;
            editGrid.Refresh();

            input.Close();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            //readFile("Data_Car.str");
            //writeFile("new_1.str");
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            if (rbMenu.Checked)
            {
                menuReadFile(txFilename.Text);
            }
            else
            {
                readFile(txFilename.Text);
            }
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            if (rbMenu.Checked)
            {
                menuRebuildStrings();
                menuWriteFile(txFilename.Text);
            }
            else
            {
                rebuildStrings();
                writeFile(txFilename.Text);
            }
        }

        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            //rebuildStrings();
            writeCSV(txFilename.Text + ".csv");
        }

        private void btnImportCSV_Click(object sender, EventArgs e)
        {
            readCSV(txFilename.Text + ".csv");
        }

        private void rbMenu_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMenu.Checked)
            {
                txFilename.Text = "eng_unistrdb.dat";
            }
        }
    }
}