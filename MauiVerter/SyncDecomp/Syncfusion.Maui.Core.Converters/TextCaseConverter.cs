using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Converters;

public class TextCaseConverter : IValueConverter
{
	public CasingMode CasingMode { get; set; }

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null)
		{
			throw new ArgumentException("Value is null");
		}
		if (value is string inputstring)
		{
			return GetCase(inputstring, CasingMode);
		}
		throw new ArgumentException("Input value  is not of type string ", nameof(value));
	}

	private string GetCase(string Inputstring, CasingMode CasingMode)
	{
		switch (CasingMode)
		{
		case CasingMode.LowerCase:
			return Inputstring.ToLowerInvariant();
		case CasingMode.UpperCase:
			return Inputstring.ToUpperInvariant();
		case CasingMode.ParagraphCase:
			return Inputstring.Substring(0, 1).ToUpperInvariant() + Inputstring.ToString().Substring(1).ToLowerInvariant();
		case CasingMode.PascalCase:
		{
			string text = string.Join("", Inputstring.Select((char inputChar) => (char.IsWhiteSpace(inputChar) & char.IsLetterOrDigit(inputChar)) ? inputChar.ToString().ToLower() : inputChar.ToString()).ToArray());
			IEnumerable<string> values = from splitedString in text.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
				select splitedString.Substring(0, 1).ToUpper() + splitedString.Substring(1);
			return string.Join("", values);
		}
		default:
			return Inputstring;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException("ConvertBack to original value is not possible. BindingMode must be set as OneWay.");
	}
}
