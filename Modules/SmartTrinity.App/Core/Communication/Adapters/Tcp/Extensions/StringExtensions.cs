using System.Text;

namespace SmartTrinity.App.Core.Communication.Adapters.Tcp.Extensions
{
    public static class StringExtensions
    {
        public static char[] EncryptMessage(this char[] input, int inputLength, char[] key, int keyLength)
        {
            const int SBoxLength = 256;
            var sBox = new char[SBoxLength];
            var sBox2 = new char[SBoxLength];

            // Initialize SBox
            for (int i = 0; i < SBoxLength; i++)
                sBox[i] = (char)i;

            // Fill SBox2 with key
            for (int i = 0, j = 0; i < SBoxLength; i++)
            {
                if (j == keyLength) j = 0;
                sBox2[i] = key[j++];
            }

            // Key scheduling algorithm (KSA)
            for (int i = 0, j = 0; i < SBoxLength; i++)
            {
                j = (j + sBox[i] + sBox2[i]) % SBoxLength;
                (sBox[i], sBox[j]) = (sBox[j], sBox[i]);
            }

            // Pseudo-random generation algorithm (PRGA)
            for (int x = 0, i = 0, j = 0; x < inputLength; x++)
            {
                i = (i + 1) % SBoxLength;
                j = (j + sBox[i]) % SBoxLength;
                (sBox[i], sBox[j]) = (sBox[j], sBox[i]);
                int t = (sBox[i] + sBox[j]) % SBoxLength;
                char k = sBox[t];
                input[x] = (char)(input[x] ^ k);
            }

            return input;
        }

        public static char[] DecryptMessage(this char[] input, int inputLength, char[] key, int keyLength)
        {
            const int SBoxLength = 256;
            var sBox = new char[SBoxLength];
            var sBox2 = new char[SBoxLength];

            // Initialize SBox
            for (int i = 0; i < SBoxLength; i++)
                sBox[i] = (char)i;

            // Fill SBox2 with key
            for (int i = 0, j = 0; i < SBoxLength; i++)
            {
                if (j == keyLength) j = 0;
                sBox2[i] = key[j++];
            }

            // Key scheduling algorithm (KSA)
            for (int i = 0, j = 0; i < SBoxLength; i++)
            {
                j = (j + sBox[i] + sBox2[i]) % SBoxLength;
                (sBox[i], sBox[j]) = (sBox[j], sBox[i]);
            }

            // Pseudo-random generation algorithm (PRGA)
            for (int x = 0, i = 0, j = 0; x < inputLength; x++)
            {
                i = (i + 1) % SBoxLength;
                j = (j + sBox[i]) % SBoxLength;
                (sBox[i], sBox[j]) = (sBox[j], sBox[i]);
                int t = (sBox[i] + sBox[j]) % SBoxLength;
                char k = sBox[t];
                input[x] = (char)(input[x] ^ k);
            }

            return input;
        }

        public static string LPad(this string valueToPad, string filler, int size)
        {
            string lValueToPad;
            for (lValueToPad = valueToPad; lValueToPad.Length < size;)
            {
                lValueToPad = new StringBuilder(filler).Append(lValueToPad).ToString();
            }
            return lValueToPad;
        }

        public static string RPad(this string valueToPad, string filler, int size)
        {
            string lValueToPad;
            for (lValueToPad = valueToPad; lValueToPad.Length < size;)
            {
                lValueToPad = new StringBuilder(lValueToPad).Append(filler).ToString();
            }
            return lValueToPad;
        }

        public static string GetComputerId()
        {
            return System.Net.Dns.GetHostName().ToUpper();
        }

        public static string ConvertBinToHex(this char[] binStr, int len)
        {
            //if (binStr == null)
            //    return "";

            char[] binToChar = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            string retStr = "";
            for (int i = 0; i < len; i++)
                retStr = new StringBuilder(retStr).Append(binToChar[(int)((uint)(binStr[i] & 0xf0) >> 4)]).Append(binToChar[binStr[i] & 0xf]).ToString();

            return retStr;
        }
    }
}
