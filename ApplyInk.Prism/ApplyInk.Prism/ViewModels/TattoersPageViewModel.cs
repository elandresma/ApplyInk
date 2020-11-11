using ApplyInk.Common.Requests;
using ApplyInk.Common.Responses;
using ApplyInk.Common.Services;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace ApplyInk.Prism.ViewModels
{
    public class TattoersPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ObservableCollection<UserResponse> _users;
        private bool _isRunning;
        private string _search;
        private List<UserResponse> _myUsers;
        private DelegateCommand _searchCommand;


        public TattoersPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Tattoers";
            LoadUsersAsync();
        }

        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(ShowUsers));

        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                ShowUsers();
            }
        }


        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }


        public ObservableCollection<UserResponse> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        private async void LoadUsersAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Check the internet connection.", "Accept");
                return;
            }

            IsRunning = true;
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<UserResponse>(
                url,
                "/api",
                "/account/GetTattoers");
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            _myUsers = (List<UserResponse>)response.Result;
            ShowUsers();
        }

        private void ShowUsers()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Users = new ObservableCollection<UserResponse>(_myUsers);
            }
            else
            {
                Users = new ObservableCollection<UserResponse>(_myUsers
                    .Where(p => p.FullName.ToLower().Contains(Search.ToLower())));
            }
        }


    }
}
