using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplyInk.Common.Responses
{

    public class CountryResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<DepartmentResponse> Departments { get; set; }

        public int DepartmentsNumber => Departments == null ? 0 : Departments.Count;

        public int CitiesNumber => Departments == null ? 0 : Departments.Sum(d => d.CitiesNumber);

    }
}
