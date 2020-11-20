using System;
using System.Collections.Generic;
using System.Text;

namespace ApplyInk.Common.Responses
{
    public class MasterDetailMeetingResponse
    {
        public int Id { get; set; }
        public MeetingResponse meeting { get; set; }

        public UserResponse user { get; set; }
    }
}
