using NotesAppMAUI.View;
using NotesAppMAUI.ViewModel.VMO;

namespace NotesAppMAUI
{
    public partial class AppShell : Shell
    {
        public UserVMO CurrentUser = null;

        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Subject), typeof(Subject));
            Routing.RegisterRoute(nameof(Goal), typeof(Goal));
            Routing.RegisterRoute(nameof(TaskItem), typeof(TaskItem));
            Routing.RegisterRoute(nameof(NewUser), typeof(NewUser));
            Routing.RegisterRoute(nameof(Share), typeof(Share));
        }
    }
}