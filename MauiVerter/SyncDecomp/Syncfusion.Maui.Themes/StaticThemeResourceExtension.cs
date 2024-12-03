using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace Syncfusion.Maui.Themes;

[ContentProperty("ResourceKey")]
public class StaticThemeResourceExtension : IMarkupExtension
{
	public string? ResourceKey { get; set; }

	public object? ProvideValue(IServiceProvider serviceProvider)
	{
		if (ResourceKey == null || serviceProvider == null)
		{
			return null;
		}
		if (serviceProvider.GetService(typeof(IRootObjectProvider)) is IRootObjectProvider rootObjectProvider && rootObjectProvider.RootObject is ResourceDictionary resourceDictionary)
		{
			try
			{
				return resourceDictionary[ResourceKey];
			}
			catch
			{
				string message = "The resource '" + ResourceKey + "' is not present in the dictionary.";
				throw new Exception(message);
			}
		}
		return null;
	}
}
