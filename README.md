# Avalonia Localization
##### _A simple real-time resource localization._

## Instructions

- View models with localizable content should derive from the `LocalizedViewModel` class
- Fields are marked as localizable using the `LocalizedProperty` attribute:
```c#
public partial class MainWindowViewModel : LocalizedViewModel
{
    [ObservableProperty]
    [LocalizedProperty(@"Hi. {0} Ioannis. {1}", "MyNameIs", "HowAreYou")]
    private string _title;
    ...
}
```
- The first argument of the attribute defines the localizable expression.
- Subsequent arguments are Resource key names to be replaced by localized resources
- The field value is not set. So, two-way binding does not work.
- Use the `SwitchTo` method to switch between languages.

Is is production ready? _No_ Feel free to use and contribute. I am trying to find a simple way to do real-time localization.

***Free software? Yeah!!***

