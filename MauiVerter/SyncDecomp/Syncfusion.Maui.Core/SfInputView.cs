using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace Syncfusion.Maui.Core;

internal class SfInputView : Entry
{
	private IDrawable? drawable;

	private bool isDeleteButtonPressed = false;

	private Microsoft.Maui.Graphics.Color? focusedColor = Colors.Gray;

	internal bool IsDeletedButtonPressed
	{
		get
		{
			return isDeleteButtonPressed;
		}
		set
		{
			isDeleteButtonPressed = value;
		}
	}

	internal Microsoft.Maui.Graphics.Color? FocusedStroke
	{
		get
		{
			return focusedColor;
		}
		set
		{
			focusedColor = value;
		}
	}

	internal IDrawable? Drawable
	{
		get
		{
			return drawable;
		}
		set
		{
			drawable = value;
		}
	}

	internal double ButtonSize { get; set; }

	public SfInputView()
	{
		Iniitalize();
	}

	private void Iniitalize()
	{
		base.Style = new Microsoft.Maui.Controls.Style(typeof(SfInputView));
		base.BackgroundColor = Colors.Transparent;
		base.TextColor = Colors.Black;
		base.FontSize = 14.0;
	}

	internal void SelectText(int start)
	{
		if (base.Handler != null && base.Handler.PlatformView is MauiPasswordTextBox mauiPasswordTextBox)
		{
			mauiPasswordTextBox.Select(start, mauiPasswordTextBox.Text.Length - start);
		}
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		if (base.Handler != null && base.Handler.PlatformView is TextBox textBox)
		{
			textBox.BorderThickness = new Thickness(0.0);
			textBox.Resources["TextControlBorderThemeThicknessFocused"] = textBox.BorderThickness;
			object obj = textBox.Resources["TextControlBorderBrushFocused"];
			if (obj is Microsoft.UI.Xaml.Media.GradientBrush gradientBrush && gradientBrush != null && gradientBrush.GradientStops != null)
			{
				Windows.UI.Color? color = gradientBrush.GradientStops.FirstOrDefault()?.Color;
				if (color.HasValue)
				{
					FocusedStroke = new Microsoft.Maui.Graphics.Color(color.Value.R, color.Value.G, color.Value.B, color.Value.A);
				}
			}
		}
		base.Focused -= SfEntry_Focused;
		base.Unfocused -= SfEntry_Unfocused;
		if (base.Handler != null)
		{
			base.Focused += SfEntry_Focused;
			base.Unfocused += SfEntry_Unfocused;
		}
	}

	private void SfEntry_Unfocused(object? sender, FocusEventArgs e)
	{
		if (drawable is SfView sfView)
		{
			sfView.InvalidateDrawable();
		}
	}

	private void SfEntry_Focused(object? sender, FocusEventArgs e)
	{
		if (drawable is SfView sfView)
		{
			sfView.InvalidateDrawable();
		}
		if (!string.IsNullOrEmpty(base.Text) && !base.IsReadOnly)
		{
			SelectText(0);
		}
	}
}
