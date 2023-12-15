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


        // Initialize the SQLite database connection
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "settings.db");
        _database = new SQLiteAsyncConnection(databasePath);
        _database.CreateTableAsync<UserSet>().Wait();
        //_database.DeleteAllAsync<UserSet>().Wait();

        // Load the user settings
        LoadUserSettings();
    }

    private async void SaveSettings_Clicked(object sender, EventArgs e)
    {
        var name = NameEntry.Text;
        int age = 0;
        int.TryParse(AgeEntry.Text, out age);
        bool theme = togTheme.IsToggled;
        var someEnt = someEntry.Text;

        int fontSize = (int)fontSizeSlider.Value;
        float brightness = (float)brightnessSlider.Value;
        string selectedFont = fontFamilyPicker.SelectedItem.ToString();

        var userSettings = new UserSet
        {
            Name = name,
            Age = age,
            lightOrDark = theme,
            SomeEntry = someEnt,
            SavedFontSize = fontSize,
            SavedBrightness = brightness,
            SavedFontFamily = selectedFont,
        };

        await _database.InsertOrReplaceAsync(userSettings);

        // Show a confirmation message
        await DisplayAlert("Success", "User settings saved", "OK");
    }

    private async void LoadUserSettings() 
    {
        // Check if the user settings already exist in the database
        var existingSettings = await _database.Table<UserSet>().FirstOrDefaultAsync();
        if (existingSettings != null)
        {
            NameEntry.Text = existingSettings.Name;
            AgeEntry.Text = existingSettings.Age.ToString();

            someEntry.Text = existingSettings.SomeEntry.ToString();

            fontSizeSlider.Value = (double)existingSettings.SavedFontSize;
            brightnessSlider.Value = (double)existingSettings.SavedBrightness;
            fontFamilyPicker.SelectedItem = existingSettings.SavedFontFamily;


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
                Application.Current.UserAppTheme = AppTheme.Dark;
            else
                Application.Current.UserAppTheme = AppTheme.Light;
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
            Application.Current.UserAppTheme = AppTheme.Dark;
        else
            Application.Current.UserAppTheme = AppTheme.Light;
    }
}