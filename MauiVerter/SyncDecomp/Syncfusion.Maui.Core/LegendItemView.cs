using System;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Core.Internals;

namespace Syncfusion.Maui.Core;

internal class LegendItemView : ContentView, ITapGestureListener, IGestureListener
{
	private readonly Action<LegendItem> legendAction;

	internal static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(LegendItemView), null, BindingMode.Default, null, OnItemTemplateChanged);

	internal DataTemplate ItemTemplate
	{
		get
		{
			return (DataTemplate)GetValue(ItemTemplateProperty);
		}
		set
		{
			SetValue(ItemTemplateProperty, value);
		}
	}

	public LegendItemView(Action<LegendItem> action)
	{
		legendAction = action;
		this.AddGestureListener(this);
	}

	private static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is LegendItemView legendItemView)
		{
			legendItemView.OnItemTemplateChanged(oldValue, newValue);
		}
	}

	private void OnItemTemplateChanged(object oldValue, object newValue)
	{
		if (newValue != null && newValue is DataTemplate dataTemplate)
		{
			base.Content = (View)dataTemplate.CreateContent();
		}
	}

	public void OnTap(TapEventArgs e)
	{
		if (legendAction != null && base.BindingContext is LegendItem obj)
		{
			legendAction(obj);
		}
	}
}
