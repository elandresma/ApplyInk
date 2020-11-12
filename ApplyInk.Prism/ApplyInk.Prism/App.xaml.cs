using Prism;
using Prism.Ioc;
using ApplyInk.Prism.ViewModels;
using ApplyInk.Prism.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using ApplyInk.Common.Services;
using Syncfusion.Licensing;

namespace ApplyInk.Prism
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            SyncfusionLicenseProvider.RegisterLicense("MzQ5NzUzQDMxMzgyZTMzMmUzMEU2OHhGUzdqNVpENUxQd3dZUkZNZ3pINHk1K1hxMVhSTmJWU284LytxNGM9");
            InitializeComponent();

            await NavigationService.NavigateAsync($"NavigationPage/{nameof(TattoersPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<TattoersPage, TattoersPageViewModel>();
        }
    }
}
