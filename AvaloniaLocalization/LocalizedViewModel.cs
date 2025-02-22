using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using StringTokenFormatter;
using StringTokenFormatter.Impl;

namespace AvaloniaApplication1;

public abstract class LocalizedViewModel : ObservableObject
{
    private readonly ConcurrentDictionary<string, LocalizedPropertyInfo> _localizedProperties;
    private readonly ResourceManager _resourceManager;

    protected LocalizedViewModel(ResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
        _localizedProperties = new ConcurrentDictionary<string, LocalizedPropertyInfo>(ExtractLocalizedProperties());
        PropertyChanged += OnPropertyChanged;
        SwitchTo(Thread.CurrentThread.CurrentUICulture);
    }

    private IEnumerable<KeyValuePair<string, LocalizedPropertyInfo>> ExtractLocalizedProperties()
    {
        var localizedProperties = new Dictionary<string, LocalizedPropertyInfo>();
        var localizableFields = GetAllLocalizableMembers();
        foreach (var memberInfo in localizableFields)
        {
            var field = memberInfo as FieldInfo;
            if (field == null) continue; // TODO: Only works for fields at the moment.
            var property = GetCorrespondingProperty(field);
            if (property == null || !property.CanWrite) continue;
            localizedProperties.Add(field.Name, new LocalizedPropertyInfo
            {
                FieldName = field.Name,
                PropertyName = property.Name,
                FieldInfo = field,
                PropertyInfo = property,
                Expression = field.GetValue(this)?.ToString(),
                ResetExpression = true
            });
        }

        return localizedProperties;
    }


    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        var field = GetCorrespondingField(e.PropertyName);
        if (field == null) return;
        var localizable = _localizedProperties.GetValueOrDefault(field.Name);
        if (localizable?.ResetExpression ?? false)
        {
            localizable.Expression = localizable.PropertyInfo.GetValue(this)?.ToString();
            if (localizable.Expression != null)
            {
                var localizedExpression = LocalizeExpression(localizable.Expression);
                if (localizedExpression != null) field.SetValue(this, localizedExpression);
            }
        }
    }

    protected void SwitchTo(CultureInfo cultureInfo)
    {
        ChangeThreadCulture(cultureInfo);
        foreach (var localizable in _localizedProperties.Values)
        {
            if (string.IsNullOrEmpty(localizable.Expression))
            {
                var fieldValue = localizable.FieldInfo.GetValue(this);
                localizable.Expression = fieldValue?.ToString();
            }

            if (localizable.Expression == null) continue;
            var localizedString = LocalizeExpression(localizable.Expression);
            if (localizedString == null) continue;
            localizable.ResetExpression = false;
            localizable.PropertyInfo.SetValue(this, localizedString);
            localizable.ResetExpression = true;
        }
    }

    private object? LocalizeExpression(string localizableExpression)
    {
        var parsedExpression =
            InterpolatedStringParser.Parse(localizableExpression, StringTokenFormatterSettings.Default);
        if (parsedExpression.Tokens().Count == 0) return null;

        var replacements = new List<KeyValuePair<string, string>>();
        foreach (var token in parsedExpression.Tokens())
        {
            // Not sure what is best here. Exception or fallback to token itself...
            var replacement = _resourceManager.GetString(token) ?? token;

            replacements.Add(new KeyValuePair<string, string>(token, replacement));
        }

        return localizableExpression.FormatFromPairs(replacements);
    }

    private List<MemberInfo> GetAllLocalizableMembers()
    {
        var attributeType = typeof(LocalizablePropertyAttribute);
        return GetType()
            .GetMembers(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(m => Attribute.IsDefined(m, attributeType, false)).ToList();
    }

    private FieldInfo? GetCorrespondingField(string? propertyName)
    {
        if (propertyName == null || propertyName.Length < 2) return null;
        var correspondingFieldName = $"_{propertyName[0].ToString().ToLowerInvariant()}{propertyName.Substring(1)}";
        return GetType().GetField(correspondingFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
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