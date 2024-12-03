using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Converters;

public class BoolToObjectConverter : IValueConverter
{
	public object? TrueValueObject { get; set; }

	public object? FalseValueObject { get; set; }

	public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
	{
		if (value != null)
		{
			if (value is bool flag)
			{
				return flag ? TrueValueObject : FalseValueObject;
			}
			throw new ArgumentException("Value is not a boolean type", nameof(value));
		}
		throw new ArgumentException("Value is null", nameof(value));
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null)
		{
			if (value.Equals(TrueValueObject))
			{
				return true;
			}
			if (value.Equals(FalseValueObject))
			{
				return false;
			}
			throw new ArgumentException("Value is not valid", nameof(value));
		}
		if (value == null && TrueValueObject == null)
		{
			return true;
		}
		throw new ArgumentException("Value is null", nameof(value));
	}
}
