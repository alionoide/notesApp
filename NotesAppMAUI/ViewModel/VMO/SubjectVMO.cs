using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesAppMAUI.ViewModel.VMO
{
    public partial class SubjectVMO : ObservableObject
    {
        [ObservableProperty]
        private int iD;

        [ObservableProperty]
        private UserVMO owner;

        [ObservableProperty]
        private PermissionVMO permission;

        [ObservableProperty]
        private string name;
        
        [ObservableProperty]
        private string description;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ColorAsColor))]
        private string color;

        public Color ColorAsColor
        {
            get { return Microsoft.Maui.Graphics.Color.FromArgb(Color); }
        }
    }
}
