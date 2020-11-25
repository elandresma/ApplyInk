using ApplyInk.Common.Requests;
using ApplyInk.Common.Responses;
using ApplyInk.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ApplyInk.Common.Services
{
    public interface IApiService
    {
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);

        Task<Response> GetListShop<T>(string urlBase, string servicePrefix, string controller);

        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);

        Task<Response> RegisterUserAsync(string urlBase, string servicePrefix, string controller, UserRequest userRequest);

        Task<Response> RecoverPasswordAsync(string urlBase, string servicePrefix, string controller, EmailRequest emailRequest);

        Task<Response> ModifyUserAsync(string urlBase, string servicePrefix, string controller, UserRequest userRequest, string token);

        Task<Response> ChangePasswordAsync(string urlBase, string servicePrefix, string controller, ChangePasswordRequest changePasswordRequest, string token);

        Task<Response> PostAsync<T>(string urlBase, string servicePrefix, string controller, T model, string token);

        Task<RandomUsers> GetRandomUser(string urlBase, string servicePrefix);

        Task<Stream> GetPictureAsync(string urlBase, string servicePrefix);
        Task<Response> CreateMeetingAsync<T>(string urlBase, string servicePrefix, string controller, MeetingRequest meeting);

    }
}
