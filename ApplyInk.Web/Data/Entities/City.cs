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
    public class City
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        [Display(Name = "City")]
        public string Name { get; set; }
        public ICollection<Shop> Shops { get; set; }

        [DisplayName("Shops Number")]
        [JsonIgnore]
        [NotMapped]
        public int ShopsNumber => Shops == null ? 0 : Shops.Count;

        [JsonIgnore]
        [NotMapped]
        public int IdDepartment { get; set; }

        [JsonIgnore]
        public Department Department { get; set; }
    }
}
