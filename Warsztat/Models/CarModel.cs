using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Warsztat.Models
{
    public class CarModel
    {
        public Cars Car { get; set; }
        public List<Repairs> Repairs { get; set; }
    }
}