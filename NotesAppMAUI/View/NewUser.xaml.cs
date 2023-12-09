using NotesAppAPI;
using NotesAppMAUI.ViewModel;

namespace NotesAppMAUI.View
{
    public partial class NewUser : ContentPage
    {
        NewUserVM vm;
        public NewUser(NewUserVM vm)
        {
            InitializeComponent();

            this.vm = vm;
            BindingContext = vm;

            Loaded += loadedRelayToVM;
        }

        private void loadedRelayToVM(object sender, EventArgs e)
        {
            vm.Loaded();
        }
    }
}