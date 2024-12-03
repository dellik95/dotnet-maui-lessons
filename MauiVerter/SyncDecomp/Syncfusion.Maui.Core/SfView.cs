using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace Syncfusion.Maui.Core;

public abstract class SfView : View, IDrawableLayout, IDrawable, IAbsoluteLayout, Microsoft.Maui.ILayout, IView, IElement, ITransform, IContainer, IList<IView>, ICollection<IView>, IEnumerable<IView>, IEnumerable, ISafeAreaView, IPadding, IVisualTreeElement
{
	private readonly ILayoutManager layoutManager;

	private DrawingOrder drawingOrder = DrawingOrder.NoDraw;

	private bool clipToBounds = true;

	private Thickness padding = new Thickness(0.0);

	private readonly List<IView> children = new List<IView>();

	private SfViewHandler? LayoutHandler => base.Handler as SfViewHandler;

	internal DrawingOrder DrawingOrder
	{
		get
		{
			return drawingOrder;
		}
		set
		{
			drawingOrder = value;
			LayoutHandler?.SetDrawingOrder(value);
		}
	}

	public IList<IView> Children => this;

	public bool ClipToBounds
	{
		get
		{
			return clipToBounds;
		}
		set
		{
			clipToBounds = value;
			LayoutHandler?.UpdateClipToBounds(value);
		}
	}

	public Thickness Padding
	{
		get
		{
			return padding;
		}
		set
		{
			padding = value;
			MeasureContent(base.Width, base.Height);
			ArrangeContent(base.Bounds);
		}
	}

	bool Microsoft.Maui.ILayout.ClipsToBounds => ClipToBounds;

	int ICollection<IView>.Count => children.Count;

	bool ICollection<IView>.IsReadOnly => ((ICollection<IView>)children).IsReadOnly;

	bool ISafeAreaView.IgnoreSafeArea => false;

	Thickness IPadding.Padding => Padding;

	DrawingOrder IDrawableLayout.DrawingOrder
	{
		get
		{
			return DrawingOrder;
		}
		set
		{
			DrawingOrder = value;
		}
	}

	IView IList<IView>.this[int index]
	{
		get
		{
			return children[index];
		}
		set
		{
			children[index] = value;
		}
	}

	IReadOnlyList<IVisualTreeElement> IVisualTreeElement.GetVisualChildren()
	{
		return Children.Cast<IVisualTreeElement>().ToList().AsReadOnly();
	}

	public SfView()
	{
		layoutManager = new DrawableLayoutManager(this);
	}

	protected virtual void OnDraw(ICanvas canvas, RectF dirtyRect)
	{
	}

	protected virtual Size MeasureContent(double widthConstraint, double heightConstraint)
	{
		return layoutManager.Measure(widthConstraint, heightConstraint);
	}

	protected virtual Size ArrangeContent(Rect bounds)
	{
		return layoutManager.ArrangeChildren(bounds);
	}

	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();
		foreach (IView child in children)
		{
			if (child is BindableObject view)
			{
				UpdateBindingContextToChild(view);
			}
		}
	}

	protected sealed override Size MeasureOverride(double widthConstraint, double heightConstraint)
	{
		return base.MeasureOverride(widthConstraint, heightConstraint);
	}

	protected sealed override Size ArrangeOverride(Rect bounds)
	{
		return base.ArrangeOverride(bounds);
	}

	protected sealed override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
	{
		return base.OnMeasure(widthConstraint, heightConstraint);
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		if (base.Handler != null)
		{
			LayoutHandler?.SetDrawingOrder(DrawingOrder);
			LayoutHandler?.UpdateClipToBounds(ClipToBounds);
			LayoutHandler?.Invalidate();
		}
	}

	internal void Add(View view)
	{
		((ICollection<IView>)this).Add((IView)view);
	}

	private void UpdateBindingContextToChild(BindableObject view)
	{
		BindableObject.SetInheritedBindingContext(view, base.BindingContext);
	}

	internal void Remove(View view)
	{
		((ICollection<IView>)this).Remove((IView)view);
	}

	internal void Insert(int index, View view)
	{
		((IList<IView>)this).Insert(index, (IView)view);
	}

	internal void Clear()
	{
		((ICollection<IView>)this).Clear();
	}

	internal void InvalidateDrawable()
	{
		LayoutHandler?.Invalidate();
	}

	void IDrawableLayout.InvalidateDrawable()
	{
		InvalidateDrawable();
	}

	Size Microsoft.Maui.ILayout.CrossPlatformMeasure(double widthConstraint, double heightConstraint)
	{
		return MeasureContent(widthConstraint, heightConstraint);
	}

	Size Microsoft.Maui.ILayout.CrossPlatformArrange(Rect bounds)
	{
		return ArrangeContent(bounds);
	}

	int IList<IView>.IndexOf(IView child)
	{
		return children.IndexOf(child);
	}

	void IList<IView>.Insert(int index, IView child)
	{
		children.Insert(index, child);
		LayoutHandler?.Insert(index, child);
		if (child is Element child2)
		{
			OnChildAdded(child2);
		}
	}

	void IList<IView>.RemoveAt(int index)
	{
		IView view = children[index];
		LayoutHandler?.Remove(children[index]);
		children.RemoveAt(index);
		if (view is Element child)
		{
			OnChildRemoved(child, index);
		}
	}

	void ICollection<IView>.Add(IView child)
	{
		children.Add(child);
		LayoutHandler?.Add(child);
		if (child is Element child2)
		{
			OnChildAdded(child2);
		}
	}

	void ICollection<IView>.Clear()
	{
		children.Clear();
		LayoutHandler?.Clear();
	}

	bool ICollection<IView>.Contains(IView child)
	{
		return children.Contains(child);
	}

	void ICollection<IView>.CopyTo(IView[] array, int arrayIndex)
	{
		children.CopyTo(array, arrayIndex);
	}

	bool ICollection<IView>.Remove(IView child)
	{
		int oldLogicalIndex = children.IndexOf(child);
		LayoutHandler?.Remove(child);
		bool result = children.Remove(child);
		if (child is Element child2)
		{
			OnChildRemoved(child2, oldLogicalIndex);
		}
		return result;
	}

	IEnumerator<IView> IEnumerable<IView>.GetEnumerator()
	{
		return children.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return children.GetEnumerator();
	}

	void IDrawable.Draw(ICanvas canvas, RectF dirtyRect)
	{
		OnDraw(canvas, dirtyRect);
	}

	Rect IAbsoluteLayout.GetLayoutBounds(IView view)
	{
		BindableObject bindableObject = (BindableObject)view;
		return (Rect)bindableObject.GetValue(AbsoluteLayout.LayoutBoundsProperty);
	}

	AbsoluteLayoutFlags IAbsoluteLayout.GetLayoutFlags(IView view)
	{
		BindableObject bindableObject = (BindableObject)view;
		return (AbsoluteLayoutFlags)bindableObject.GetValue(AbsoluteLayout.LayoutFlagsProperty);
	}
}
