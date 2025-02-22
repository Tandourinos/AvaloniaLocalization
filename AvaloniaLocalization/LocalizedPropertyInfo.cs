using System.Reflection;

namespace AvaloniaLocalization;

public class LocalizedPropertyInfo
{
    public string FieldName { get; set; } = string.Empty;
    public string PropertyName { get; set; } = string.Empty;

    public FieldInfo FieldInfo { get; set; } = null!;
    public PropertyInfo PropertyInfo { get; set; } = null!;
    public bool ResetExpression { get; set; } = true;

    public string? Expression { get; set; } = null;
}