namespace GT3.HashGenerator
{
    public static class HashGenerator
    {
        public static ulong GenerateHash(string name)
        {
            ulong hash = 0;
            char[] nameChars = name.ToCharArray();

            foreach (char nameChar in nameChars)
            {
                hash += (byte)nameChar;
            }

            foreach (char nameChar in nameChars)
            {
                byte asciiValue = (byte)nameChar;
                ulong temp1 = hash << 7;
                ulong temp2 = hash >> 57;
                hash = temp1 | temp2;
                hash += asciiValue;
            }

            return hash;
        }
    }
}