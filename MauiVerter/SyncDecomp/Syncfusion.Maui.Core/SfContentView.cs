using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core;

[ContentProperty("Content")]
public abstract class SfContentView : SfView
{
	public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(SfContentView), null, BindingMode.OneWay, null, OnContentPropertyChanged);

	public View Content
	{
		get
		{
			return (View)GetValue(ContentProperty);
		}
		set
		{
			SetValue(ContentProperty, value);
		}
	}

	private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfContentView sfContentView)
		{
			sfContentView?.OnContentChanged(oldValue, newValue);
		}
	}

	protected virtual void OnContentChanged(object oldValue, object newValue)
	{
		if (oldValue != null && oldValue is View view)
		{
			Remove(view);
		}
		if (newValue != null && newValue is View view2)
		{
			Add(view2);
		}
	}
}
