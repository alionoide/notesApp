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
    public partial class TaskItemVM : ObservableObject
    {
        [ObservableProperty]
        private UserVMO user;

        [ObservableProperty]
        private TaskItemVMO taskItem;

        [ObservableProperty]
        private string errorMessage;

        public ICommand SaveCommand { get; set; }


        INotesAPI api;

        public TaskItemVM(INotesAPI api)
        {
            this.api = api;

            SaveCommand = new RelayCommand(save);
        }

        public void Loaded()
        {
            if (User == null)
            {
                User = (App.Current.MainPage as AppShell).CurrentUser;
            }
        }

        private void save()
        {
            if(String.IsNullOrEmpty(TaskItem.Text))
            {
                ErrorMessage = "Task text must not be empty";
            }
            else
            {
                api.UpdateTask(Converters.Convert(TaskItem));
            }
        }
    }
}
