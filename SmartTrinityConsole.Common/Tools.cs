using System;
using System.Text;

namespace SmartTrinityApi.Common
{
    //TODO: Investigate if is better a static class o interface with injection... 
    public static class Tools
    {
        static string generation = "11345113451";
        static int generation2 = GetGeneration(3) + 1;
        static int generation3 = GetGeneration(1);

        public static char[] EncryptMessage(char[] inp, int inplen, char[] key, int keylen)
        {
            char[] Sbox = new char[257];
            char[] Sbox2 = new char[257];
            char k = '\0';
            int j;
            int t;
            int x;
            int i = j = t = x = 0;
            char temp = '\0';
            for (i = 0; i < 256; i++)
            {
                Sbox[i] = '\0';
                Sbox2[i] = '\0';
            }

            for (i = 0; i < 256; i++)
            {
                Sbox[i] = (char)i;
            }

            j = 0;
            for (i = 0; i < 256; i++)
            {
                if (j == keylen)
                {
                    j = 0;
                }
                Sbox2[i] = key[j++];
            }

            j = 0;
            for (i = 0; i < 256; i++)
            {
                j = (j + Sbox[i] + Sbox2[i]) % 256;
                temp = Sbox[i];
                Sbox[i] = Sbox[j];
                Sbox[j] = temp;
            }

            i = j = 0;
            for (x = 0; x < inplen; x++)
            {
                i = (i + 1) % 256;
                j = (j + Sbox[i]) % 256;
                temp = Sbox[i];
                Sbox[i] = Sbox[j];
                Sbox[j] = temp;
                t = (Sbox[i] + Sbox[j]) % 256;
                k = Sbox[t];
                inp[x] = (char)(inp[x] ^ k);
            }

            return inp;
        }


        public static char[] DecryptMessage()
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

        public static string Lpad(string valueToPad, string filler, int size)
        {
            string lValueToPad;
            for (lValueToPad = valueToPad; lValueToPad.Length < size;)
            {
                lValueToPad = (new StringBuilder(filler)).Append(lValueToPad).ToString();
            }
            return lValueToPad;
        }

        public static string Rpad(string valueToPad, string filler, int size)
        {
            string lValueToPad;
            for (lValueToPad = valueToPad; lValueToPad.Length < size;)
            {
                lValueToPad = (new StringBuilder(lValueToPad)).Append(filler).ToString();
            }
            return lValueToPad;
        }
    }
}
