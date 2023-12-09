using NotesAppAPI;
using NotesAppMAUI.ViewModel;

namespace NotesAppMAUI.View
{
    public partial class Shared : ContentPage
    {
        SharedVM vm;
        public Shared(SharedVM vm)
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