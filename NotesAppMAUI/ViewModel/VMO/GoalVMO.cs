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
        private DateTime? dueDate;

        [ObservableProperty]
        private UserVMO assignedUser;
    }
}
