# Avalonia Localization

##### _A simple real-time resource localization._
Just 3 source files in the library and a dependency on the [StringTokenFormatter](https://github.com/andywilsonuk/StringTokenFormatter) nuget.

## Instructions

- View models with localizable content should derive from the `LocalizedViewModel` class
- Fields are marked as localizable using the `LocalizableProperty` attribute:

```c#
public partial class MainWindowViewModel : LocalizedViewModel
{
    [ObservableProperty]
    [LocalizableProperty]
    private string _title = @"{Hi}. {MyNameIs} Ioannis. {HowAreYou}";;
    ...
}
```

- The field value defines the localizable expression using the [StringTokenFormatter](https://github.com/andywilsonuk/StringTokenFormatter).
- Use the `SwitchTo` method to switch between languages in real-time.
- Only strings at the moment.
- All localizable field names should start with _ and a lowercase character for auto-discovery.
- Expressions are parsed and tokens are replaced by the corresponding localised values defined in the resources in real-time. 

Is is production ready? _No_ Feel free to use and contribute. I am trying to find a simple way to do real-time localization.

***Free software? Yeah!!***
