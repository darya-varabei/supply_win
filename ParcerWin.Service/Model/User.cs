using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcerWin.Service.Model
{
    public class User
    {
        public string? Login { get; set; }
        public string? Password { get; set; }
        public UserInfo? UserInfo { get; set; } 
    }
}
