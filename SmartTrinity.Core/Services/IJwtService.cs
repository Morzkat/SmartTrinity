using SmartTrinity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.Core.Services
{
    public interface IJwtService
    {
        Task<string> GenerateJwtToken(User user);
    }
}
