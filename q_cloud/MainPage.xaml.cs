
using Microsoft.Data.Sqlite;

namespace q_cloud
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            _ = NavigateWithDelay();
        }

        private async Task NavigateWithDelay()
        {
            try
            {
                db_class.create_db();                
                List<List<String>> entries = db_class.get_data();

                
                if (entries.Count == 0)
                {
                    await Task.Delay(1000);
                    Console.WriteLine("Go to enter page");
                    await Shell.Current.GoToAsync("///EnterUserPage");
                }
                else
                {
                    await Task.Delay(1000);
                    Console.WriteLine("Go to home page");
                    await Shell.Current.GoToAsync("///HomePage");
                }      


            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }           
        }       
    }
}
