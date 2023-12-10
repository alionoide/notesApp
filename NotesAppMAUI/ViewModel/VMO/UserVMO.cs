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
        [NotifyPropertyChangedFor(nameof(NameToShow))]
        private string username;
        
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NameToShow))]
        private string displayName;

        public string NameToShow
        {
            get { return DisplayName == null ? Username : DisplayName; }
        }

        [ObservableProperty]
        private PermissionVMO permission;
    }
}
