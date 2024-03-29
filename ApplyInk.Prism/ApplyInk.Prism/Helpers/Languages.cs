﻿using ApplyInk.Common.Helpers;
using ApplyInk.Prism.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ApplyInk.Prism.Helpers
{
    public static class Languages
    {
        static Languages()
        {
            CultureInfo ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            Culture = ci.Name;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Culture { get; set; }

        public static string Accept => Resource.Accept;
        public static string Update => Resource.Update;


        public static string ConnectionError => Resource.ConnectionError;

        public static string Error => Resource.Error;

        public static string MyMeetings => Resource.MyMeetings;

        public static string Loading => Resource.Loading;

        public static string SearchTattoer => Resource.SearchTattoer;

        public static string SearchCategory => Resource.SearchCategory;
        public static string MeetingCreated => Resource.MeetingCreated;
        public static string Schedule => Resource.Schedule; 
        public static string Name => Resource.Name;

        public static string Description => Resource.Description;


        public static string Category => Resource.Category;

        public static string Login => Resource.Login;

        public static string ModifyUser => Resource.ModifyUser;

        public static string Email => Resource.Email;

        public static string EmailError => Resource.EmailError;

        public static string EmailPlaceHolder => Resource.EmailPlaceHolder;

        public static string Password => Resource.Password;

        public static string Tattoer => Resource.Tattoer;

        public static string Tattoers => Resource.Tattoers;

        public static string PasswordError => Resource.PasswordError;

        public static string PasswordPlaceHolder => Resource.PasswordPlaceHolder;

        public static string ForgotPassword => Resource.ForgotPassword;

        public static string LoginError => Resource.LoginError;

        public static string Logout => Resource.Logout;

        public static string LoginFirstMessage => Resource.LoginFirstMessage;

        public static string Details => Resource.Details;

        public static string Save => Resource.Save;

        public static string Register => Resource.Register;

        public static string FirstName => Resource.FirstName;

        public static string FirstNameError => Resource.FirstNameError;

        public static string FirstNamePlaceHolder => Resource.FirstNamePlaceHolder;

        public static string LastName => Resource.LastName;

        public static string LastNameError => Resource.LastNameError;

        public static string LastNamePlaceHolder => Resource.LastNamePlaceHolder;

        public static string Address => Resource.Address;

        public static string AddressError => Resource.AddressError;

        public static string AddressPlaceHolder => Resource.AddressPlaceHolder;

        public static string Phone => Resource.Phone;

        public static string PhoneError => Resource.PhoneError;

        public static string PhonePlaceHolder => Resource.PhonePlaceHolder;

        public static string City => Resource.City;

        public static string CityError => Resource.CityError;

        public static string CityPlaceHolder => Resource.CityPlaceHolder;

        public static string Department => Resource.Department;

        public static string DepartmentError => Resource.DepartmentError;

        public static string DepartmentPlaceHolder => Resource.DepartmentPlaceHolder;

        public static string Country => Resource.Country;

        public static string CountryError => Resource.CountryError;

        public static string CountryPlaceHolder => Resource.CountryPlaceHolder;

        public static string PasswordConfirm => Resource.PasswordConfirm;

        public static string PasswordConfirmError1 => Resource.PasswordConfirmError1;

        public static string PasswordConfirmError2 => Resource.PasswordConfirmError2;

        public static string PasswordConfirmPlaceHolder => Resource.PasswordConfirmPlaceHolder;

        public static string Error001 => Resource.Error001;

        public static string Error002 => Resource.Error002;

        public static string Error003 => Resource.Error003;

        public static string Error004 => Resource.Error004;

        public static string Ok => Resource.Ok;

        public static string RegisterMessge => Resource.RegisterMessge;

        public static string PictureSource => Resource.PictureSource;

        public static string Cancel => Resource.Cancel;

        public static string FromCamera => Resource.FromCamera;

        public static string FromGallery => Resource.FromGallery;

        public static string NoCameraSupported => Resource.NoCameraSupported;

        public static string NoGallerySupported => Resource.NoGallerySupported;

        public static string RecoverPassword => Resource.RecoverPassword;

        public static string RecoverPasswordMessage => Resource.RecoverPasswordMessage;

        public static string ChangePassword => Resource.ChangePassword;

        public static string ChangeUserMessage => Resource.ChangeUserMessage;

        public static string ConfirmNewPassword => Resource.ConfirmNewPassword;

        public static string ConfirmNewPasswordError1 => Resource.ConfirmNewPasswordError1;

        public static string ConfirmNewPasswordError2 => Resource.ConfirmNewPasswordError2;

        public static string ConfirmNewPasswordPlaceHolder => Resource.ConfirmNewPasswordPlaceHolder;

        public static string CurrentPassword => Resource.CurrentPassword;

        public static string CurrentPasswordError => Resource.CurrentPasswordError;

        public static string CurrentPasswordPlaceHolder => Resource.CurrentPasswordPlaceHolder;

        public static string NewPassword => Resource.NewPassword;

        public static string NewPasswordError => Resource.NewPasswordError;

        public static string NewPasswordPlaceHolder => Resource.NewPasswordPlaceHolder;

        public static string Error005 => Resource.Error005;

        public static string ChangePassworrdMessage => Resource.ChangePassworrdMessage;

        public static string Yes => Resource.Yes;

        public static string No => Resource.No;

        public static string Total => Resource.Total;

        public static string Items => Resource.Items;

        public static string Delete => Resource.Delete;

        public static string Shop => Resource.Shop;

        public static string Shops => Resource.Shops;

        public static string Categories => Resource.Categories;

        public static string ContactInformation => Resource.ContactInformation;

        public static string BasicProfile => Resource.BasicProfile;

        public static string TattoerDetails => Resource.TattoerDetails;

        public static string TattoersForCategory => Resource.TattoersForCategory;

        public static string CategoryError => Resource.CategoryError;

        public static string CategoryPlaceHolder => Resource.CategoryPlaceHolder;

        public static string ChangeOnSocialNetwork => Resource.ChangeOnSocialNetwork;

        public static string LoginFacebook => Resource.LoginFacebook;
        public static string Client => Resource.Client;

        public static string Status => Resource.Status;
        public static string CreateMeeting => Resource.CreateMeeting;
        public static string MeetingCancelled => Resource.MeetingCancelled;
        public static string SeeSchedule => Resource.SeeSchedule;




    }

}
