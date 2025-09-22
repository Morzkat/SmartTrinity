using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.App.Core.Models
{
    public class ConsoleSettings
    {
        public int Port { get; set; }
        public string Host { get; set; }
        public Credentials Credentials { get; set; }
        public bool Enable { get; set; }
    }

    public class Credentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
