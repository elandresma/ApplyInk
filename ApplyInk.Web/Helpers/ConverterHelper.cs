using ApplyInk.Web.Data.Entities;
using ApplyInk.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyInk.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Category ToCategory(CategoryViewModel model, bool isNew)
        {
            return new Category
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name
            };
        }

        public CategoryViewModel ToCategoryViewModel(Category category)
        {
            return new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
