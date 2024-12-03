namespace Syncfusion.Maui.Themes;

internal interface IThemeElement
{
	void OnControlThemeChanged(string oldTheme, string newTheme);

	void OnCommonThemeChanged(string oldTheme, string newTheme);
}
