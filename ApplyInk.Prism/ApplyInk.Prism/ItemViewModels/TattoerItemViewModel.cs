using ApplyInk.Common.Responses;
using ApplyInk.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplyInk.Prism.ItemViewModels
{

    public class TattoerItemViewModel : UserResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectTattoerCommand;

        public TattoerItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectTattoerCommand => _selectTattoerCommand ?? (_selectTattoerCommand = new DelegateCommand(SelectTattoerAsync));

        private async void SelectTattoerAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "tattoer", this }
            };

            await _navigationService.NavigateAsync(nameof(TattoerDetailPage), parameters);
        }
    }

}
