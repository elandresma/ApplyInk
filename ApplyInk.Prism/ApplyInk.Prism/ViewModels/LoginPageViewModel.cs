using ApplyInk.Common.Helpers;
using ApplyInk.Common.Models;
using ApplyInk.Common.Requests;
using ApplyInk.Common.Responses;
using ApplyInk.Common.Services;
using ApplyInk.Prism.Helpers;
using ApplyInk.Prism.Views;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ApplyInk.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private bool _isEnabled;
        private string _password;
        private DelegateCommand _loginCommand;
        private DelegateCommand _registerCommand;
        private DelegateCommand _forgotPasswordCommand;
        private readonly IFacebookClient _facebookService = CrossFacebookClient.Current;
        private DelegateCommand _loginFacebookCommand;


        public LoginPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Login;
            IsEnabled = true;
        }

        public DelegateCommand LoginFacebookCommand => _loginFacebookCommand ?? (_loginFacebookCommand = new DelegateCommand(LoginFacebookAsync));

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(LoginAsync));

        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(RegisterAsync));

        public DelegateCommand ForgotPasswordCommand => _forgotPasswordCommand ?? (_forgotPasswordCommand = new DelegateCommand(ForgotPasswordAsync));

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

        public string Email { get; set; }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private async void LoginAsync()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailError,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordError,
                    Languages.Accept);
                return;
            }

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
            TokenRequest request = new TokenRequest
            {
                Password = Password,
                Username = Email
            };

            Response response = await _apiService.GetTokenAsync(url, "api", "/Account/CreateToken", request);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.LoginError, Languages.Accept);
                Password = string.Empty;
                return;
            }

            TokenResponse token = (TokenResponse)response.Result;
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.IsLogin = true;

            IsRunning = false;
            IsEnabled = true;

            await _navigationService.NavigateAsync($"/{nameof(TattoerMasterDetailPage)}/NavigationPage/{nameof(TattoersPage)}");
            Password = string.Empty;

        }

        private async void ForgotPasswordAsync()
        {
            await _navigationService.NavigateAsync(nameof(RecoverPasswordPage));
        }

        private async void RegisterAsync()
        {
            await _navigationService.NavigateAsync(nameof(RegisterPage));
        }

        //private async void LoginFacebookAsync()
        //{
        //    try
        //    {
        //        if (_facebookService.IsLoggedIn)
        //        {
        //            _facebookService.Logout();
        //        }

        //        async void userDataDelegate(object sender, FBEventArgs<string> e)
        //        {
        //            switch (e.Status)
        //            {
        //                case FacebookActionStatus.Completed:
        //                    FacebookProfile facebookProfile = await Task.Run(() => JsonConvert.DeserializeObject<FacebookProfile>(e.Data));
        //                    await LoginFacebookAsync(facebookProfile);
        //                    break;
        //                case FacebookActionStatus.Canceled:
        //                    await App.Current.MainPage.DisplayAlert("Facebook Auth", "Canceled", "Ok");
        //                    break;
        //                case FacebookActionStatus.Error:
        //                    await App.Current.MainPage.DisplayAlert("Facebook Auth", "Error", "Ok");
        //                    break;
        //                case FacebookActionStatus.Unauthorized:
        //                    await App.Current.MainPage.DisplayAlert("Facebook Auth", "Unauthorized", "Ok");
        //                    break;
        //            }

        //            _facebookService.OnUserData -= userDataDelegate;
        //        }

        //        _facebookService.OnUserData += userDataDelegate;

        //        string[] fbRequestFields = { "email", "first_name", "picture.width(999)", "gender", "last_name" };
        //        string[] fbPermisions = { "email" };
        //        await _facebookService.RequestUserDataAsync(fbRequestFields, fbPermisions);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.ToString());
        //    }
        //}

        private async void LoginFacebookAsync()
        {
            try
            {
                if (_facebookService.IsLoggedIn)
                {
                    _facebookService.Logout();
                }

                async void userDataDelegate(object sender, FBEventArgs<string> e)
                {
                    switch (e.Status)
                    {
                        case FacebookActionStatus.Completed:
                            FacebookProfile facebookProfile = await Task.Run(() => JsonConvert.DeserializeObject<FacebookProfile>(e.Data));
                            await LoginFacebookAsync(facebookProfile);
                            break;
                        case FacebookActionStatus.Canceled:
                            await App.Current.MainPage.DisplayAlert("Facebook Auth", "Canceled", "Ok");
                            break;
                        case FacebookActionStatus.Error:
                            await App.Current.MainPage.DisplayAlert("Facebook Auth", "Error", "Ok");
                            break;
                        case FacebookActionStatus.Unauthorized:
                            await App.Current.MainPage.DisplayAlert("Facebook Auth", "Unauthorized", "Ok");
                            break;
                    }

                    _facebookService.OnUserData -= userDataDelegate;
                }

                _facebookService.OnUserData += userDataDelegate;

                string[] fbRequestFields = { "email", "first_name", "picture.width(999)", "gender", "last_name" };
                string[] fbPermisions = { "email" };
                await _facebookService.RequestUserDataAsync(fbRequestFields, fbPermisions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }



        private async Task LoginFacebookAsync(FacebookProfile facebookProfile)
        {
            IsRunning = true;
            IsEnabled = false;

            string url = App.Current.Resources["UrlAPI"].ToString();

            Response response = await _apiService.GetTokenAsync(url, "api", "/Account/LoginFacebook", facebookProfile);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.LoginError, Languages.Accept);
                Password = string.Empty;
                return;
            }

            TokenResponse token = (TokenResponse)response.Result;
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.IsLogin = true;

            IsRunning = false;
            IsEnabled = true;

            await _navigationService.NavigateAsync($"/{nameof(TattoerMasterDetailPage)}/NavigationPage/{nameof(TattoersPage)}");
            Password = string.Empty;
        }



    }

}
