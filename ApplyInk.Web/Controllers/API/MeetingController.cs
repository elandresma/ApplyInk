using ApplyInk.Common.Requests;
using ApplyInk.Common.Responses;
using ApplyInk.Web.Data;
using ApplyInk.Web.Data.Entities;
using ApplyInk.Web.Helpers;
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
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;

        public MeetingController(DataContext context, IUserHelper userHelper)
        {
            _userHelper = userHelper;
            _context = context;
        }


        [HttpPost]
        [Route("GetMeetings")]
        public async Task<IActionResult> GetMeetings([FromBody] MeetingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }
            var myMeeting= await _context.MasterDetailMeetings
                .Include(m => m.meeting)
                 .Include(m => m.user)
                 .Where(m => m.user.Email == request.Email)
                 .ToListAsync();//debe traer solo 1

            return Ok(await _context.MasterDetailMeetings
                 .Include(m => m.meeting)
                 .ThenInclude(me => me.Shop)
                 .Include(m => m.user)
                 .Where(m => m.meeting.Id== myMeeting.FirstOrDefault().Id)
                 .ToListAsync());

        }


        [HttpPost]
        [Route("GetMeetings2")]
        public async Task<IActionResult> GetMeetings2([FromBody] MeetingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }
           
            return Ok(await _context.Meetings
                 .Include(m => m.masterDetailMeeting)
                 .ThenInclude(ma => ma.user)
                 .Include(me => me.Shop)
                 .Where(m => m.masterDetailMeeting.Any(ma => ma.user.Email == request.Email ))
                 .ToListAsync());

        }



        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateMeeting ([FromBody] MeetingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }

            Shop shop = await _context.Shops.FindAsync(request.ShopId);
            Meeting meeting = new Meeting
            {
                Date = request.Date,
                Status = request.Status,
                Shop = shop
            };

             await CreateMeetingdetail(meeting);


            Meeting meetingClient = _context.Meetings.LastOrDefault();
            User client = await _userHelper.GetUserAsync(request.Email);
            MasterDetailMeeting masterDetailMeetingClient = new MasterDetailMeeting
            {
                meeting = meetingClient,
                user = client
            };

            User tattooer = await _userHelper.GetUserAsync(request.EmailTattooer);
            MasterDetailMeeting masterDetailMeetingTattooer = new MasterDetailMeeting
            {
                meeting = meetingClient,
                user = tattooer
            };
            try
            {
                _context.MasterDetailMeetings.Add(masterDetailMeetingClient);
                _context.MasterDetailMeetings.Add(masterDetailMeetingTattooer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Ok(new Response { IsSuccess = true });
        }

        public async Task<IActionResult> CreateMeetingdetail(Meeting meeting)
        {
            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();
            return Ok();

        }
    }
}
