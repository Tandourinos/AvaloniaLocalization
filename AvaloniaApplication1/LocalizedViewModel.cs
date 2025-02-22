using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using StringTokenFormatter;
using StringTokenFormatter.Impl;

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
        ChangeThreadCulture(cultureInfo);
        var localizableFields = GetAllLocalizableMembers();
        var attributeType = typeof(LocalizedPropertyAttribute);
        foreach (var field in localizableFields)
        {
            var property = GetCorrespondingProperty(field);
            if (property != null && property.CanWrite)
            {
                var attr = field.GetCustomAttribute(attributeType, false) as LocalizedPropertyAttribute;
                var expression = attr?.Expression;
                if (expression != null)
                {
                    var parsedExpression =
                        InterpolatedStringParser.Parse(expression, StringTokenFormatterSettings.Default);
                    var replacements = new List<KeyValuePair<string, string>>();
                    foreach (var token in parsedExpression.Tokens())
                    {
                        var replacement = _resourceManager.GetString(token);
                        if (replacement == null)
                        {
                            // Not sure what is best here. Exception or fallback to token itself...
                            replacement = token;
                        }

                        replacements.Add(new KeyValuePair<string, string>(token, replacement));
                    }

                    expression = expression.FormatFromPairs(replacements);
                }

                property.SetValue(this, expression);
            }
        }
    }
    
    private List<MemberInfo> GetAllLocalizableMembers()
    {
        var attributeType = typeof(LocalizedPropertyAttribute);
        return GetType()
            .GetMembers(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(m => Attribute.IsDefined(m, attributeType, false)).ToList();
    }

    //TODO: Needs work
    private PropertyInfo? GetCorrespondingProperty(MemberInfo field)
    {
        if (field.Name.StartsWith("_"))
        {
            var propName = Regex.Replace(field.Name, @"_(\w)", m => m.Groups[1].Value.ToUpper());
            return field.DeclaringType?.GetProperty(propName);
        }

        return null;
    }
    
    private void ChangeThreadCulture(CultureInfo cultureInfo)
    {
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
        Thread.CurrentThread.CurrentCulture = cultureInfo;
    }
}