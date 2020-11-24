using ApplyInk.Common.Responses;
using ApplyInk.Common.Services;
using ApplyInk.Prism.Helpers;
using ApplyInk.Prism.ItemViewModels;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace ApplyInk.Prism.ViewModels
{
    public class CategoriesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private string _search;
        private List<CategoryResponse> _myCategories;
        private DelegateCommand _searchCommand;
        private ObservableCollection<CategoryItemViewModel> _categories;


        public CategoriesPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Categories;
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
        public ObservableCollection<CategoryItemViewModel> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }



        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
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
            Response response = await _apiService.GetListAsync<CategoryResponse>(
                url,
                "/api",
                "/categories");
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            _myCategories = (List<CategoryResponse>)response.Result;
            ShowUsers();
        }

        private void ShowUsers()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Categories = new ObservableCollection<CategoryItemViewModel>(_myCategories.Select(t => new CategoryItemViewModel(_navigationService)
                {

                    Id = t.Id,
                    Name = t.Name
                })
                    .ToList());
            }
            else
            {
                Categories = new ObservableCollection<CategoryItemViewModel>(_myCategories.Select(t => new CategoryItemViewModel(_navigationService)
                {

                    Id = t.Id,
                    Name = t.Name
                })
                    .Where(t => t.Name.ToLower().Contains(Search.ToLower()))
                    .ToList());


            }
        }


    }
}
