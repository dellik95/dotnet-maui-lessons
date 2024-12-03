using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Converters;

public class MultiConverter : List<IValueConverter>, IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		Type targetType2 = targetType;
		CultureInfo culture2 = culture;
		object parameter2 = parameter;
		if (value == null)
		{
			throw new ArgumentNullException(nameof(value));
		}
		IList<ParametersOfMultiConverter> propertiesOfConverter = parameter2 as IList<ParametersOfMultiConverter>;
		if (propertiesOfConverter != null)
		{
			return this.Aggregate(value, (object currentValue, IValueConverter currentConverter) => currentConverter.Convert(currentValue, targetType2, propertiesOfConverter.FirstOrDefault((ParametersOfMultiConverter x) => x.TypeOfConverter == currentConverter.GetType())?.ValueOfConverter, culture2));
		}
		return this.Aggregate(value, (object currentValue, IValueConverter currentConverter) => currentConverter.Convert(currentValue, targetType2, parameter2, culture2));
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException("ConvertBack to original value is not possible. BindingMode must be set as OneWay.");
	}
}
