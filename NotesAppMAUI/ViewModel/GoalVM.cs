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
    [QueryProperty(nameof(Goal), "Goal")]
    public partial class GoalVM : ObservableObject
    {
        [ObservableProperty]
        private UserVMO user;

        [ObservableProperty]
        private GoalVMO goal;

        [ObservableProperty]
        private ObservableCollection<TaskItemVMO> tasks;

        [ObservableProperty]
        private ObservableCollection<UserVMO> avaliableUsers;

        [ObservableProperty]
        private TimeSpan? time;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private bool editModeEnabled;

        [ObservableProperty]
        private bool isRefreshing;

        public ICommand RefreshCommand { get; set; }
        public ICommand AddTaskCommand { get; set; }
        public ICommand GoToTaskCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ShareCommand { get; set; }



        INotesAPI api;

        public GoalVM(INotesAPI api)
        {
            this.api = api;

            RefreshCommand = new RelayCommand(refreshList);
            AddTaskCommand = new RelayCommand(addTask);
            GoToTaskCommand = new RelayCommand<TaskItemVMO>(goToTask);
            SaveCommand = new RelayCommand(save);
            DeleteCommand = new RelayCommand(delete);
            EditCommand = new RelayCommand(edit);
            ShareCommand = new RelayCommand(share);
        }

        public void Loaded()
        {
            User = (App.Current.MainPage as AppShell).CurrentUser;

            Time = Goal.DueDate.HasValue ? new TimeSpan(Goal.DueDate.Value.Hour, Goal.DueDate.Value.Minute, Goal.DueDate.Value.Second) : null;
            refreshList();
        }

        private void refreshList()
        {
            Tasks = new ObservableCollection<TaskItemVMO>(api.GetTasks(Goal.ID).Select(o => Converters.Convert(o)));
            IsRefreshing = false;
        }

        private void addTask()
        {
            if (Goal.Permission.CanCUD)
            {
                var newTask = new TaskItemVMO
                {
                    ID = -1,
                    Text = "New task",
                    Goal = Goal,
                    AssignedUser = null,
                    DueDate = null,
                    Progress = 0,
                    Permission = Goal.Permission
                };

                newTask.ID = api.AddTask(Converters.Convert(newTask));
                goToTask(newTask); 
            }
        }

        private void goToTask(TaskItemVMO vmo)
        {
            ErrorMessage = string.Empty;
            vmo.Permission = Goal.Permission;
            Shell.Current.GoToAsync($"TaskItem?", new Dictionary<string, object>
            {
                ["TaskItem"] = vmo,
                ["User"] = User,
            });
        }

        private void save()
        {
            if (Goal.Permission.CanCUD)
            {
                if (String.IsNullOrEmpty(Goal.Name))
                {
                    ErrorMessage = "Goal name must not be empty";
                }
                else
                {
                    Goal.DueDate = Goal.DueDate.HasValue && Time.HasValue 
                        ? new DateTime(Goal.DueDate.Value.Year, Goal.DueDate.Value.Month, Goal.DueDate.Value.Day, Time.Value.Hours, Time.Value.Minutes, Time.Value.Seconds)
                        : Goal.DueDate;
                    EditModeEnabled = false;
                    api.UpdateGoal(Converters.Convert(Goal));
                } 
            }
        }

        private void edit()
        {
            if (Goal.Permission.CanCUD)
            {
                EditModeEnabled = !EditModeEnabled;
                if (EditModeEnabled)
                {
                    int prevAssignedID = -1;
                    if (Goal.AssignedUser != null)
                    {
                        prevAssignedID = Goal.AssignedUser.ID;
                    }

                    AvaliableUsers = new ObservableCollection<UserVMO>(api.GetAvaliableUsers(Goal.ID).Select(Converters.Convert));

                    if (prevAssignedID != -1)
                    {
                        Goal.AssignedUser = AvaliableUsers.Where(o => o.ID == prevAssignedID).FirstOrDefault();
                    }
                } 
            }
        }

        private async void delete()
        {
            if (Goal.Permission.CanCUD)
            {
                bool confirm = await AppShell.Current.DisplayAlert("Confirm Delete", "Are you sure you want to delete \"" + Goal.Name + "\"?", "Yes, Delete", "No, Cancel");
                if (confirm)
                {
                    api.DeleteGoal(Converters.Convert(Goal));
                    await AppShell.Current.GoToAsync($"..");
                }
            }
        }

        private void share()
        {
            if (Goal.Permission.CanShare)
            {
                Shell.Current.GoToAsync($"ShareThing?", new Dictionary<string, object>
                {
                    ["Goal"] = Goal,
                    ["User"] = User,
                });
            }
        }
    }
}
