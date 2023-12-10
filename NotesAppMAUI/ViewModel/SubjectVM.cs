using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotesAppAPI;
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
    [QueryProperty(nameof(Subject), "Subject")]
    public partial class SubjectVM : ObservableObject
    {
        [ObservableProperty]
        private UserVMO user;

        [ObservableProperty]
        private SubjectVMO subject;

        [ObservableProperty]
        private ObservableCollection<GoalVMO> goals;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private bool editModeEnabled;

        [ObservableProperty]
        private bool isRefreshing;

        public ICommand RefreshCommand { get; set; }
        public ICommand AddGoalCommand { get; set; }
        public ICommand GoToGoalCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ShareCommand { get; set; }


        INotesAPI api;

        public SubjectVM(INotesAPI api)
        {
            this.api = api;

            RefreshCommand = new RelayCommand(refreshList);
            AddGoalCommand = new RelayCommand(addGoal);
            GoToGoalCommand = new RelayCommand<GoalVMO>(goToGoal);
            SaveCommand = new RelayCommand(save);
            DeleteCommand = new RelayCommand(delete);
            EditCommand = new RelayCommand(edit);
            ShareCommand = new RelayCommand(share);
        }

        public void Loaded()
        {
            User = (App.Current.MainPage as AppShell).CurrentUser;
            
            refreshList();
        }

        private void refreshList()
        {
            Goals = new ObservableCollection<GoalVMO>(api.GetGoals(Subject.ID).Select(o => Converters.Convert(o)));
            IsRefreshing = false;
        }

        private void addGoal()
        {
            if (Subject.Permission.CanCUD)
            {
                var newGoal = new GoalVMO
                {
                    ID = -1,
                    Name = "New Goal",
                    Subject = Subject,
                    AssignedUser = null,
                    DueDate = null,
                    Permission = Subject.Permission
                };

                newGoal.ID = api.AddGoal(Converters.Convert(newGoal));
                goToGoal(newGoal); 
            }
        }

        private void goToGoal(GoalVMO vmo)
        {
            ErrorMessage = string.Empty;
            vmo.Permission = Subject.Permission;
            Shell.Current.GoToAsync($"Goal?", new Dictionary<string, object>
            {
                ["Goal"] = vmo,
                ["User"] = User,
            });
        }
        private void save()
        {
            if (Subject.Permission.CanCUD)
            {
                if (String.IsNullOrEmpty(Subject.Name))
                {
                    ErrorMessage = "Subject name must not be empty";
                }
                else
                {
                    EditModeEnabled = false;
                    api.UpdateSubject(Converters.Convert(Subject));
                } 
            }
        }

        private void edit()
        {
            if (Subject.Permission.CanCUD)
            {
                EditModeEnabled = !EditModeEnabled;
            }
        }

        private async void delete()
        {
            if (Subject.Permission.CanCUD)
            {
                bool confirm = await AppShell.Current.DisplayAlert("Confirm Delete", "Are you sure you want to delete \"" + Subject.Name + "\"?", "Yes, Delete", "No, Cancel");
                if (confirm)
                {
                    api.DeleteSubject(Converters.Convert(Subject));
                    await AppShell.Current.GoToAsync($"..");
                }
            }
        }

        private void share()
        {
            if (Subject.Permission.CanShare)
            {
                Shell.Current.GoToAsync($"ShareThing?", new Dictionary<string, object>
                {
                    ["Subject"] = Subject,
                    ["User"] = User,
                });
            }
        }
    }
}
