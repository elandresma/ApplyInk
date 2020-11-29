﻿using ApplyInk.Common.Helpers;
using ApplyInk.Common.Requests;
using ApplyInk.Common.Responses;
using ApplyInk.Common.Services;
using ApplyInk.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Navigation;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace ApplyInk.Prism.ViewModels
{
    public class MyMeetingsViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        private List<MasterDetailMeetingResponse> _meetings;
        private readonly INavigationService _navigationService;
        private bool _isRunning;

        public MyMeetingsViewModel(IApiService apiService, INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.MyMeetings;
            LoadMyMeetings();
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
        public List<MasterDetailMeetingResponse> Meetings
        {
            get => _meetings;
            set => SetProperty(ref _meetings, value);
        }

        private async void LoadMyMeetings()
        {

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }
            MeetingRequest request = new MeetingRequest
            {
                Email = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token).User.Email.ToString()
            };

            string url = App.Current.Resources["UrlAPI"].ToString();

            Response response = await _apiService.GetMyMeetingsAsync<Response>(
                url,
                "/api",
                "/Meeting/GetMeetings", request);
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            _meetings = (List<MasterDetailMeetingResponse>)response.Result;
            Meetings = new List<MasterDetailMeetingResponse>(_meetings);

        }

    }
}
