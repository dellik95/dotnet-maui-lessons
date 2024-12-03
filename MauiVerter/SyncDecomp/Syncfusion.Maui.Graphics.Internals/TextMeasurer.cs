using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;

namespace Syncfusion.Maui.Graphics.Internals;

internal class TextMeasurer : ITextMeasurer
{
	[ThreadStatic]
	private static ITextMeasurer? instance;

	private TextBlock? textBlock = null;

	public static ITextMeasurer Instance
	{
		get
		{
			instance ??= CreateTextMeasurer();
			return instance;
		}
	}

	internal static ITextMeasurer CreateTextMeasurer()
	{
		return new TextMeasurer();
	}

	public Microsoft.Maui.Graphics.Size MeasureText(string text, float textSize, FontAttributes attributes = FontAttributes.None, string? fontFamily = null)
	{
		if (textBlock == null)
		{
			textBlock = new TextBlock();
		}
		textBlock.Text = text;
		textBlock.FontSize = textSize;
		textBlock.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
		return new Microsoft.Maui.Graphics.Size((float)textBlock.DesiredSize.Width, (float)textBlock.DesiredSize.Height);
	}

	public Microsoft.Maui.Graphics.Size MeasureText(string text, ITextElement textElement)
	{
		IFontManager requiredService = MauiWinUIApplication.Current.Services.GetRequiredService<IFontManager>();
		if (textBlock == null)
		{
			textBlock = new TextBlock();
		}
		textBlock.Text = text;
		textBlock.FontSize = textElement.FontSize;
		Microsoft.Maui.Font font = textElement.Font;
		textBlock.FontFamily = requiredService.GetFontFamily(font);
		textBlock.FontStyle = font.ToFontStyle();
		textBlock.FontWeight = font.ToFontWeight();
		textBlock.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
		return new Microsoft.Maui.Graphics.Size((float)textBlock.DesiredSize.Width, (float)textBlock.DesiredSize.Height);
	}
}
