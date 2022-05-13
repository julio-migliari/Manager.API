using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Core.Entities
{
    public class User 
    {
        protected User()
        {
            CreatedAt = DateTime.Now;
            Active = true;
        }
        public int Id { get; private set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public DateTime CreatedAt { get; private set; }
        public bool Active { get; private set; }
        public void Deactivate()
        {
            Active = false;
        }

    }
}
