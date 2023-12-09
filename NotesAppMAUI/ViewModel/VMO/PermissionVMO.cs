using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesAppMAUI.ViewModel.VMO
{
    public partial class PermissionVMO : ObservableObject
    {
        [ObservableProperty]
        private int iD;
        
        [ObservableProperty]
        private string name;
        
        [ObservableProperty]
        private bool isAdmin;
        
        [ObservableProperty]
        private bool canCUD;
        
        [ObservableProperty]
        private bool canShare;
        
        [ObservableProperty]
        private bool canAssign;

        [ObservableProperty]
        private bool canProgress;
    }
}
