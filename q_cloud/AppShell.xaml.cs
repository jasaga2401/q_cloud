namespace q_cloud
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            this.Navigated += HandleNavigated;
        }

        private void HandleNavigated(object sender, ShellNavigatedEventArgs e)
        {
            // Reset navigation state
            if (e.Source == ShellNavigationSource.ShellSectionChanged)
            {
                Shell.Current.FlyoutIsPresented = false;
                Shell.Current.Navigation.PopToRootAsync(false);
            }
        }
    }
}
