using System.ComponentModel;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace Syncfusion.Maui.Core.Internals;

internal class ContentPlaceHolder : AbsoluteLayout
{
	private PanZoomListener m_panZoomListener;

	internal ContentPlaceHolder(PanZoomListener panZoomListener)
	{
		m_panZoomListener = panZoomListener;
		base.ChildAdded += OnChildAdded;
		base.ChildRemoved += OnChildRemoved;
	}

	private void OnChildRemoved(object? sender, ElementEventArgs e)
	{
		e.Element.PropertyChanged -= OnChildPropertyChanged;
	}

	private void OnChildPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		string propertyName = e.PropertyName;
		string text = propertyName;
		if (text == "HorizontalOptions" || text == "VerticalOptions")
		{
			LayoutContent();
		}
	}

	private void OnChildAdded(object? sender, ElementEventArgs e)
	{
		e.Element.PropertyChanged += OnChildPropertyChanged;
	}

	internal void AddZoomGestures()
	{
		if (m_panZoomListener != null)
		{
			this.AddGestureListener(m_panZoomListener);
			this.AddKeyboardListener(m_panZoomListener);
			this.AddTouchListener(m_panZoomListener);
		}
	}

	internal void RemoveZoomGestures()
	{
		this.ClearKeyboardListeners();
		this.ClearTouchListeners();
		this.ClearGestureListeners();
	}

	private double GetAlignmentProportion(LayoutAlignment layoutAlignment)
	{
		return layoutAlignment switch
		{
			LayoutAlignment.Start => 0.0, 
			LayoutAlignment.End => 1.0, 
			_ => 0.5, 
		};
	}

	internal void LayoutContent()
	{
		if (base.Children.Count > 0 && base.Children[0] is View view)
		{
			SetLayoutFlags((IView)view, AbsoluteLayoutFlags.PositionProportional);
			double alignmentProportion = GetAlignmentProportion(view.HorizontalOptions.Alignment);
			double alignmentProportion2 = GetAlignmentProportion(view.VerticalOptions.Alignment);
			SetLayoutBounds((IView)view, new Rect(alignmentProportion, alignmentProportion2, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
		}
	}

	internal void RequestSize(double width, double height)
	{
		base.WidthRequest = width;
		base.HeightRequest = height;
	}

	internal void Unload()
	{
		RemoveZoomGestures();
		if (base.Children.Count > 0)
		{
			base.Children.Clear();
		}
	}

	~ContentPlaceHolder()
	{
		base.ChildAdded -= OnChildAdded;
		base.ChildRemoved -= OnChildRemoved;
	}

	internal void Reset()
	{
		if (base.Children.Count > 0)
		{
			base.Children.Clear();
		}
		RequestSize(-1.0, -1.0);
	}
}
