using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Warsztat.ViewModels
{
    public class AddressViewModel
    {
        public long SelectedAddressId { get; set; }
        public IEnumerable<SelectListItem> Addresses { get; set; }
    }
}