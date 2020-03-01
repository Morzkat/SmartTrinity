using System.Collections.Generic;
using SmartTrinityApi.Common;

namespace SmartTrinityConsole.Infrastructure.Persistence
{
    public static class SmartUserPersistence
    {
        private static readonly string _user = "1";
        private static readonly string _pw = "1";
        public static bool UserIsLogged { get; set; }

        // TODO: Create dictionary with service denied responses...
        private enum _serviceDeniedResponses 
        {
            DENIED,
        };

        public static string PrepareDataForLogin()
        {
            string key = Tools.RPad(_user, " ", 25);
            string pw = Tools.RPad(_pw, " ", 25);

            char[] encryptedPw = Tools.Encrypt(pw.ToCharArray(), 25, key.ToCharArray(), 20);
            string data = $"US=1|PW={Tools.ConvertBinToHex(encryptedPw, 25)}|";

            return data;
        }
    }
}

