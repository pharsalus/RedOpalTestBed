#nullable enable
using SQLite;

namespace RedOpalTestBed;

/// <summary>
/// We could also save these settings to CSV or some Webservice but SQLite is fast, easy and feature rich
/// If we want to use defaults we can simply use MAUI preferences
/// OR a combo of both!
/// LINK: https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/preferences?tabs=windows
/// </summary>
public partial class Settings : ContentPage
{
    //Data that you don't want exposed.
    private SQLiteAsyncConnection _database;
    public SettingsViewModel ViewModel { get; set; }

    public Settings()
    {
        InitializeComponent();
        ViewModel = new SettingsViewModel();
        BindingContext = ViewModel;

        //Create the database in the service OR have some constructor so don't have to do it here.
        // Initialize the SQLite database connection
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "settings.db");
        _database = new SQLiteAsyncConnection(databasePath);
        _database.CreateTableAsync<UserSet>().Wait();
        //_database.DeleteAllAsync<UserSet>().Wait();

        fontSizeSlider.Value = ViewModel.FontSize; //-> This would be automatic binding 

        // Load the user settings
        LoadUserSettings();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Reload the user settings to refresh the page's state
        LoadUserSettings();
    }

    private async void SaveSettings_Clicked(object sender, EventArgs e)
    {
        var name = NameEntry.Text; //This would be replaced by saving some login information OR pulling a user account etc.
        int age = 0; //Set age off an entry etc.
        int.TryParse(AgeEntry.Text, out age); // Calculated by the app or taken from account information
        bool theme = togTheme.IsToggled;
        //var someEnt = someEntry.Text;
        //Adding Font Weight
        
        int fontSize = (int)fontSizeSlider.Value; 
        float brightness = (float)brightnessSlider.Value;
        string selectedFont = fontFamilyPicker.SelectedItem?.ToString() ?? "DefaultFont";
        string selectedFontWeight = fontWeightPicker.SelectedItem?.ToString() ?? "DefaultFontWeight";

        var userSettings = new UserSet
        {
            Name = name,
            Age = age,
            lightOrDark = theme,
            //SomeEntry = someEnt,
            SavedFontSize = ViewModel.FontSize,
            SavedBrightness = ViewModel.Brightness,
            SavedFontFamily = ViewModel.SelectedFontFamily,
            SavedFontWeight = ViewModel.SelectedFontWeight,
        };

        
        await _database.InsertOrReplaceAsync(userSettings);

        // Show a confirmation message
        await DisplayAlert("Success", "User settings saved", "OK");
    }

    //PART of the database services
    //Onloading OR onAppearing for settings adjustments.
    private async void LoadUserSettings()
    {
        // Check if the user settings already exist in the database
        var existingSettings = await _database.Table<UserSet>().FirstOrDefaultAsync();
        if (existingSettings != null)
        {
            NameEntry.Text = existingSettings.Name;
            AgeEntry.Text = existingSettings.Age.ToString();

            //someEntry.Text = existingSettings.SomeEntry?.ToString() ?? "Default Value";

            ViewModel.FontSize = (int)existingSettings.SavedFontSize;
            ViewModel.Brightness = (float)existingSettings.SavedBrightness;
            fontFamilyPicker.SelectedItem = existingSettings.SavedFontFamily;
            //Adding font weight picker
            fontWeightPicker.SelectedItem = existingSettings.SavedFontWeight;

            if (existingSettings.lightOrDark)
            {
                togTheme.IsToggled = true;
            }
            else
            {
                togTheme.IsToggled = false;
            }

            var currentTheme = existingSettings.lightOrDark;
            if (currentTheme)
                Application.Current!.UserAppTheme = AppTheme.Dark;
            else
                Application.Current!.UserAppTheme = AppTheme.Light;
        }
    }

    //PREFERENCES
    //SWITCH: https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/switch
    private void OnThemeSwitchToggled(object sender, ToggledEventArgs e)
    {
        bool isDarkTheme = e.Value;
        Preferences.Set("DarkThemeOn", isDarkTheme ? "Dark" : "Light");

        // Apply the theme
        if (isDarkTheme)
            Application.Current!.UserAppTheme = AppTheme.Dark;
        else
            Application.Current!.UserAppTheme = AppTheme.Light;
    }
}