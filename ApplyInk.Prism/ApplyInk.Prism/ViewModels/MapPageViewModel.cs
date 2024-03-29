﻿using ApplyInk.Common.Responses;
using ApplyInk.Common.Services;
using ApplyInk.Prism.Helpers;
using ApplyInk.Prism.ItemViewModels;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
namespace ApplyInk.Prism.ViewModels
{
    public class MapPageViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly IApiService _apiService;
    private ObservableCollection<UserResponse> _users;
    private bool _isRunning;
    private List<UserResponse> _myUsers; 
    private ObservableCollection<TattoerItemViewModel> _tattoers;


    public MapPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
    {
        _navigationService = navigationService;
        _apiService = apiService;
        Title = Languages.Shops;
        LoadUsersAsync();
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


}
}
