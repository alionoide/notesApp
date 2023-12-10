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
        private ObservableCollection<GoalVMO> goals;

        [ObservableProperty]
        private bool isRefreshing;

        [ObservableProperty]
        private bool isGoalsRefreshing;

        public ICommand RefreshCommand { get; set; }
        public ICommand GoToSubjectCommand { get; set; }
        public ICommand RefreshGoalsCommand { get; set; }
        public ICommand GoToGoalCommand { get; set; }


        INotesAPI api;

        public SharedVM(INotesAPI api)
        {
            this.api = api;

            RefreshCommand = new RelayCommand(refreshList);
            GoToSubjectCommand = new RelayCommand<SubjectVMO>(goToSubject);
            RefreshGoalsCommand = new RelayCommand(refreshGoalList);
            GoToGoalCommand = new RelayCommand<GoalVMO>(goToGoal);
        }

        public void Loaded()
        {
            if (User == null)
            {
                User = (App.Current.MainPage as AppShell).CurrentUser;
            }
            refreshList();
            refreshGoalList();
        }

        private void refreshList()
        {
            Subjects = new ObservableCollection<SubjectVMO>(api.GetSharedSubjects(User.ID).Select(o => Converters.Convert(o)));
            IsRefreshing = false;
        }
        private void refreshGoalList()
        {
            Goals = new ObservableCollection<GoalVMO>(api.GetSharedGoals(User.ID).Select(o => Converters.Convert(o)));
            IsGoalsRefreshing = false;
        }

        private void goToSubject(SubjectVMO vmo)
        {
            Shell.Current.GoToAsync($"Subject?", new Dictionary<string, object>
            {
                ["Subject"] = vmo,
                ["User"] = User,
            });
        }
        private void goToGoal(GoalVMO vmo)
        {
            Shell.Current.GoToAsync($"Goal?", new Dictionary<string, object>
            {
                ["Goal"] = vmo,
                ["User"] = User,
            });
        }
    }
}
