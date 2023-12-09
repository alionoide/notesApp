using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotesAppAPI;
using NotesAppAPI.Models;
using NotesAppMAUI.ViewModel.VMO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotesAppMAUI.ViewModel
{
    [QueryProperty(nameof(User), "User")]
    public partial class SharedVM : ObservableObject
    {
        [ObservableProperty]
        private UserVMO user;

        [ObservableProperty]
        private ObservableCollection<SubjectVMO> subjects;

        [ObservableProperty]
        private bool isRefreshing;

        public ICommand RefreshCommand { get; set; }
        public ICommand AddSubjectCommand { get; set; }
        public ICommand GoToSubjectCommand { get; set; }


        INotesAPI api;

        public SharedVM(INotesAPI api)
        {
            this.api = api;

            RefreshCommand = new RelayCommand(refreshList);
            AddSubjectCommand = new RelayCommand(addSubject);
            GoToSubjectCommand = new RelayCommand<SubjectVMO>(goToSubject);
        }

        public void Loaded()
        {
            if (User == null)
            {
                User = (App.Current.MainPage as AppShell).CurrentUser;
            }
            refreshList();
        }

        private void refreshList()
        {
            Subjects = new ObservableCollection<SubjectVMO>(api.GetSubjects(User.ID).Select(o => Converters.Convert(o)));
            IsRefreshing = false;
        }

        private void addSubject()
        {
            var newSubject = new SubjectVMO 
            {
                ID = -1,
                Name = "New Subject",
                Description = string.Empty,
                Color = null,
                Owner = User,
                Permission = new PermissionVMO
                {
                    ID = 0,
                    Name = "Owner",
                    IsAdmin = true,
                    CanCUD = true,
                    CanAssign = true,
                    CanProgress = true,
                    CanShare = true,
                }
            };

            newSubject.ID = api.AddSubject(Converters.Convert(newSubject));
            goToSubject(newSubject);
        }

        private void goToSubject(SubjectVMO vmo)
        {
            Shell.Current.GoToAsync($"Subject?", new Dictionary<string, object>
            {
                ["Subject"] = vmo,
                ["User"] = User,
            });
        }
    }
}
