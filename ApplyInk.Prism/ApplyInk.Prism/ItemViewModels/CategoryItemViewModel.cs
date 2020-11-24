using ApplyInk.Common.Responses;
using ApplyInk.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplyInk.Prism.ItemViewModels
{
    public class CategoryItemViewModel : CategoryResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectCategoryCommand;

        public CategoryItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectCategoryCommand => _selectCategoryCommand ?? (_selectCategoryCommand = new DelegateCommand(SelectCategoryAsync));

        private async void SelectCategoryAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "category", this }
            };

            await _navigationService.NavigateAsync(nameof(TattoersForCategoryPage), parameters);
        }
    }
}
