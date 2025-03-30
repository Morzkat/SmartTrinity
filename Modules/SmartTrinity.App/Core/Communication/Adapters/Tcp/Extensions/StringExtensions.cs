using System.Text;

namespace SmartTrinity.App.Core.Communication.Adapaters.Tcp.Extensions
{
    public static class StringExtensions
    {
        public static char[] EncryptMessage(this char[] inp, int inplen, char[] key, int keylen)
        {
            return null;
        }

        public static char[] DecryptMessage(this char[] inp, int inplen, char[] key, int keylen)
        {
            char[] SBox = new char[257];
            char[] SBox2 = new char[257];
            int j;
            int t;
            int x;
            int i = 0;
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
            char temp;
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
                char k = SBox[t];
                inp[x] = (char)(inp[x] ^ k);
            }

            return inp;
        }

        public static string LPad(this string valueToPad, string filler, int size)
        {
            string lValueToPad;
            for (lValueToPad = valueToPad; lValueToPad.Length < size;)
            {
                lValueToPad = (new StringBuilder(filler)).Append(lValueToPad).ToString();
            }
            return lValueToPad;
        }

        public static string RPad(this string valueToPad, string filler, int size)
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

        public static string ConvertBinToHex(this char[] binStr, int len)
        {
            if (binStr == null)
                return "";

            char[] binToChar = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            string retStr = "";
            for (int i = 0; i < len; i++)
                retStr = (new StringBuilder(retStr)).Append(binToChar[(int)((uint)(binStr[i] & 0xf0) >> 4)]).Append(binToChar[binStr[i] & 0xf]).ToString();

            return retStr;
        }
    }
}
