using ApplyInk.Common.Enums;
using ApplyInk.Common.Helpers;
using ApplyInk.Common.Requests;
using ApplyInk.Common.Responses;
using ApplyInk.Common.Services;
using ApplyInk.Prism.Helpers;
using ApplyInk.Prism.Views;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ApplyInk.Prism.ViewModels
{
    public class ModifyUserPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IFilesHelper _filesHelper;
        private ImageSource _image;
        private UserResponse _user;      
        private bool _isRunning;
        private bool _isEnabled;
        private MediaFile _file;
        private DelegateCommand _changeImageCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _changePasswordCommand;
        private bool _IsapplylnkUser;

        public ModifyUserPageViewModel(
            INavigationService navigationService,
            IApiService apiService,
            IFilesHelper filesHelper)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _filesHelper = filesHelper;
            Title = Languages.ModifyUser;
            IsEnabled = true;
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            User = token.User;
            Image = User.ImageFullPath;
            IsapplylnkUser = User.LoginType == LoginType.Applylnk;


        }

        public bool IsapplylnkUser
        {
            get => _IsapplylnkUser;
            set => SetProperty(ref _IsapplylnkUser, value);
        }


        public DelegateCommand ChangeImageCommand => _changeImageCommand ??
            (_changeImageCommand = new DelegateCommand(ChangeImageAsync));

        public DelegateCommand SaveCommand => _saveCommand ??
            (_saveCommand = new DelegateCommand(SaveAsync));

        public DelegateCommand ChangePasswordCommand => _changePasswordCommand ??
            (_changePasswordCommand = new DelegateCommand(ChangePasswordAsync));

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
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

   
        private async void ChangeImageAsync()
        {

            if (!IsapplylnkUser)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ChangeOnSocialNetwork, Languages.Accept);
                return;
            }

            await CrossMedia.Current.Initialize();

            string source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.PictureSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.FromCamera);

            if (source == Languages.Cancel)
            {
                _file = null;
                return;
            }

            if (source == Languages.FromCamera)
            {
                if (!CrossMedia.Current.IsCameraAvailable)
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.NoCameraSupported, Languages.Accept);
                    return;
                }

                _file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.NoGallerySupported, Languages.Accept);
                    return;
                }

                _file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (_file != null)
            {
                Image = ImageSource.FromStream(() =>
                {
                    System.IO.Stream stream = _file.GetStream();
                    return stream;
                });
            }
        }

        private async void SaveAsync()
        {
            bool isValid = await ValidateDataAsync();
            if (!isValid)
            {
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

            byte[] imageArray = null;
            if (_file != null)
            {
                imageArray = _filesHelper.ReadFully(_file.GetStream());
            }

            UserRequest request = new UserRequest
            {
                Address = User.Address,                             
                Email = User.Email,
                FirstName = User.FirstName,
                ImageArray = imageArray,
                LastName = User.LastName,
                Password = "123456", // Doen't matter, it's only to pass the data annotation
                Phone = User.PhoneNumber
            };

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.ModifyUserAsync(url, "/api", "/Account", request, token.Token);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                if (response.Message == "Error001")
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.Error001, Languages.Accept);
                }
                else if (response.Message == "Error004")
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.Error004, Languages.Accept);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                }

                return;
            }

            UserResponse updatedUser = (UserResponse)response.Result;
            token.User = updatedUser;
            Settings.Token = JsonConvert.SerializeObject(token);
            TattoerMasterDetailPageViewModel.GetInstance().LoadUser();
            await App.Current.MainPage.DisplayAlert(Languages.Ok, Languages.ChangeUserMessage, Languages.Accept);
        }

        private async Task<bool> ValidateDataAsync()
        {
         
            if (string.IsNullOrEmpty(User.FirstName))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.FirstNameError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.LastName))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.LastNameError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.Address))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.AddressError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.PhoneNumber))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PhoneError, Languages.Accept);
                return false;
            }
          
            return true;
        }

        private async void ChangePasswordAsync()
        {

            if (!IsapplylnkUser)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ChangeOnSocialNetwork, Languages.Accept);
                return;
            }

            await _navigationService.NavigateAsync(nameof(ChangePasswordPage));
        }

    }

}
