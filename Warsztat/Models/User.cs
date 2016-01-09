using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Warsztat.Models
{
    public class User
    {

        public long ID_user { get; set; }
        public int ID_role { get; set; }
        public string RoleName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}