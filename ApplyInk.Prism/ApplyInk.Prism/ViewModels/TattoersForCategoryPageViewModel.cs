using ApplyInk.Common.Helpers;
using ApplyInk.Common.Responses;
using ApplyInk.Common.Services;
using ApplyInk.Prism.Helpers;
using ApplyInk.Prism.ItemViewModels;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Essentials;


namespace ApplyInk.Prism.ViewModels
{

    public class TattoersForCategoryPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ObservableCollection<UserResponse> _users;
        private bool _isRunning;
        private bool _isEnabled;
        private string _search;
        private CategoryResponse _category;
        private List<UserResponse> _myUsers;
        private List<UserResponse> _myUsers2;
        private UserResponse _user;
        private DelegateCommand _searchCommand;
        private ObservableCollection<TattoerItemViewModel> _tattoers;
        private ObservableCollection<CategoryResponse> _categories;



        public TattoersForCategoryPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.TattoersForCategory;
            IsEnabled = true;
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            User = token.User;
            
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

        public ObservableCollection<CategoryResponse> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public CategoryResponse Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }


        public ObservableCollection<UserResponse> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        private async void LoadCategoriesAsync()
        {
            IsRunning = true;
            IsEnabled = false;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<CategoryResponse>(url, "/api", "/Categories");
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            List<CategoryResponse> list = (List<CategoryResponse>)response.Result;
            Categories = new ObservableCollection<CategoryResponse>(list.OrderBy(c => c.Name));
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("category"))
            {
                Category = parameters.GetValue<CategoryResponse>("category");
            }
            LoadUsersAsync();
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

            
            _myUsers2 = (List<UserResponse>)response.Result;
            _myUsers = _myUsers2.Where(t => t.Categories.Contains(Category)).ToList();

            ShowUsers();
        }



        //private void ShowUsers()
        //{
        //    if (Category.Name == null)
        //    {
        //        Tattoers = new ObservableCollection<TattoerItemViewModel>(_myUsers.Select(t => new TattoerItemViewModel(_navigationService)
        //        {

        //            Id = t.Id,
        //            SocialNetworkURL = t.SocialNetworkURL,
        //            Address = t.Address,
        //            Categories = t.Categories,
        //            Email = t.Email,
        //            PhoneNumber = t.PhoneNumber,
        //            FirstName = t.FirstName,
        //            LastName = t.LastName,
        //            ImageId = t.ImageId,
        //            Shop = t.Shop

        //        })
        //            .ToList());
        //    }
        //    else
        //    {
        //        Tattoers = new ObservableCollection<TattoerItemViewModel>(_myUsers.Select(t => new TattoerItemViewModel(_navigationService)
        //        {

        //            Id = t.Id,
        //            SocialNetworkURL = t.SocialNetworkURL,
        //            Address = t.Address,
        //            Categories = t.Categories,
        //            Email = t.Email,
        //            PhoneNumber = t.PhoneNumber,
        //            FirstName = t.FirstName,
        //            LastName = t.LastName,
        //            ImageId = t.ImageId,
        //            Shop = t.Shop

        //        })
        //            .Where(t => t.Categories == Category)
        //            .ToList());


        //    }
        //}

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
                    .ToList());


            }
        }


    }
}
