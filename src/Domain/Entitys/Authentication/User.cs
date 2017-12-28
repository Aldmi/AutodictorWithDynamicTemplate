using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitys.Authentication
{
    public class User : EntityBase
    {
        public string Login { get; set; }
        public string Password { get; set; }

        //public int? RoleId { get; set; }
        public Role Role { get; set; }
    }
}
