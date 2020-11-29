using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApplyInk.Web.Data.Entities
{
    public class MasterDetailMeeting
    {
        public int Id { get; set; }

        [JsonIgnore]
        public Meeting meeting { get; set; }

        public User user { get; set; }

    }
}
