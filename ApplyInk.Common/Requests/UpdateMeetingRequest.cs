using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplyInk.Common.Requests
{
    public class UpdateMeetingRequest
    {
        [Required]
        public int IdMeeting { get; set; }
        public bool isActive { get; set; }
    }
}
