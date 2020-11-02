using ApplyInk.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyInk.Web.Models
{
    public class CategoryViewModel : Category
    {
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
