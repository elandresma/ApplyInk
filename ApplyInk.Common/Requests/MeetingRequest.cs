using ApplyInk.Common.Enums;
using ApplyInk.Common.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplyInk.Common.Requests
{
   public class MeetingRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        public string EmailTattooer { get; set; }

        public int ShopId { get; set; }

        public StatusMeeting Status { get; set; }

        public DateTime Date { get; set; }

        public DateTime DateLocal => Date.ToLocalTime();
    }
}
