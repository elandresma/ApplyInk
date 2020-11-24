using ApplyInk.Common.Responses;
using ApplyInk.Common.Services;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Position = Xamarin.Forms.Maps.Position;

namespace ApplyInk.Prism.Views
{
    public partial class MapPage : ContentPage
    {
        private readonly IGeolocatorService _geolocatorService;
        private ApiService _apiService;
        public MapPage(IGeolocatorService geolocatorService, ApiService apiService)
        {
            InitializeComponent();
            _geolocatorService = geolocatorService;
            _apiService = apiService;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MoveMapToCurrentPositionAsync();
            LoadUsersAsync();
        }

        private async void LoadUsersAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return;
            }

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListShop<ShopResponse>(url, "api", "/account/Getshop");

            if (!response.IsSuccess)
            {
                return;
            }

            List<ShopResponse> shops = (List<ShopResponse>)response.Result;
            foreach (ShopResponse shop in shops)
            {
                MyMap.Pins.Add(new Pin
                {
                    Address = shop.Address,
                    Label = shop.Name,
                    Position = new Position(shop.Latitude, shop.Logitude),
                    Type = PinType.Place

                });
            }
        }


        private async void MoveMapToCurrentPositionAsync()
        {
            bool isLocationPermision = await CheckLocationPermisionsAsync();

            if (isLocationPermision)
            {
                MyMap.IsShowingUser = true;

                await _geolocatorService.GetLocationAsync();
                if (_geolocatorService.Latitude != 0 && _geolocatorService.Longitude != 0)
                {
                    Position position = new Position(
                        _geolocatorService.Latitude,
                        _geolocatorService.Longitude);
                    MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                        position,
                        Distance.FromKilometers(.5)));
                }
            }
        }


        private async Task<bool> CheckLocationPermisionsAsync()
        {
            Plugin.Permissions.Abstractions.PermissionStatus permissionLocation = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            Plugin.Permissions.Abstractions.PermissionStatus permissionLocationAlways = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationAlways);
            Plugin.Permissions.Abstractions.PermissionStatus permissionLocationWhenInUse = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);
            bool isLocationEnabled = permissionLocation == Plugin.Permissions.Abstractions.PermissionStatus.Granted ||
                                        permissionLocationAlways == Plugin.Permissions.Abstractions.PermissionStatus.Granted ||
                                        permissionLocationWhenInUse == Plugin.Permissions.Abstractions.PermissionStatus.Granted;
            if (isLocationEnabled)
            {
                return true;
            }

            await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);

            permissionLocation = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            permissionLocationAlways = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationAlways);
            permissionLocationWhenInUse = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);
            return permissionLocation == Plugin.Permissions.Abstractions.PermissionStatus.Granted ||
                    permissionLocationAlways == Plugin.Permissions.Abstractions.PermissionStatus.Granted ||
                    permissionLocationWhenInUse == Plugin.Permissions.Abstractions.PermissionStatus.Granted;
        }

    }

}
