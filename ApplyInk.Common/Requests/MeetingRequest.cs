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

        public int IdMeeting { get; set; }
    }
}
