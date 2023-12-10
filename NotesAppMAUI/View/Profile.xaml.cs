using NotesAppAPI;
using NotesAppMAUI.ViewModel;

namespace NotesAppMAUI.View
{
    public partial class Profile : ContentPage
    {
        ProfileVM vm;
        public Profile(ProfileVM vm)
        {
            InitializeComponent();

            this.vm = vm;
            BindingContext = vm;

            Appearing += loadedRelayToVM;
        }

        private void loadedRelayToVM(object sender, EventArgs e)
        {
            vm.Loaded();
        }
    }
}