using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaApplication1;

public partial class MainWindowViewModel() : LocalizedViewModel(Resources.ResourceManager)
{
    [ObservableProperty] [LocalizableProperty]
    private string _currentCulture = @"{Culture}";

    [ObservableProperty] [LocalizableProperty]
    private string _title = @"{Hi}. {MyNameIs} Ioannis. {HowAreYou}";

    [RelayCommand]
    private void Switch()
    {
        SwitchTo(CultureInfo.CurrentUICulture.Equals(CultureInfo.GetCultureInfo("el-GR"))
            ? CultureInfo.GetCultureInfo("en-US")
            : CultureInfo.GetCultureInfo("el-GR"));
    }
}