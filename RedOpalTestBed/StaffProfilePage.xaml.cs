using RedOpalTestBed.Data; // Update the namespace to match your project

namespace RedOpalTestBed;

public partial class StaffProfilePage : ContentPage
{
    private Person selectedPerson;

    public StaffProfilePage(Person person)
    {
        InitializeComponent();
        selectedPerson = person;

        // Assuming the Person object has these properties
        personPhoto.Source = "placeholder_grey.png"; // Update logic if you have different images for each person
        personName.Text = selectedPerson.Name;
        personId.Text = $"ID: {selectedPerson.Id}";
        personPhone.Text = $"Phone: {selectedPerson.Phone}";
        personDepartmentId.Text = $"Department ID: {selectedPerson.DepartmentId}";
        personStreetCity.Text = $"Address: {selectedPerson.Street}, {selectedPerson.City}";
        personStateZipCountry.Text = $"{selectedPerson.State}, {selectedPerson.ZIP}, {selectedPerson.Country}";
        // Set other fields as necessary
    }
}