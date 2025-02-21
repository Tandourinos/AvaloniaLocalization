using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaApplication1;

public abstract class LocalizedViewModel : ObservableObject
{
    private readonly ResourceManager _resourceManager;

    protected LocalizedViewModel()
    {
        _resourceManager = Resources.ResourceManager;
        SwitchTo(Thread.CurrentThread.CurrentUICulture);
    }

    protected void SwitchTo(CultureInfo cultureInfo)
    {
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        
        var attributeType = typeof(LocalizedPropertyAttribute);

        var localizableFields = GetType()
            .GetMembers(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(m => Attribute.IsDefined(m, attributeType, false)).ToList();

        foreach (var field in localizableFields)
        {
            // Get corresponding property
            if (field.Name.StartsWith("_"))
            {
                var propName = Regex.Replace(field.Name, @"_(\w)", m => m.Groups[1].Value.ToUpper());
                var prop = field.DeclaringType.GetProperty(propName);
                if (prop != null && prop.CanWrite)
                {
                    var attr = field.GetCustomAttribute(attributeType, false) as LocalizedPropertyAttribute;
                    var expression = attr.Expression;
                    var tokens = attr.Tokens;
                    
                    //string pattern = @"(?<!{)\{(\d+)\}(?!})";
                    var pattern = @"\{(\d+)\}";
                    expression = Regex.Replace(expression, pattern, (m) =>
                    {
                        var key = m.Groups[1].Value;
                        if (int.TryParse(key, out int index) && index >= 0 && index < tokens.Length)
                        {
                            return _resourceManager.GetString(tokens[index]) ?? m.Groups[0].Value;
                        }
                        return m.Groups[0].Value;
                    });
                    expression = expression.Replace("\\{", "{");
                    expression = expression.Replace("\\}", "}");
                    
                    prop.SetValue(this, expression);
                }
            }
        }
    }
}