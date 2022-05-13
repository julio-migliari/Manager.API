using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Models
{
    public class UserView
    {
        public UserView(string userName, string token, int id)
        {
            UserName = userName;
            Token = token;
            Id = id;
        }

        public string UserName { get; private set; }
        public string Token { get; private set; }
        public int Id { get; private set; }
    }
}
