using ApplyInk.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyInk.Web.Models
{
    public class DetailsMeetingViewModel
    {
        public string NameUser { get; set; }
        public string NameTattooer { get; set; }
        public string NameShop { get; set; }

        public StatusMeeting Status { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime DateLocal => Date.ToLocalTime();

    }
}
