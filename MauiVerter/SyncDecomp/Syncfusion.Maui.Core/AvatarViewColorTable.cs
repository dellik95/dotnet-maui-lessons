using System.Collections.Generic;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core;

internal static class AvatarViewColorTable
{
	internal static int CurrentBackgroundColorIndex { get; set; } = 0;


	internal static Color InitialsLightColor { get; set; } = Color.FromArgb("#FFFFFF");


	internal static Color InitialsDarkColor { get; set; } = Color.FromArgb("#000000");


	internal static List<AvatarViewAutomaticColor>? AutomaticColors { get; set; }

	internal static void GenerateAutomaticBackgroundColors()
	{
		AutomaticColors = new List<AvatarViewAutomaticColor>();
		AutomaticColors.Add(new AvatarViewAutomaticColor
		{
			LightColor = Color.FromArgb("#90DDFE"),
			DarkColor = Color.FromArgb("#976F0C")
		});
		AutomaticColors.Add(new AvatarViewAutomaticColor
		{
			LightColor = Color.FromArgb("#9FCC69"),
			DarkColor = Color.FromArgb("#740A1C")
		});
		AutomaticColors.Add(new AvatarViewAutomaticColor
		{
			LightColor = Color.FromArgb("#FCCE65"),
			DarkColor = Color.FromArgb("#5C2E91")
		});
		AutomaticColors.Add(new AvatarViewAutomaticColor
		{
			LightColor = Color.FromArgb("#FE9B90"),
			DarkColor = Color.FromArgb("#004E8C")
		});
		AutomaticColors.Add(new AvatarViewAutomaticColor
		{
			LightColor = Color.FromArgb("#9AA8F5"),
			DarkColor = Color.FromArgb("#B73EAA")
		});
		AutomaticColors.Add(new AvatarViewAutomaticColor
		{
			LightColor = Color.FromArgb("#F5EF9A"),
			DarkColor = Color.FromArgb("#69797E")
		});
		AutomaticColors.Add(new AvatarViewAutomaticColor
		{
			LightColor = Color.FromArgb("#FBBC93"),
			DarkColor = Color.FromArgb("#068387")
		});
		AutomaticColors.Add(new AvatarViewAutomaticColor
		{
			LightColor = Color.FromArgb("#D7E99C"),
			DarkColor = Color.FromArgb("#498204")
		});
		AutomaticColors.Add(new AvatarViewAutomaticColor
		{
			LightColor = Color.FromArgb("#E79AF5"),
			DarkColor = Color.FromArgb("#4F6BED")
		});
		AutomaticColors.Add(new AvatarViewAutomaticColor
		{
			LightColor = Color.FromArgb("#9FEFC5"),
			DarkColor = Color.FromArgb("#CA500F")
		});
	}
}
