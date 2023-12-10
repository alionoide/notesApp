using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesAppMAUI.ViewModel.VMO
{
    public partial class TaskItemVMO : ObservableObject
    {
        [ObservableProperty]
        private int iD;
        
        [ObservableProperty]
        private GoalVMO goal;
        
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(AssignedUserNotNull))]
        private UserVMO assignedUser;
        
        [ObservableProperty]
        private string text;
        
        [ObservableProperty]
        private double progress;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DueDateHasValue))]
        private DateTime? dueDate;

        [ObservableProperty]
        private PermissionVMO permission;

        public bool AssignedUserNotNull
        {
            get { return AssignedUser != null; }
        }
        public bool DueDateHasValue
        {
            get { return DueDate.HasValue; }
        }
    }
}
