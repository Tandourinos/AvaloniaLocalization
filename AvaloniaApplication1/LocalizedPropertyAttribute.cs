using System;

namespace AvaloniaApplication1;

[AttributeUsage(AttributeTargets.Field)]
public class LocalizedPropertyAttribute : Attribute
{
    private readonly string _interpolatedExpression;
    public string Expression => _interpolatedExpression;
    
    public LocalizedPropertyAttribute(string expression)
    {
        _interpolatedExpression = expression;
    }
}