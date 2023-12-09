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
    public partial class NewUserVM : ObservableObject
    {
        [ObservableProperty]
        private UserVMO user;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string passwordConfirm;

        INotesAPI api;

        public ICommand SaveCommand { get; set; }

        public NewUserVM(INotesAPI api)
        {
            this.api = api;

            SaveCommand = new RelayCommand(saveNewUser);
        }

        public void Loaded()
        {
            User = new UserVMO
            {
                ID = -1,
                DisplayName = null,
                Email = string.Empty,
                Username = string.Empty
            };
        }


        private void saveNewUser()
        {
            if (String.IsNullOrWhiteSpace(User.Email))
            {
                ErrorMessage = "Email is invalid";
            }
            else if (String.IsNullOrWhiteSpace(User.Username))
            {
                ErrorMessage = "Email is invalid";
            }
            else if (String.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Password too weak";
            }
            else if (Password != PasswordConfirm)
            {
                ErrorMessage = "New password and confirmation do not match";
            }
            else
            {
                if (String.IsNullOrWhiteSpace(User.DisplayName))
                {
                    User.DisplayName = null;
                }

                ErrorMessage = string.Empty;
                api.AddUser(Converters.Convert(User), Password);
                navigateBack();
            }
        }

        private void navigateBack()
        {
            Password = null;
            PasswordConfirm = null;

            (App.Current.MainPage as AppShell).CurrentUser = null;
            AppShell.Current.GoToAsync($"//Login");
        }
    }
}
