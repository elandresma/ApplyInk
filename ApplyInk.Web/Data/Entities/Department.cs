using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyInk.Web.Data.Entities
{
    public class Department
    {​​​​
        public int Id {​​​​ get; set; }​​​​
 
        [MaxLength(50, ErrorMessage = "The field {​​​​0}​​​​ must contain {​​​​1}​​​​ characters max")]
        [Required]
        public string Name {​​​​ get; set; }​​​​
 
        public ICollection<City> Cities {​​​​ get; set; }​​​​
 
        [DisplayName("Cities Number")]
        public int CitiesNumber => Cities == null ? 0 : Cities.Count;

        [JsonIgnore]
        [NotMapped]
        public int IdCountry {​​​​ get; set; }​​​​
 
        [JsonIgnore]
        public Country Country {​​​​ get; set; }​​​​
 
    }​​​​
}
