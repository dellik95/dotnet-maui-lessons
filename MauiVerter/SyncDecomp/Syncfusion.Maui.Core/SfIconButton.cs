using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core.Internals;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Core;

internal class SfIconButton : Grid, ITouchListener
{
	internal Action<string>? Clicked;

	private bool showTouchEffect;

	private bool visibility;

	internal bool ShowTouchEffect
	{
		get
		{
			return showTouchEffect;
		}
		set
		{
			if (value != showTouchEffect)
			{
				showTouchEffect = value;
			}
		}
	}

	internal SfEffectsView EffectsView { get; set; }

	internal bool Visibility
	{
		get
		{
			return visibility;
		}
		set
		{
			base.IsVisible = value;
			visibility = value;
		}
	}

	internal SfIconButton(View child, bool showTouchEffect = true)
	{
		this.showTouchEffect = showTouchEffect;
		EffectsView = new SfEffectsView();
		Add(EffectsView);
		EffectsView.Content = child;
		EffectsView.ShouldIgnoreTouches = true;
		this.AddTouchListener(this);
		visibility = true;
	}

	void ITouchListener.OnTouch(PointerEventArgs e)
	{
		if (!showTouchEffect)
		{
			return;
		}
		if (e.Action == PointerActions.Pressed)
		{
			EffectsView.ApplyEffects();
		}
		else if (e.Action == PointerActions.Released)
		{
			EffectsView.Reset();
			if (EffectsView.Content is SfIconView sfIconView)
			{
				Clicked?.Invoke(sfIconView.Text);
			}
			if (showTouchEffect && base.IsEnabled)
			{
				EffectsView.Background = new SolidColorBrush(Colors.Black.WithAlpha(0.04f));
			}
			else
			{
				EffectsView.Background = Brush.Transparent;
			}
		}
		else if (e.Action == PointerActions.Entered)
		{
			EffectsView.ApplyEffects(SfEffects.Highlight);
		}
		else if (e.Action == PointerActions.Exited)
		{
			EffectsView.Reset();
			EffectsView.Background = Brush.Transparent;
		}
		else if (e.Action == PointerActions.Cancelled)
		{
			EffectsView.Reset();
			EffectsView.Background = Brush.Transparent;
		}
	}

	internal void UpdateStyle(ITextElement textStyle)
	{
		if (EffectsView.Content is SfIconView sfIconView)
		{
			sfIconView.UpdateStyle(textStyle);
		}
	}

	internal void UpdateIconColor(Color iconColor)
	{
		if (EffectsView.Content is SfIconView sfIconView)
		{
			sfIconView.UpdateIconColor(iconColor);
		}
	}
}
