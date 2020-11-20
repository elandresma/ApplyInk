using ApplyInk.Common.Requests;
using ApplyInk.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyInk.Web.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingController : ControllerBase
    {
        private readonly DataContext _context;

        public MeetingController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMeetings(MeetingRequest request)
        {
            var myMeeting= await _context.MasterDetailMeetings
                .Include(m => m.meeting)
                 .Include(m => m.user)
                 .Where(m => m.user.Email == request.Email)
                 .ToListAsync();

            return Ok(await _context.MasterDetailMeetings
                 .Include(m => m.meeting)
                 .ThenInclude(me => me.Shop)
                 .Include(m => m.user)
                 .Where(m => m.meeting.Id== myMeeting.FirstOrDefault().Id)
                 .ToListAsync());

        }
    }
}
