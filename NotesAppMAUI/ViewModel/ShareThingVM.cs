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
    [QueryProperty(nameof(Goal), "Goal")]
    [QueryProperty(nameof(Subject), "Subject")]
    [QueryProperty(nameof(User), "User")]
    public partial class ShareThingVM : ObservableObject
    {
        [ObservableProperty]
        private UserVMO user;

        [ObservableProperty]
        private UserVMO selectedUser;

        [ObservableProperty]
        private ObservableCollection<UserVMO> avaliableUsers;

        [ObservableProperty]
        private ObservableCollection<UserVMO> currentUsers;

        [ObservableProperty]
        private PermissionVMO selectedPermission;

        [ObservableProperty]
        private ObservableCollection<PermissionVMO> avaliablePermissions;

        [ObservableProperty]
        private GoalVMO goal;

        [ObservableProperty]
        private SubjectVMO subject;

        [ObservableProperty]
        private string errorMessage;

        INotesAPI api;

        public ICommand ShareCommand { get; set; }
        public ICommand RemoveUserCommand { get; set; }

        public ShareThingVM(INotesAPI api)
        {
            this.api = api;

            ShareCommand = new RelayCommand(share);
            RemoveUserCommand = new RelayCommand<UserVMO>(removeUser);
        }

        public void Loaded()
        {
            User = (App.Current.MainPage as AppShell).CurrentUser;
            AvaliableUsers = new ObservableCollection<UserVMO>(api.GetAllUsersNotCurrent(User.ID).Select(Converters.Convert));
            AvaliablePermissions = new ObservableCollection<PermissionVMO>(api.GetPermissions().Select(Converters.Convert));
            refreshCurrentUsers();
        }

        private void share()
        {
            if (SelectedPermission == null)
            {
                ErrorMessage = "Must select a permission";
            }
            else if (SelectedUser == null)
            {
                ErrorMessage = "Must select a user to share to";
            }
            else if (Goal != null && !Goal.Permission.CanShare)
            {
                ErrorMessage = "Cannot share, you do not have permission!";
            }
            else if (Subject != null && !Subject.Permission.CanShare)
            {
                ErrorMessage = "Cannot share, you do not have permission!";
            }
            else
            {
                try
                {
                    if (Goal != null)
                    {
                        api.ShareGoal(Goal.ID, SelectedUser.ID, SelectedPermission.ID, User.ID);
                        Goal = null;
                    }
                    if (Subject != null)
                    {
                        api.ShareSubject(Subject.ID, SelectedUser.ID, SelectedPermission.ID, User.ID);
                        Subject = null;
                    }
                    navigateBack();
                }
                catch (Exception)
                {
                    ErrorMessage = "Error while sharing, is user already added?";
                }
            }
        }

        private async void removeUser(UserVMO userToRemove)
        {
            bool confirm = await AppShell.Current.DisplayAlert("Confirm Remove", "Are you sure you want to remove \"" + userToRemove.NameToShow + "\"?", "Yes, Remove", "No, Cancel");
            if (confirm)
            {
                if (Goal != null)
                {
                    api.RemoveUserFromGoal(Goal.ID, userToRemove.ID);
                }
                if (Subject != null)
                {
                    api.RemoveUserFromSubject(Subject.ID, userToRemove.ID);
                }
                refreshCurrentUsers();
            }
        }

        private void refreshCurrentUsers()
        {
            if (Goal != null)
            {
                CurrentUsers = new ObservableCollection<UserVMO>(api.GetUsersGoalSharedWith(Goal.ID).Select(o => Converters.Convert(o)));
            }
            if (Subject != null)
            {
                CurrentUsers = new ObservableCollection<UserVMO>(api.GetUsersSubSharedWith(Subject.ID).Select(o => Converters.Convert(o)));
            }
        }

        private void navigateBack()
        {
            AppShell.Current.GoToAsync($"..");
        }
    }
}
