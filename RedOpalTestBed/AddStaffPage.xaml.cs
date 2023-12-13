using RedOpalTestBed.Data;

namespace RedOpalTestBed;

public partial class AddStaffPage : ContentPage
{
    private PersonRepository repository;

    public AddStaffPage()
    {
        InitializeComponent();
        repository = new PersonRepository(); // Or inject it if using dependency injection
    }

    private async void AddStaff_Clicked(object sender, EventArgs e)
    {
        var newStaff = new Person
        {
            Name = nameEntry.Text,
            Phone = phoneEntry.Text,
            DepartmentId = int.Parse(departmentIdEntry.Text), // Assuming DepartmentID is an integer
            Street = streetEntry.Text,
            City = cityEntry.Text,
            State = stateEntry.Text,
            ZIP = zipEntry.Text,
            Country = countryEntry.Text
            // ID is auto-generated in the database
        };

        // Validation logic should be added here
        // For example, check if the entries are not empty, etc.

        await repository.AddPersonAsync(newStaff);
        


        // Optionally navigate back or display a confirmation
        DisplayAlert("Success", "New staff member added", "OK");

        Navigation.PopAsync(); // Navigate back to the previous page
    }
}
