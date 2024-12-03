using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Themes;

internal interface IParentThemeElement : IThemeElement
{
	ResourceDictionary GetThemeDictionary();
}
