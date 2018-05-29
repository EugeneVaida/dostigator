using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dostigator.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }

        public ICollection<Aim> Aims { get; set; }        
        public User()
        {
            Aims = new List<Aim>();            
        }
        
        public int? RoleId { get; set; }
        public Role Role { get; set; }
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}