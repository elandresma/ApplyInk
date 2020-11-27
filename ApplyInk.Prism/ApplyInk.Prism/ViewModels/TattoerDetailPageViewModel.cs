using ApplyInk.Common.Helpers;
using ApplyInk.Common.Responses;
using ApplyInk.Prism.Helpers;
using ApplyInk.Prism.ItemViewModels;
using ApplyInk.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;

namespace ApplyInk.Prism.ViewModels
{

    public class TattoerDetailPageViewModel : ViewModelBase
    {
        private UserResponse _tattoer;
        private readonly INavigationService _navigationService;
        private ObservableCollection<CategoryResponse> _categories;
        private DelegateCommand _seeCommand;

        public TattoerDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Tattoer";
        }

        public DelegateCommand seeCommand => _seeCommand ?? (_seeCommand = new DelegateCommand(ScheduleAsync));

        public UserResponse Tattoer
        {
            get => _tattoer;
            set => SetProperty(ref _tattoer, value);
        }

        public ObservableCollection<CategoryResponse> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("tattoer"))
            {
                Tattoer = parameters.GetValue<UserResponse>("tattoer");
                Title = Languages.TattoerDetails;
                Categories = new ObservableCollection<CategoryResponse>(Tattoer.Categories);
            }
        }

        private async void ScheduleAsync()
        {
            if (Settings.IsLogin)
            {
                NavigationParameters parameters = new NavigationParameters
                {
                { "tattoer", Tattoer }
                };

                await _navigationService.NavigateAsync(nameof(Schedule), parameters);
            }
            else {
                await App.Current.MainPage.DisplayAlert(Languages.Error, "Debe loguearse", Languages.Accept);
            }
        }



    }

}
