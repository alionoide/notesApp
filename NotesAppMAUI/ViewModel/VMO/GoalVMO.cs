using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesAppMAUI.ViewModel.VMO
{
    public partial class GoalVMO : ObservableObject
    {
        [ObservableProperty]
        private int iD;
        
        [ObservableProperty]
        private SubjectVMO subject;

        [ObservableProperty]
        private PermissionVMO permission;

        [ObservableProperty]
        private string name;
        
        [ObservableProperty]
        private string text;
        
        [ObservableProperty]
        private double progress;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DueDateHasValue))]
        private DateTime? dueDate;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(AssignedUserNotNull))]
        private UserVMO assignedUser;

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
