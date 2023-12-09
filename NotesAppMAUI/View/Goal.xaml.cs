using NotesAppAPI;
using NotesAppMAUI.ViewModel;

namespace NotesAppMAUI.View
{
    public partial class Goal : ContentPage
    {
        GoalVM vm;
        public Goal(GoalVM vm)
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