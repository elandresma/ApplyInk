using ApplyInk.Common.Responses;
using ApplyInk.Prism.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ApplyInk.Prism.ViewModels
{

    public class TattoerDetailPageViewModel : ViewModelBase
    {
        private UserResponse _tattoer;
        private ObservableCollection<CategoryResponse> _categories;

        public TattoerDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Tattoer";
        }

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


    }

}
