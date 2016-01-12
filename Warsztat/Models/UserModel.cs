using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Warsztat.Models
{
    public class UserModel
    {
        public Users Users { get; set; }
        public Addresses Addresses { get; set; }
        public List<Cars> Cars { get; set; }
        public List<Orders> Orders { get; set; }
    }
}