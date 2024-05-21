namespace SmartTrinity.App.Extensions
{
    public static class MsgDataExtensions
    {
        public static string ClearMessage(this string msgData)
        {
            int addressPipesLength = 12 + 1234.ToString().Length;
            msgData = msgData.Substring(addressPipesLength, msgData.Length - addressPipesLength);
            msgData = msgData.Substring(0, msgData.Length - 1);

            return msgData;
        }

        public static Dictionary<string, string> ToDictionary(this string msgData)
        {
            string[] msgs = msgData.Split('|');
            Dictionary<string, string> resultData = new Dictionary<string, string>();

            for (int i = 0; i < msgs.Length; i++)
            {
                try
                {
                    string[] keyValue = msgs[i].Split('=');
                    resultData.Add(keyValue[0], keyValue[1]);
                }
                catch { continue; }

            }

            return resultData;
        }
    }
}
