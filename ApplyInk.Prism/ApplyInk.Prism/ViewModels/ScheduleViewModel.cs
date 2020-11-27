using ApplyInk.Common.Helpers;
using ApplyInk.Common.Requests;
using ApplyInk.Common.Responses;
using ApplyInk.Common.Services;
using ApplyInk.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace ApplyInk.Prism.ViewModels
{
    public class ScheduleViewModel : ViewModelBase
    {
        private DateTime _date;
        private bool _isRunning;
        private UserResponse _tattoer;
        private readonly IApiService _apiService;
        private readonly INavigationService _navigationService;
        private DelegateCommand _addmeetingcommand;
        public ScheduleViewModel(IApiService apiService, INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Schedule";//Languages.AddMeeting;
        }

        public DelegateCommand AddMeetingCommand => _addmeetingcommand ?? (_addmeetingcommand = new DelegateCommand(CreateMeetingAsync));
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public UserResponse Tattoer
        {
            get => _tattoer;
            set => SetProperty(ref _tattoer, value);
        }

        [Obsolete]
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
               // OnPropertyChanged(nameof(Date));
            }
        }

        public Command DateChosen
        {
            get
            {
                return new Command((obj) =>
                {
                    System.Diagnostics.Debug.WriteLine(obj as DateTime?);
                });
            }
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("tattoer"))
            {
                Tattoer = parameters.GetValue<UserResponse>("tattoer");
            }
        }
        private async void CreateMeetingAsync()
        {
            IsRunning = true;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            MeetingRequest request = new MeetingRequest
            {
                Date = Date,
                Email = token.User.Email,
                EmailTattooer = Tattoer.Email,
                ShopId =Tattoer.Shop.Id,
                Status = 0

            };
            string url = App.Current.Resources["UrlAPI"].ToString();

            Response response = await _apiService.CreateMeetingAsync<MeetingResponse>(
                url,
                "/api",
                "/Meeting/Create", request);
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }
            
            await App.Current.MainPage.DisplayAlert(Languages.Ok, "ok", Languages.Accept);
            await _navigationService.GoBackAsync();
            
        }

    }
}
