using RedOpalTestBed.Data; // Update the namespace to match your project

namespace RedOpalTestBed
{
    public partial class MainPage : ContentPage
    {
        private PersonRepository repository; // Use PersonRepository
        private List<Person> people = new List<Person>(); // Use a list of Person objects

        public MainPage()
        {
            InitializeComponent();

            repository = new PersonRepository(); // Initialize PersonRepository
            LoadPeople(); // Load staff data
        }

        private void LoadPeople()
        {
            people = repository.GetAllPeople(); // Retrieve people data
            ListOfPeople.ItemsSource = people; // Bind to ListOfPeople CollectionView
        }

        private void AddStaff_Clicked(object sender, EventArgs e)
        {
            // Logic to add a new staff member
            // Update to match the data entry method in your UI
            Person newPerson = new Person
            {
                // Assign properties based on your UI and Person class
            };

            repository.AddPerson(newPerson); // Use AddPerson method
            LoadPeople();
        }

        private void UpdateStaff_Clicked(object sender, EventArgs e)
        {
            if (ListOfPeople.SelectedItem != null)
            {
                // Update selected staff member information
                Person selectedPerson = (Person)ListOfPeople.SelectedItem;
                // Update properties based on your UI and Person class

                repository.UpdatePerson(selectedPerson); // Use UpdatePerson method
                LoadPeople();
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

    }
}

