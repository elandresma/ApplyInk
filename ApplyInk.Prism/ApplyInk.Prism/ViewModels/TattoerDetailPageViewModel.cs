using ApplyInk.Common.Responses;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplyInk.Prism.ViewModels
{

    public class TattoerDetailPageViewModel : ViewModelBase
    {
        private UserResponse _tattoer;

        public TattoerDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Tattoer";
        }

        public UserResponse User
        {
            get => _tattoer;
            set => SetProperty(ref _tattoer, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("tattoer"))
            {
                User = parameters.GetValue<UserResponse>("tattoer");
                Title = User.FullName;
            }
        }


    }

}
