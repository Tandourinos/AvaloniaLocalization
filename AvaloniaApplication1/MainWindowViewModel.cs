using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaApplication1;

public partial class MainWindowViewModel : LocalizedViewModel
{
    [ObservableProperty]
    [LocalizedProperty(@"Hi. {0} Ioannis. {1}", "MyNameIs", "HowAreYou")]
    private string _title;

    [ObservableProperty]
    [LocalizedProperty("{0}", "Culture")]
    private string _currentCulture;

    [RelayCommand]
    private void Switch()
    {
        SwitchTo(CultureInfo.CurrentUICulture.Equals(CultureInfo.GetCultureInfo("el-GR"))
            ? CultureInfo.GetCultureInfo("en-US")
            : CultureInfo.GetCultureInfo("el-GR"));
    }
}