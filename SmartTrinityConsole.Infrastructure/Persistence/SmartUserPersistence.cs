using System.Collections.Generic;
using Microsoft.Extensions.Options;
using SmartTrinityApi.Common;
using SmartTrinityApi.Core.Entities;

namespace SmartTrinityConsole.Infrastructure.Persistence
{
    public static class SmartUserPersistence
    {
        public static string User = "1";
        public static string Password = "1";
        public static string LastReply = "User connected to console status | CONNECTED";

        public static bool UserIsLogged { get; set; }

        // TODO: Create dictionary with service denied responses...
        private enum _serviceDeniedResponses 
        {
            DENIED,
        };

        public static string PrepareDataForLogin()
        {
            string key = Tools.RPad(User, " ", 25);
            string pw = Tools.RPad(Password, " ", 25);

            char[] encryptedPw = Tools.Encrypt(pw.ToCharArray(), 25, key.ToCharArray(), 20);
            string data = $"US=1|PW={Tools.ConvertBinToHex(encryptedPw, 25)}|";

            return data;
        }
    }
}

