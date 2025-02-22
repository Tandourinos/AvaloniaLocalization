using System.Reflection;

namespace AvaloniaApplication1;

public class LocalizedPropertyInfo
{
    public string FieldName { get; set; }
    public string PropertyName { get; set; }

    public FieldInfo FieldInfo { get; set; }
    public PropertyInfo PropertyInfo { get; set; }
    public bool ResetExpression { get; set; } = true;

    public string? Expression { get; set; } = null;
}