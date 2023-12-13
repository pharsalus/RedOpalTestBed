using RedOpalTestBed.Data;

namespace RedOpalTestBed;

public partial class UpdateStaffPage : ContentPage
{
    private PersonRepository repository;
    private Person staffToUpdate;

    public UpdateStaffPage(Person staff)
    {
        InitializeComponent();
        repository = new PersonRepository(); 
        staffToUpdate = staff;

        // Load staff details into input fields
        nameEntry.Text = staffToUpdate.Name;
        phoneEntry.Text = staffToUpdate.Phone;
        departmentIdEntry.Text = staffToUpdate.DepartmentId.ToString(); 
        streetEntry.Text = staffToUpdate.Street;
        cityEntry.Text = staffToUpdate.City;
        stateEntry.Text = staffToUpdate.State;
        zipEntry.Text = staffToUpdate.ZIP;
        countryEntry.Text = staffToUpdate.Country;
    }

    private async void UpdateStaff_Clicked(object sender, EventArgs e)
    {
        // Update staff ToUpdate object with new values from input fields
        staffToUpdate.Name = nameEntry.Text;
        staffToUpdate.Phone = staffToUpdate.Name;
        if (int.TryParse(departmentIdEntry.Text, out int departmentId))
        {
            staffToUpdate.DepartmentId = departmentId;
        }
        else
        {
            // Handle the case where departmentIdEntry.Text is not a valid integer
            // For example, you might want to alert the user or set a default value
        }
        staffToUpdate.Street = streetEntry.Text;
        staffToUpdate.City = cityEntry.Text;
        staffToUpdate.State = stateEntry.Text;
        staffToUpdate.ZIP = zipEntry.Text;
        staffToUpdate.Country = countryEntry.Text;

        await repository.UpdatePersonAsync(staffToUpdate); // Save changes

        // Confirmation message
        await DisplayAlert("Success", "Staff member updated", "OK");

        // Navigate back
        await Navigation.PopAsync();
    }
}
