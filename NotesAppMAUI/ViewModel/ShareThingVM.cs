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

        public ShareThingVM(INotesAPI api)
        {
            this.api = api;

            ShareCommand = new RelayCommand(share);
        }

        public void Loaded()
        {
            if (User == null)
            {
                User = (App.Current.MainPage as AppShell).CurrentUser;
            }
            AvaliableUsers = new ObservableCollection<UserVMO>(api.GetAllUsersNotCurrent(User.ID).Select(Converters.Convert));
            AvaliablePermissions = new ObservableCollection<PermissionVMO>(api.GetPermissions().Select(Converters.Convert));
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
            else
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
        }

        private void navigateBack()
        {
            AppShell.Current.GoToAsync($"..");
        }
    }
}
