using ApplyInk.Web.Data.Entities;
using ApplyInk.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyInk.Web.Helpers
{
    public interface IConverterHelper
    {
        Category ToCategory(CategoryViewModel model, bool isNew);

        CategoryViewModel ToCategoryViewModel(Category category);
    }
}
