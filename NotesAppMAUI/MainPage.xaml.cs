using NotesAppAPI;
using NotesAppMAUI.View;
using NotesAppMAUI.ViewModel.VMO;

namespace NotesAppMAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            INotesAPI api = new NotesAPI();

            if (PasswordEry.Text == null || UsernameEry.Text == null)
            {
                CounterBtn.Text = "Invalid Login";
                return;
            }

            var user = api.Login(UsernameEry.Text, PasswordEry.Text);


            if (user != null)
            {
                UserVMO userVmo = Converters.Convert(user);

                (App.Current.MainPage as AppShell).CurrentUser = userVmo;

                PasswordEry.Text = null;
                CounterBtn.Text = "Login";

                await AppShell.Current.GoToAsync($"//Subjects?", new Dictionary<string, object>
                {
                    ["User"] = userVmo,
                });
            }
            else
            {
                CounterBtn.Text = "Invalid Login";
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            AppShell.Current.GoToAsync($"NewUser");
        }
    }
}