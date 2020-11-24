using ApplyInk.Common.Helpers;
using ApplyInk.Common.Models;
using ApplyInk.Common.Responses;
using ApplyInk.Prism.Helpers;
using ApplyInk.Prism.ItemViewModels;
using ApplyInk.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ApplyInk.Prism.ViewModels
{
    public class TattoerMasterDetailPageViewModel : ViewModelBase
    {
        private static TattoerMasterDetailPageViewModel _instance;
        private readonly INavigationService _navigationService;
        private UserResponse _user;

        public TattoerMasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _instance = this;
            _navigationService = navigationService;
            LoadMenus();
            LoadUser();
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public static TattoerMasterDetailPageViewModel GetInstance()
        {
            return _instance;
        }

        public void LoadUser()
        {
            if (Settings.IsLogin)
            {
                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
                User = token.User;
                LoadMenus();
            }
        }


        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        private void LoadMenus()
        {

            if (Settings.IsLogin)
            {

                List<Menu> menus = new List<Menu>
                {

                      new Menu
                    {
                        Icon = "ic_location_on",
                        PageName = $"{nameof(MapPage)}",
                        Title = Languages.Shop
                    },

                    new Menu
                    {
                        Icon = "ic_action_assignment_ind",
                        PageName = $"{nameof(TattoersPage)}",
                        Title = Languages.Tattoers
                    },

                    new Menu
                    {
                        Icon = "ic_action_person",
                        PageName = $"{nameof(ModifyUserPage)}",
                        Title = Languages.ModifyUser,
                        IsLoginRequired = true
                    },

                    new Menu
                    {
                        Icon = "ic_action_list_alt",
                        PageName = $"{nameof(TattoersForCategoryPage)}",
                        Title = Languages.TattoersForCategory,
                        IsLoginRequired = true
                    },

                    new Menu
                    {
                        Icon = "ic_action_exit",
                        PageName = $"{nameof(LoginPage)}",
                        Title = Settings.IsLogin ? Languages.Logout : Languages.Login
                    }
                };

                  Menus = new ObservableCollection<MenuItemViewModel>(
                     menus.Select(m => new MenuItemViewModel(_navigationService)
                     {
                       Icon = m.Icon,
                       PageName = m.PageName,
                       Title = m.Title,
                       IsLoginRequired = m.IsLoginRequired
                     }).ToList());

            }else {
                List<Menu> menus = new List<Menu>
                {
                    new Menu
                    {
                        Icon = "ic_action_assignment_ind",
                        PageName = $"{nameof(TattoersPage)}",
                        Title = Languages.Tattoers
                    },
                     new Menu
                    {
                        Icon = "ic_action_exit",
                        PageName = $"{nameof(LoginPage)}",
                        Title = Settings.IsLogin ? Languages.Logout : Languages.Login
                    }
                };

                Menus = new ObservableCollection<MenuItemViewModel>(
                   menus.Select(m => new MenuItemViewModel(_navigationService)
                   {
                       Icon = m.Icon,
                       PageName = m.PageName,
                       Title = m.Title,
                       IsLoginRequired = m.IsLoginRequired
                   }).ToList());







            }
        }
    }

}
