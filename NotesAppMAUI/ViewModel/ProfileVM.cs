using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotesAppAPI;
using NotesAppAPI.Models;
using NotesAppMAUI.ViewModel.VMO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotesAppMAUI.ViewModel
{
    [QueryProperty(nameof(User), "User")]
    public partial class ProfileVM : ObservableObject
    {
        [ObservableProperty]
        private UserVMO user;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string newPassword;

        [ObservableProperty]
        private string newPasswordConfirm;

        INotesAPI api;

        public ICommand SaveCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        public ProfileVM(INotesAPI api)
        {
            this.api = api;

            SaveCommand = new RelayCommand(saveProfile);
            ChangePasswordCommand = new RelayCommand(changePassword);
            LogoutCommand = new RelayCommand(logout);
        }

        public void Loaded()
        {
            User = (App.Current.MainPage as AppShell).CurrentUser;
        }


        private void saveProfile()
        {
            if (String.IsNullOrWhiteSpace(User.Email))
            {
                ErrorMessage = "Email is invalid";
            }
            else
            {
                if (String.IsNullOrWhiteSpace(User.DisplayName))
                {
                    User.DisplayName = null;
                }

                ErrorMessage = string.Empty;
                api.UpdateUser(Converters.Convert(User));
                (App.Current.MainPage as AppShell).CurrentUser = User;
            }
        }

        private void changePassword()
        {
            if (String.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Password Incorrect";
            }
            else if (String.IsNullOrWhiteSpace(NewPassword))
            {
                ErrorMessage = "New password too weak";
            }
            else if (NewPassword != NewPasswordConfirm)
            {
                ErrorMessage = "New password and confirmation do not match";
            }
            else
            {
                if (api.ChangePassword(User.ID, Password, NewPassword))
                {
                    ErrorMessage = String.Empty;
                    logout();
                }
                else
                {
                    ErrorMessage = "Password Incorrect";
                }
            }
        }

        private void logout()
        {
            Password = null;
            NewPassword = null;
            NewPasswordConfirm = null;

            (App.Current.MainPage as AppShell).CurrentUser = null;
            AppShell.Current.GoToAsync($"//Login");
        }
    }
}
