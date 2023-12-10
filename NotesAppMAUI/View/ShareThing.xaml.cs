using NotesAppAPI;
using NotesAppMAUI.ViewModel;

namespace NotesAppMAUI.View
{
    public partial class ShareThing : ContentPage
    {
        ShareThingVM vm;
        public ShareThing(ShareThingVM vm)
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