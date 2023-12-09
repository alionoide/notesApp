using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NotesAppMAUI.ViewModel.VMO
{
    public partial class UserVMO : ObservableObject              
    {
        [ObservableProperty]
        private int iD;
        
        [ObservableProperty]
        private string username;
        
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string displayName;
    }
}
