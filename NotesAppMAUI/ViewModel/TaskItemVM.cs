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
    [QueryProperty(nameof(TaskItem), "TaskItem")]
    [QueryProperty(nameof(DueDate), "DueDate")]
    public partial class TaskItemVM : ObservableObject
    {
        [ObservableProperty]
        private UserVMO user;

        [ObservableProperty]
        private TaskItemVMO taskItem;

        [ObservableProperty]
        private ObservableCollection<UserVMO> avaliableUsers;

        [ObservableProperty]
        private TimeSpan? time;

        [ObservableProperty]
        private DateTime dueDate;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private bool editModeEnabled;

        public ICommand SaveCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ProgressChangedCommand { get; set; }


        INotesAPI api;

        public TaskItemVM(INotesAPI api)
        {
            this.api = api;

            SaveCommand = new RelayCommand(save);
            DeleteCommand = new RelayCommand(delete);
            EditCommand = new RelayCommand(edit);
            ProgressChangedCommand = new RelayCommand(save);
        }

        public void Loaded()
        {
            User = (App.Current.MainPage as AppShell).CurrentUser;

            TaskItem.DueDate = DueDate == DateTime.MinValue ? null : DueDate;
            Time = TaskItem.DueDate.HasValue ? new TimeSpan(TaskItem.DueDate.Value.Hour, TaskItem.DueDate.Value.Minute, TaskItem.DueDate.Value.Second) : null;
        }

        private void save()
        {
            if (TaskItem.Permission.CanCUD)
            {
                if (String.IsNullOrEmpty(TaskItem.Text))
                {
                    ErrorMessage = "Task text must not be empty";
                }
                else
                {
                    TaskItem.DueDate = TaskItem.DueDate.HasValue && Time.HasValue
                        ? new DateTime(TaskItem.DueDate.Value.Year, TaskItem.DueDate.Value.Month, TaskItem.DueDate.Value.Day, Time.Value.Hours, Time.Value.Minutes, Time.Value.Seconds)
                        : TaskItem.DueDate;
                    EditModeEnabled = false;
                    api.UpdateTask(Converters.Convert(TaskItem));
                } 
            }
        }
        private void edit()
        {
            if (TaskItem.Permission.CanCUD)
            {
                EditModeEnabled = !EditModeEnabled;
                if (EditModeEnabled)
                {
                    int prevAssignedID = -1;
                    if (TaskItem.AssignedUser != null)
                    {
                        prevAssignedID = TaskItem.AssignedUser.ID;
                    }

                    AvaliableUsers = new ObservableCollection<UserVMO>(api.GetAvaliableUsers(TaskItem.Goal.ID).Select(Converters.Convert));

                    if (prevAssignedID != -1)
                    {
                        TaskItem.AssignedUser = AvaliableUsers.Where(o => o.ID == prevAssignedID).FirstOrDefault();
                    }
                }
            }
        }

        private async void delete()
        {
            if (TaskItem.Permission.CanCUD)
            {
                bool confirm = await AppShell.Current.DisplayAlert("Confirm Delete", "Are you sure you want to delete \"" + TaskItem.Text + "\"?", "Yes, Delete", "No, Cancel");
                if (confirm)
                {
                    api.DeleteTask(Converters.Convert(TaskItem));
                    await AppShell.Current.GoToAsync($"..");
                }
            }
        }
    }
}
