using NotesAppAPI;
using NotesAppMAUI.ViewModel;

namespace NotesAppMAUI.View
{
    public partial class Subject : ContentPage
    {
        SubjectVM vm;
        public Subject(SubjectVM vm)
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