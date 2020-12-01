using ApplyInk.Common.Enums;
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
        private readonly IConverterHelper _converteHelper;

        public MeetingController(DataContext context, IUserHelper userHelper, IConverterHelper converterHelper )
        {
            _userHelper = userHelper;
            _context = context;
            _converteHelper = converterHelper;
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

            var rpta = await _context.Meetings
                    .Include(m => m.masterDetailMeeting)
                    .ThenInclude(ma => ma.user)
                    .Include(me => me.Shop)
                    .Where(m => m.masterDetailMeeting.Any(ma => ma.user.Email == request.Email))
                    .ToListAsync();
            
            return Ok(_converteHelper.ToMeetingAuxResponse(rpta));

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

        [HttpPut]
        [Route("UpdateMeetings")]
        public async Task<IActionResult> UpdateMeetingStatus([FromBody] UpdateMeetingRequest request)
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
            Meeting meeting = await _context.Meetings.FindAsync(request.IdMeeting);
            meeting.Status = StatusMeeting.Cancelled;
            _context.Update(meeting);
            await _context.SaveChangesAsync();
            return Ok(new Response { IsSuccess = true });
        }
    }
}
