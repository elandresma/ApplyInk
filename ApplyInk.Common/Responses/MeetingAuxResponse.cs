using ApplyInk.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplyInk.Common.Responses
{
    public class MeetingAuxResponse
    {
        public int Id { get; set; }

        public string userName { get; set; }

        public string tattooerName { get; set; }

        public ShopResponse Shop { get; set; }

        public StatusMeeting Status { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime DateLocal => Date.ToLocalTime();
    }
}
