using ApplyInk.Common.Responses;
using ApplyInk.Common.Services;
using ApplyInk.Prism.Helpers;
using ApplyInk.Prism.ItemViewModels;
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
        private ObservableCollection<TattoerItemViewModel> _tattoers;


        public TattoersPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Tattoers;
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
        public ObservableCollection<TattoerItemViewModel> Tattoers
        {
            get => _tattoers;
            set => SetProperty(ref _tattoers, value);
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
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
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
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            _myUsers = (List<UserResponse>)response.Result;
            ShowUsers();
        }

        private void ShowUsers()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Tattoers = new ObservableCollection<TattoerItemViewModel>(_myUsers.Select(t => new TattoerItemViewModel(_navigationService)
                {

                    Id = t.Id,
                    SocialNetworkURL = t.SocialNetworkURL,
                    Address = t.Address,
                    Categories = t.Categories,
                    Email = t.Email,
                    PhoneNumber = t.PhoneNumber,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    ImageId = t.ImageId,
                    Shop = t.Shop

                })
                    .ToList());
            }
            else
            {
                Tattoers = new ObservableCollection<TattoerItemViewModel>(_myUsers.Select(t => new TattoerItemViewModel(_navigationService)
                {

                    Id = t.Id,
                    SocialNetworkURL = t.SocialNetworkURL,
                    Address = t.Address,
                    Categories = t.Categories,
                    Email = t.Email,
                    PhoneNumber = t.PhoneNumber,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    ImageId = t.ImageId,
                    Shop = t.Shop

                })
                    .Where(t => t.FullName.ToLower().Contains(Search.ToLower()))
                    .ToList()) ;


            }
        }


    }
}
