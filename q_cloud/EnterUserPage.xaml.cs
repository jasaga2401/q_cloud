namespace q_cloud;

public partial class EnterUserPage : ContentPage
{
	public EnterUserPage()
	{
		InitializeComponent();
	}

    public async void OnRegisterButtonClicked(object sender, EventArgs e)
	{
		try 
		{
            string forename = ForenameEntry.Text;
            string surname = SurnameEntry.Text;
            DateOnly dob = DateOnly.FromDateTime(myDateOfBirth.Date);
            DateOnly startdob = DateOnly.FromDateTime(myStartDate.Date);
            DateOnly quitdob = DateOnly.FromDateTime(myQuitDate.Date);
            int averageSmokeDay = Int32.Parse(avgNumSmokedVal.Text);

            // parsing so that the correct value is being entered by removing the £ sign
            string costPacketCigarettes = cstPacketCigarettes.Text;
            costPacketCigarettes = costPacketCigarettes.Replace("£", "");
            double cstPckCig = double.Parse(costPacketCigarettes);

            await DisplayAlert("Notice", forename + " " + surname + " " + dob + " " + startdob + " " + quitdob + " " + averageSmokeDay + " " + cstPckCig, "OK");

            db_class.insert_data(forename, surname, dob, startdob, quitdob, averageSmokeDay, cstPckCig);

            // navigate to the home page
            await Shell.Current.GoToAsync("///HomePage");

        }         
		
		catch (Exception ex)

		{
            await DisplayAlert("Error", ex.Message, "OK");
		}
	}
    private void OnSliderValueAvgNumSmokedVal(object sender, ValueChangedEventArgs e)
    {
        Console.WriteLine("***************** Entered slider here *******************************");
        // Example of additional handling
        avgNumSmokedVal.Text = $"{e.NewValue:F0}";
    }

    private void OnSliderValueCstPacketCigarettes(object sender, ValueChangedEventArgs e)
    {
        Console.WriteLine("***************** Entered slider here *******************************");
        // Example of additional handling
        cstPacketCigarettes.Text = $"£{e.NewValue:F2}";
    }
}