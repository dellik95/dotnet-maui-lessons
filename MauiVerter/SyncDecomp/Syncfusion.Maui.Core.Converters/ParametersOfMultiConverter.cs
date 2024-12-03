using System;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Converters;

public class ParametersOfMultiConverter : BindableObject
{
	public Type? TypeOfConverter { get; set; }

	public object? ValueOfConverter { get; set; }
}
