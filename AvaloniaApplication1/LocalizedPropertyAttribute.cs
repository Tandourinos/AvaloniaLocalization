using System;

namespace AvaloniaApplication1;

[AttributeUsage(AttributeTargets.Field)]
public class LocalizedPropertyAttribute : Attribute
{
    private readonly string[] _tokens;
    private readonly string _expression;
    public string[] Tokens => _tokens;
    public string Expression => _expression;
    
    public LocalizedPropertyAttribute(string expression, params string[] tokens)
    {
        _expression = expression;
        _tokens = tokens;
    }
}