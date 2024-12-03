using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace Syncfusion.Maui.Core.Localization;

public class LocalizationResourceAccessor
{
	private static CultureInfo culture = Thread.CurrentThread.CurrentUICulture;

	public static ResourceManager? ResourceManager { get; set; }

	internal static CultureInfo Culture
	{
		get
		{
			return culture;
		}
		set
		{
			culture = value;
		}
	}

	public static string? GetString(string text)
	{
		string result = string.Empty;
		try
		{
			if (ResourceManager != null)
			{
				result = ResourceManager.GetString(text, Culture);
			}
		}
		catch
		{
		}
		return result;
	}

	public static void InitializeDefaultResource(string baseName)
	{
		if (ResourceManager == null)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			ResourceManager = new ResourceManager(baseName, callingAssembly);
			Culture = new CultureInfo("en-US");
		}
	}
}
