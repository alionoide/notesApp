using NotesAppAPI;
using NotesAppMAUI.ViewModel;

namespace NotesAppMAUI.View
{
    public partial class TaskItem : ContentPage
    {
        TaskItemVM vm;
        public TaskItem(TaskItemVM vm)
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