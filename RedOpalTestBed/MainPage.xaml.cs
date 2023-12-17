using RedOpalTestBed.Data; 

namespace RedOpalTestBed
{
    public partial class MainPage : ContentPage
    {
        // Use PersonRepository
        private PersonRepository repository; 
        private List<Person> people = new List<Person>(); 

        public MainPage()
        {
            InitializeComponent();

            // Initialize PersonRepository
            repository = new PersonRepository(); 
            LoadPeople(); 
        }

        private void LoadPeople()
        {
            people = repository.GetAllPeople(); // Retrieve people data
            ListOfPeople.ItemsSource = people; // Bind to ListOfPeople CollectionView
        }

        private void AddStaff_Clicked(object sender, EventArgs e)
        {
            
            Navigation.PushAsync(new AddStaffPage());
        }
        //OnAppearing Refreshes the page when the user navigates back to it
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadPeople(); 
        }
        private async void UpdateStaff_Clicked(object sender, EventArgs e)
        {
            if (ListOfPeople.SelectedItem != null)
            {
                Person selectedPerson = (Person)ListOfPeople.SelectedItem;

                // Navigate to the UpdateStaffPage, passing the selectedPerson as a parameter
                await Navigation.PushAsync(new UpdateStaffPage(selectedPerson));
            }
            else
            {
                await DisplayAlert("Alert", "Please select a staff member to update", "OK");
            }
        }

        private void DeleteStaff_Clicked(object sender, EventArgs e)
        {
            if (ListOfPeople.SelectedItem != null)
            {
                // Delete the selected staff member
                Person selectedPerson = (Person)ListOfPeople.SelectedItem;
                repository.DeletePerson(selectedPerson.Id); // Use DeletePerson method
                LoadPeople();
            }
        }
        private Person? selectedPerson;

        private void OnPersonSelected(object sender, SelectionChangedEventArgs e)
        {
            selectedPerson = e.CurrentSelection.FirstOrDefault() as Person;
        }

        private async void FullStaffProfile_Clicked(object sender, EventArgs e)
        {
            if (selectedPerson != null)
            {
                var profilePage = new StaffProfilePage(selectedPerson);
                await Navigation.PushAsync(profilePage);
            }
            else
            {
                await DisplayAlert("Alert", "Please select a staff member first", "OK");
            }
        }
        private void PlayPage_Clicked(object sender, EventArgs e)
        {

            Navigation.PushAsync(new FukAroundPage());
        }
        //private void Settings_Clicked(object sender, EventArgs e)
        //{

        //    Navigation.PushAsync(new Settings());
        //}

    }
}

