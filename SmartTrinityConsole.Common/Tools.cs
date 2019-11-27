using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrinityApi.Common
{
    //TODO: Investigate if is better a static class o interface with injection... 
    public static class Tools
    {
        private static string generation = "11345113451";
        private static int generation2 = GetGeneration(3) + 1;
        private static int generation3 = GetGeneration(1);
        public static int CurrentSocketPort { get; set; }

        public static char[] Encrypt(char[] inp, int inplen, char[] key, int keylen)
        {
            char[] SBox = new char[257];
            char[] SBox2 = new char[257];
            char k = '\0';
            int j;
            int t;
            int x;
            int i = j = t = x = 0;
            char temp = '\0';
            for (i = 0; i < 256; i++)
            {
                SBox[i] = '\0';
                SBox2[i] = '\0';
            }

            for (i = 0; i < 256; i++)
            {
                SBox[i] = (char)i;
            }

            j = 0;
            for (i = 0; i < 256; i++)
            {
                if (j == keylen)
                {
                    j = 0;
                }
                SBox2[i] = key[j++];
            }

            j = 0;
            for (i = 0; i < 256; i++)
            {
                j = (j + SBox[i] + SBox2[i]) % 256;
                temp = SBox[i];
                SBox[i] = SBox[j];
                SBox[j] = temp;
            }

            i = j = 0;
            for (x = 0; x < inplen; x++)
            {
                i = (i + 1) % 256;
                j = (j + SBox[i]) % 256;
                temp = SBox[i];
                SBox[i] = SBox[j];
                SBox[j] = temp;
                t = (SBox[i] + SBox[j]) % 256;
                k = SBox[t];
                inp[x] = (char)(inp[x] ^ k);
            }

            return inp;
        }


        public static char[] Decrypt()
        {
            return null;
        }

        public static int GetSeeds()
        {
            return generation3 * generation2 * generation2 * generation2 * generation2 * generation2 + generation.Length * generation2 * generation2;
        }

        public static int GetGeneration(int generation31)
        {
            generation3 = generation31;
            return generation31 * 3;
        }

        public static string LPad(string valueToPad, string filler, int size)
        {
            string lValueToPad;
            for (lValueToPad = valueToPad; lValueToPad.Length < size;)
            {
                lValueToPad = (new StringBuilder(filler)).Append(lValueToPad).ToString();
            }
            return lValueToPad;
        }

        public static string RPad(string valueToPad, string filler, int size)
        {
            string lValueToPad;
            for (lValueToPad = valueToPad; lValueToPad.Length < size;)
            {
                lValueToPad = (new StringBuilder(lValueToPad)).Append(filler).ToString();
            }
            return lValueToPad;
        }

        public static string GetComputerId()
        {
            return System.Net.Dns.GetHostName().ToUpper();
        }

        public static string ConvertBinToHex(char[] binStr, int len)
        {
            char[] binToChar = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            string retStr = "";
            for (int i = 0; i < len; i++)
                retStr = (new StringBuilder(retStr)).Append(binToChar[(int)((uint)(binStr[i] & 0xf0) >> 4)]).Append(binToChar[binStr[i] & 0xf]).ToString();

            return retStr;
        }
    }
}
