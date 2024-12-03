using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Converters;

public class DecimalValueConverter : IValueConverter
{
	public int NumberDecimalDigits { get; set; }

	public OutputType OutputType { get; set; }

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		string text = "0";
		if (value != null)
		{
			if (value.ToString() == "")
			{
				text = new string('0', NumberDecimalDigits);
				return "0." + text;
			}
			decimal num = decimal.Parse(value.ToString());
			switch (OutputType)
			{
			case OutputType.String:
			{
				if (num % 1m == 0m)
				{
					text = new string('0', NumberDecimalDigits);
					return $"{Math.Round(num)}.{text}";
				}
				string text2 = num.ToString();
				int length = text2.Substring(text2.IndexOf(".")).Length;
				if (length - 1 >= 1 && length - 1 <= NumberDecimalDigits)
				{
					text = new string('0', NumberDecimalDigits - (length - 1));
					return $"{num}{text}";
				}
				return Math.Round(num, NumberDecimalDigits).ToString();
			}
			default:
				return Math.Round(num, NumberDecimalDigits);
			}
		}
		throw new ArgumentException("Value cannot be null", nameof(value));
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException("ConvertBack to original value is not possible. BindingMode must be set as OneWay.");
	}
}
