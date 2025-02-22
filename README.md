# Avalonia Localization

##### _A simple real-time resource localization._

## Instructions

- View models with localizable content should derive from the `LocalizedViewModel` class
- Fields are marked as localizable using the `LocalizedProperty` attribute:

```c#
public partial class MainWindowViewModel : LocalizedViewModel
{
    [ObservableProperty]
    [LocalizedProperty(@"{Hi}. {MyNameIs} Ioannis. {HowAreYou}")]
    private string _title;
    ...
}
```

- The argument of the attribute defines the localizable expression using the [StringTokenFormatter](https://github.com/andywilsonuk/StringTokenFormatter).
- The field value is not set. So:
  - No two-way binding.
  - Expression cannot change during run-time. TODO...
- Use the `SwitchTo` method to switch between languages in real-time.

Is is production ready? _No_ Feel free to use and contribute. I am trying to find a simple way to do real-time localization.

***Free software? Yeah!!***
