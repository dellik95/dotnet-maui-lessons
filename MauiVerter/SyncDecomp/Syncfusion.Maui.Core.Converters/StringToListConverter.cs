using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Converters;

public class StringToListConverter : IValueConverter
{
	public string Separator { get; set; } = string.Empty;


	public IList<string> Separators { get; } = new List<string>();


	public StringSplitOptions SplitOptions { get; set; } = StringSplitOptions.None;


	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null)
		{
			if (value is string text)
			{
				if (Separators.Count == 0)
				{
					string[] source = text.Split(Separator, SplitOptions);
					return source.ToList();
				}
				if (Separators.Count >= 1)
				{
					string[] source2 = text.Split(Separators.ToArray(), SplitOptions);
					return source2.ToList();
				}
				return string.Empty;
			}
			throw new ArgumentException($"Value is of {new Func<Type>(value.GetType)}");
		}
		return string.Empty;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException("ConvertBack to original value is not possible. BindingMode must be set as OneWay.");
	}
}
