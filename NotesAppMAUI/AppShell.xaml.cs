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

            Routing.RegisterRoute(nameof(SubjectList), typeof(SubjectList));
            Routing.RegisterRoute(nameof(Subject), typeof(Subject));
            Routing.RegisterRoute(nameof(Goal), typeof(Goal));
            Routing.RegisterRoute(nameof(Task), typeof(Task));
        }
    }
}