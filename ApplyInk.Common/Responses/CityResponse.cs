using System;
using System.Collections.Generic;
using System.Text;

namespace ApplyInk.Common.Responses
{
    public class CityResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int IdDepartment { get; set; }

        public DepartmentResponse Department { get; set; }

        public ICollection<ShopResponse> Shops { get; set; }

        public int ShopsNumber => Shops == null ? 0 : Shops.Count;

    }
}
