using TripPlanApp.ViewModels;

namespace TripPlanApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        public AppShell(ShellViewModel vm)
        {
            this.BindingContext = vm;
            InitializeComponent();
        }
    }
}
