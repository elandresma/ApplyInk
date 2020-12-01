using ApplyInk.Common.Enum;
using ApplyInk.Common.Enums;
using ApplyInk.Common.Responses;
using ApplyInk.Web.Data.Entities;
using ApplyInk.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyInk.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Category ToCategory(CategoryViewModel model, bool isNew)
        {
            return new Category
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name
            };
        }

        public CategoryViewModel ToCategoryViewModel(Category category)
        {
            return new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            };
        }
        public List<MeetingAuxResponse> ToMeetingAuxResponse(List<Meeting> meetings)
        {
            List<MeetingAuxResponse> meetingsResponse = new List<MeetingAuxResponse>();
            foreach (var meeting in meetings)
            {
                var masterTattooer = meeting.masterDetailMeeting.Where(m => m.user.UserType == UserType.Tattooer);
                var masterClient = meeting.masterDetailMeeting.Where(m => m.user.UserType == UserType.User);

                meetingsResponse.Add(new MeetingAuxResponse
                {
                    Id = meeting.Id,
                    Shop = meeting.Shop.Name,
                    Date = meeting.DateLocal,
                    tattooerName = masterTattooer.FirstOrDefault().user.FullName,
                    Status = meeting.Status == StatusMeeting.Active ? true : false,
                    userName = masterClient.FirstOrDefault().user.FullName
                });
            }


            return meetingsResponse;


        }

    }
}
