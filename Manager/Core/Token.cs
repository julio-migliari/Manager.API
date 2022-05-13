using Manager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Core
{
    public class Token
    {
        public string Hash { get; private set; }
        public Token()
        {
            Hash = "kjbdfkngkleg4555";
        }
        public Token(User user)
        {
            Hash = user.UserName + user.Pass;
        }

        public byte[] GetHash()
        {
            return Encoding.ASCII.GetBytes(Hash); 
        }

    }
}
