using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyInk.Web.Data.Entities
{
    public class MasterDetailMeeting
    {
        public int Id { get; set; }
        public Meeting meeting { get; set; }

        public User user { get; set; }

    }
}
