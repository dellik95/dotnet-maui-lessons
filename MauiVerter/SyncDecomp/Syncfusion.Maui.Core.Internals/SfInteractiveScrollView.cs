using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace Syncfusion.Maui.Core.Internals;

[ContentProperty("Content")]
public class SfInteractiveScrollView : View, ITapGestureListener, IGestureListener
{
	internal PanZoomListener? m_panZoomListener;

	internal Size m_contentSize = Size.Zero;

	private TaskCompletionSource<bool>? m_scrollCompletionSource;

	private double m_currentHorizontalOffset = 0.0;

	private double m_currentVerticalOffset = 0.0;

	private double m_currentHorizontalProportion = 0.0;

	private double m_currentVerticalProportion = 0.0;

	internal PanGestureManager? m_panGestureManager;

	private static readonly BindablePropertyKey ZoomFactorPropertyKey = BindableProperty.CreateReadOnly(nameof(ZoomFactor), typeof(double), typeof(SfInteractiveScrollView), 1.0);

	public static readonly BindableProperty ZoomFactorProperty = ZoomFactorPropertyKey.BindableProperty;

	public static readonly BindableProperty MinZoomFactorProperty = BindableProperty.Create(nameof(MinZoomFactor), typeof(double), typeof(SfInteractiveScrollView), 0.1, BindingMode.OneWay, null, coerceValue: CoerceMinZoom, propertyChanged: OnMinZoomChanged);

	public static readonly BindableProperty MaxZoomFactorProperty = BindableProperty.Create(nameof(MaxZoomFactor), typeof(double), typeof(SfInteractiveScrollView), 10.0, BindingMode.OneWay, null, coerceValue: CoerceMaxZoom, propertyChanged: OnMaxZoomChanged);

	private static readonly BindableProperty CanBecomeFirstResponderProperty = BindableProperty.Create(nameof(CanBecomeFirstResponder), typeof(bool), typeof(SfInteractiveScrollView), true);

	public static readonly BindableProperty AllowZoomProperty = BindableProperty.Create(nameof(AllowZoom), typeof(bool), typeof(SfInteractiveScrollView), false, BindingMode.OneWay, null, OnAllowZoomChanged);

	private static readonly BindablePropertyKey ViewportHeightPropertyKey = BindableProperty.CreateReadOnly(nameof(ViewportHeight), typeof(double), typeof(SfInteractiveScrollView), 0.0);

	public static readonly BindableProperty ViewportHeightProperty = ViewportHeightPropertyKey.BindableProperty;

	private static readonly BindablePropertyKey ViewportWidthPropertyKey = BindableProperty.CreateReadOnly(nameof(ViewportWidth), typeof(double), typeof(SfInteractiveScrollView), 0.0);

	public static readonly BindableProperty ViewportWidthProperty = ViewportWidthPropertyKey.BindableProperty;

	public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(ScrollOrientation), typeof(SfInteractiveScrollView), ScrollOrientation.Vertical, BindingMode.OneWay, null, OnOrientationChanged);

	private static readonly BindablePropertyKey ScrollYPropertyKey = BindableProperty.CreateReadOnly(nameof(ScrollY), typeof(double), typeof(SfInteractiveScrollView), 0.0, BindingMode.OneWayToSource, null, OnVerticalOffsetChanged);

	public static readonly BindableProperty ScrollYProperty = ScrollYPropertyKey.BindableProperty;

	private static readonly BindablePropertyKey ScrollXPropertyKey = BindableProperty.CreateReadOnly(nameof(ScrollX), typeof(double), typeof(SfInteractiveScrollView), 0.0, BindingMode.OneWayToSource, null, OnHorizontalOffsetChanged);

	public static readonly BindableProperty ScrollXProperty = ScrollXPropertyKey.BindableProperty;

	public static readonly BindableProperty ScrollYProportionProperty = BindableProperty.Create(nameof(ScrollYProportion), typeof(double), typeof(SfInteractiveScrollView), 0.0, BindingMode.OneWay, null, OnVerticalProportionalOffsetChanged);

	public static readonly BindableProperty ScrollXProportionProperty = BindableProperty.Create(nameof(ScrollXProportion), typeof(double), typeof(SfInteractiveScrollView), 0.0, BindingMode.OneWay, null, OnHorizontalProportionalOffsetChanged);

	public static readonly BindableProperty VerticalScrollBarVisibilityProperty = BindableProperty.Create(nameof(VerticalScrollBarVisibility), typeof(ScrollBarVisibility), typeof(SfInteractiveScrollView), ScrollBarVisibility.Default);

	public static readonly BindableProperty HorizontalScrollBarVisibilityProperty = BindableProperty.Create(nameof(HorizontalScrollBarVisibility), typeof(ScrollBarVisibility), typeof(SfInteractiveScrollView), ScrollBarVisibility.Default);

	public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(SfInteractiveScrollView), null, BindingMode.OneWay, null, OnContentChanged);

	private static readonly BindableProperty ContentSizeProperty = BindableProperty.Create(nameof(ContentSize), typeof(Size), typeof(ScrollView), default(Size), BindingMode.OneWay, null, OnExtentSizeChanged);

	public static readonly BindableProperty ExtentSizeProperty = BindableProperty.Create(nameof(ExtentSize), typeof(Size?), typeof(ScrollView), null, BindingMode.OneWay, null, OnExtentSizeRequestChanged);

	public static readonly BindableProperty ZoomLocationProperty = BindableProperty.Create(nameof(ZoomLocation), typeof(ZoomLocation), typeof(ScrollView), ZoomLocation.Default);

	internal bool IsScrolling { get; set; }

	internal bool IsContentLayoutRequested { get; set; }

	internal ContentPlaceHolder? PresentedContent { get; set; }

	internal bool CanBecomeFirstResponder
	{
		get
		{
			return (bool)GetValue(CanBecomeFirstResponderProperty);
		}
		set
		{
			SetValue(CanBecomeFirstResponderProperty, value);
		}
	}

	public ZoomLocation ZoomLocation
	{
		get
		{
			return (ZoomLocation)GetValue(ZoomLocationProperty);
		}
		set
		{
			SetValue(ZoomLocationProperty, value);
		}
	}

	public double ScrollYProportion
	{
		get
		{
			return (double)GetValue(ScrollYProportionProperty);
		}
		set
		{
			SetValue(ScrollYProportionProperty, value);
		}
	}

	public double ScrollXProportion
	{
		get
		{
			return (double)GetValue(ScrollXProportionProperty);
		}
		set
		{
			SetValue(ScrollXProportionProperty, value);
		}
	}

	public double ViewportHeight
	{
		get
		{
			return (double)GetValue(ViewportHeightProperty);
		}
		internal set
		{
			SetValue(ViewportHeightPropertyKey, value);
		}
	}

	public double ViewportWidth
	{
		get
		{
			return (double)GetValue(ViewportWidthProperty);
		}
		internal set
		{
			SetValue(ViewportWidthPropertyKey, value);
		}
	}

	public double MinZoomFactor
	{
		get
		{
			return (double)GetValue(MinZoomFactorProperty);
		}
		set
		{
			SetValue(MinZoomFactorProperty, value);
		}
	}

	public double MaxZoomFactor
	{
		get
		{
			return (double)GetValue(MaxZoomFactorProperty);
		}
		set
		{
			SetValue(MaxZoomFactorProperty, value);
		}
	}

	public DoubleTapSettings? DoubleTapSettings
	{
		get
		{
			if (m_panZoomListener != null)
			{
				return m_panZoomListener.DoubleTapSettings;
			}
			return null;
		}
	}

	public ScrollBarVisibility VerticalScrollBarVisibility
	{
		get
		{
			return (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty);
		}
		set
		{
			SetValue(VerticalScrollBarVisibilityProperty, value);
		}
	}

	public ScrollBarVisibility HorizontalScrollBarVisibility
	{
		get
		{
			return (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty);
		}
		set
		{
			SetValue(HorizontalScrollBarVisibilityProperty, value);
		}
	}

	public ScrollOrientation Orientation
	{
		get
		{
			return (ScrollOrientation)GetValue(OrientationProperty);
		}
		set
		{
			SetValue(OrientationProperty, value);
		}
	}

	internal Size ContentSize
	{
		get
		{
			return (Size)GetValue(ContentSizeProperty);
		}
		set
		{
			SetValue(ContentSizeProperty, value);
		}
	}

	public Size? ExtentSize
	{
		get
		{
			return (Size?)GetValue(ExtentSizeProperty);
		}
		set
		{
			SetValue(ExtentSizeProperty, value);
		}
	}

	public double ScrollY
	{
		get
		{
			return (double)GetValue(ScrollYProperty);
		}
		internal set
		{
			SetValue(ScrollYPropertyKey, value);
		}
	}

	public double ScrollX
	{
		get
		{
			return (double)GetValue(ScrollXProperty);
		}
		internal set
		{
			SetValue(ScrollXPropertyKey, value);
		}
	}

	public double ZoomFactor
	{
		get
		{
			return (double)GetValue(ZoomFactorProperty);
		}
		internal set
		{
			SetValue(ZoomFactorPropertyKey, value);
		}
	}

	public View? Content
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

	public bool AllowZoom
	{
		get
		{
			return (bool)GetValue(AllowZoomProperty);
		}
		set
		{
			SetValue(AllowZoomProperty, value);
		}
	}

	private Point CenteredOrigin => new Point(ScrollX + ViewportWidth / 2.0, ScrollY + ViewportHeight / 2.0);

	public event EventHandler<ZoomEventArgs>? ZoomChanged;

	public event EventHandler<ZoomEventArgs>? ZoomStarted;

	public event EventHandler<ZoomEventArgs>? ZoomEnded;

	public event EventHandler<ScrollChangedEventArgs>? ScrollChanged;

	public SfInteractiveScrollView()
	{
		InitializeControl();
	}

	private void InitializeControl()
	{
		m_panZoomListener = new PanZoomListener();
		m_panZoomListener.PanMode = PanMode.Vertical;
		m_panZoomListener.MouseWheelSettings.ZoomKeyModifier = KeyboardKey.Ctrl;
		m_panGestureManager = new PanGestureManager(this, m_panZoomListener);
		PresentedContent = new ContentPlaceHolder(m_panZoomListener);
		WireEvents();
	}

	public void ZoomBy(double zoomFactor, Point? zoomOrigin = null)
	{
		ZoomTo(ZoomFactor * zoomFactor, zoomOrigin);
	}

	internal void ScrollTo(double x, double y, bool animated = true)
	{
		if (base.Handler != null)
		{
			ScrollToParameters args = new ScrollToParameters(x, y, animated);
			base.Handler.Invoke("ScrollTo", args);
			args = null;
		}
		else
		{
			ScrollX = x;
			ScrollY = y;
		}
	}

	public Task ScrollToAsync(double x, double y, bool animated = true)
	{
		switch (Orientation)
		{
		case ScrollOrientation.Neither:
			return Task.FromResult(result: false);
		case ScrollOrientation.Vertical:
			x = ScrollX;
			break;
		case ScrollOrientation.Horizontal:
			y = ScrollY;
			break;
		}
		CheckTaskCompletionSource();
		ScrollTo(x, y, animated);
		return m_scrollCompletionSource.Task;
	}

	public Task ScrollByAsync(double x, double y, bool animated = true)
	{
		return ScrollToAsync(ScrollX + x, ScrollY + y, animated);
	}

	private void CheckTaskCompletionSource()
	{
		if (m_scrollCompletionSource != null && m_scrollCompletionSource.Task.Status == TaskStatus.Running)
		{
			m_scrollCompletionSource.TrySetCanceled();
		}
		m_scrollCompletionSource = new TaskCompletionSource<bool>();
	}

	internal void ScrollToX(double x, bool animated = true)
	{
		ScrollTo(x, ScrollY, animated);
		ScrollX = x;
	}

	internal void ScrollToY(double y, bool animated = true)
	{
		ScrollTo(ScrollX, y, animated);
		ScrollY = y;
	}

	public void ZoomTo(double zoomFactor, Point? zoomOrigin = null)
	{
		if (base.Handler != null)
		{
			m_panZoomListener?.ZoomTo(zoomFactor, zoomOrigin);
		}
		else
		{
			ZoomFactor = zoomFactor;
		}
	}

	void ITapGestureListener.OnTap(TapEventArgs e)
	{
	}

	private void CheckIfZoomLocationRequested(ZoomEventArgs e)
	{
		if (ZoomLocation == ZoomLocation.Centered && ViewportHeight > 0.0 && ViewportWidth > 0.0)
		{
			e.ZoomOrigin = CenteredOrigin;
		}
	}

	private void AddOrRemoveZoomGestures(bool allowZoom)
	{
		if (allowZoom)
		{
			PresentedContent?.AddZoomGestures();
		}
		else
		{
			PresentedContent?.RemoveZoomGestures();
		}
	}

	internal void UpdateHorizontalProprotion(double horizontalOffset)
	{
		if (horizontalOffset != m_currentHorizontalOffset && ContentSize.Width > 0.0)
		{
			ScrollXProportion = (m_currentHorizontalProportion = horizontalOffset / ContentSize.Width);
		}
	}

	internal void UpdateVerticalProportion(double verticalOffset)
	{
		if (verticalOffset != m_currentVerticalOffset && ContentSize.Height > 0.0)
		{
			ScrollYProportion = (m_currentVerticalProportion = verticalOffset / ContentSize.Height);
		}
	}

	private void ScrollToVerticalOffsetProportionately(double offsetProportion)
	{
		if (ContentSize.Height > 0.0)
		{
			ScrollToY(m_currentVerticalOffset = offsetProportion * ContentSize.Height, animated: false);
		}
	}

	private void ScrollToHorizontalOffsetProportionately(double offsetProportion)
	{
		if (ContentSize.Width > 0.0)
		{
			ScrollToX(m_currentHorizontalOffset = offsetProportion * ContentSize.Width, animated: false);
		}
	}

	private PanMode ConvertOrientationToPanMode(ScrollOrientation orientation)
	{
		return orientation switch
		{
			ScrollOrientation.Neither => PanMode.None, 
			ScrollOrientation.Horizontal => PanMode.Horizontal, 
			ScrollOrientation.Vertical => PanMode.Vertical, 
			_ => PanMode.Both, 
		};
	}

	internal void SendScrollFinished()
	{
		m_scrollCompletionSource?.TrySetResult(result: true);
	}

	private void ApplyPresetProperties()
	{
		if (MinZoomFactor > ZoomFactor)
		{
			ZoomTo(MinZoomFactor);
		}
		else if (ZoomFactor != (double)ZoomFactorProperty.DefaultValue)
		{
			ZoomTo(ZoomFactor);
		}
		if (ScrollY != (double)ScrollYProperty.DefaultValue || ScrollX != (double)ScrollXProperty.DefaultValue)
		{
			IsContentLayoutRequested = true;
			ScrollTo(ScrollX, ScrollY, animated: false);
		}
	}

	internal void RequestLayout(double controlWidth, double controlHeight)
	{
		if (controlWidth <= 0.0 || controlHeight <= 0.0)
		{
			if (controlWidth <= 0.0)
			{
				controlWidth = m_contentSize.Width;
			}
			else if (controlHeight <= 0.0)
			{
				controlHeight = m_contentSize.Height;
			}
		}
		Layout(new Rect(base.Bounds.X, base.Bounds.Y, controlWidth, controlHeight));
	}

	private void UpdatePresentedContent(View content)
	{
		if (PresentedContent != null)
		{
			PresentedContent.Children.Add(content);
			PresentedContent.LayoutContent();
			ApplyPresetProperties();
		}
	}

	private void InvalidateExtentSize(Size extentSize)
	{
		extentSize.Width = Math.Max(extentSize.Width, base.Bounds.Width);
		extentSize.Height = Math.Max(extentSize.Height, base.Bounds.Height);
		ContentSize = extentSize;
	}

	private void Reset()
	{
		ScrollTo(0.0, 0.0, animated: false);
		m_panZoomListener.CurrentZoomFactor = 1.0;
		if (!ExtentSize.HasValue)
		{
			ContentSize = Size.Zero;
		}
		PresentedContent?.Reset();
		ZoomFactor = 1.0;
		double scrollY = (ScrollX = 0.0);
		ScrollY = scrollY;
		m_contentSize = Size.Zero;
	}

	private void WireZoomEvents()
	{
		if (m_panZoomListener != null)
		{
			m_panZoomListener.ZoomStarted += OnZoomStarted;
			m_panZoomListener.ZoomChanged += OnZoomChanged;
			m_panZoomListener.ZoomEnded += OnZoomEnded;
		}
	}

	private void UnwireZoomEvents()
	{
		if (m_panZoomListener != null)
		{
			m_panZoomListener.ZoomStarted -= OnZoomStarted;
			m_panZoomListener.ZoomChanged -= OnZoomChanged;
			m_panZoomListener.ZoomEnded -= OnZoomEnded;
		}
	}

	private void WireEvents()
	{
		base.PropertyChanging += OnPropertyChanging;
		base.PropertyChanged += OnPropertyChanged;
		WireZoomEvents();
	}

	private void UnwireEvents()
	{
		base.PropertyChanging -= OnPropertyChanging;
		base.PropertyChanged -= OnPropertyChanged;
		UnwireZoomEvents();
	}

	private void OnZoomStarted(object? sender, ZoomEventArgs e)
	{
		if (this.ZoomStarted != null)
		{
			CheckIfZoomLocationRequested(e);
			this.ZoomStarted(this, e);
		}
	}

	private void OnZoomChanged(object? sender, ZoomEventArgs e)
	{
		if (this.ZoomChanged != null)
		{
			CheckIfZoomLocationRequested(e);
			this.ZoomChanged(this, e);
		}
		ZoomFactor = e.ZoomFactor;
	}

	private void OnZoomEnded(object? sender, ZoomEventArgs e)
	{
		if (this.ZoomEnded != null)
		{
			CheckIfZoomLocationRequested(e);
			this.ZoomEnded(this, e);
		}
	}

	private void OnPropertyChanging(object sender, Microsoft.Maui.Controls.PropertyChangingEventArgs e)
	{
		string propertyName = e.PropertyName;
		string text = propertyName;
		if (text == "Content")
		{
			if (Content != null)
			{
				Content.SizeChanged -= OnContentSizeChanged;
			}
			Reset();
		}
	}

	private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		string propertyName = e.PropertyName;
		string text = propertyName;
		if (!(text == "Content"))
		{
			if (text == "BackgroundColor" && PresentedContent != null)
			{
				PresentedContent.BackgroundColor = base.BackgroundColor;
			}
			return;
		}
		if (Content != null)
		{
			Content.SizeChanged += OnContentSizeChanged;
		}
		InvalidateMeasure();
	}

	internal void OnScrollChanged(ScrollChangedEventArgs scrolledEventArgs)
	{
		if (ScrollY != scrolledEventArgs.ScrollY || ScrollX != scrolledEventArgs.ScrollX)
		{
			ScrollY = scrolledEventArgs.ScrollY;
			ScrollX = scrolledEventArgs.ScrollX;
			this.ScrollChanged?.Invoke(this, scrolledEventArgs);
		}
	}

	private static object CoerceMinZoom(BindableObject bindable, object value)
	{
		if (bindable is SfInteractiveScrollView sfInteractiveScrollView && double.TryParse(value.ToString(), out var result) && result > sfInteractiveScrollView.MaxZoomFactor)
		{
			value = sfInteractiveScrollView.MaxZoomFactor;
		}
		return value;
	}

	private static object CoerceMaxZoom(BindableObject bindable, object value)
	{
		if (bindable is SfInteractiveScrollView sfInteractiveScrollView && double.TryParse(value.ToString(), out var result) && result < sfInteractiveScrollView.MinZoomFactor)
		{
			value = sfInteractiveScrollView.MinZoomFactor;
		}
		return value;
	}

	private static void OnMinZoomChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfInteractiveScrollView sfInteractiveScrollView)
		{
			double num = (double)newValue;
			sfInteractiveScrollView.m_panZoomListener.MinZoomFactor = num;
			if (sfInteractiveScrollView.ZoomFactor < num)
			{
				sfInteractiveScrollView.ZoomTo(num, new Point(sfInteractiveScrollView.ScrollX, sfInteractiveScrollView.ScrollY));
			}
		}
	}

	private static void OnMaxZoomChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfInteractiveScrollView sfInteractiveScrollView)
		{
			double num = (double)newValue;
			sfInteractiveScrollView.m_panZoomListener.MaxZoomFactor = num;
			if (sfInteractiveScrollView.ZoomFactor > num)
			{
				sfInteractiveScrollView.ZoomTo(num, new Point(sfInteractiveScrollView.ScrollX, sfInteractiveScrollView.ScrollY));
			}
		}
	}

	private static void OnAllowZoomChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfInteractiveScrollView sfInteractiveScrollView && sfInteractiveScrollView.Handler != null)
		{
			bool allowZoom = (bool)newValue;
			sfInteractiveScrollView.AddOrRemoveZoomGestures(allowZoom);
		}
	}

	private static void OnHorizontalOffsetChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfInteractiveScrollView sfInteractiveScrollView && sfInteractiveScrollView.Handler != null)
		{
			sfInteractiveScrollView.UpdateHorizontalProprotion((double)newValue);
		}
	}

	private static void OnVerticalOffsetChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfInteractiveScrollView sfInteractiveScrollView && sfInteractiveScrollView.Handler != null)
		{
			sfInteractiveScrollView.UpdateVerticalProportion((double)newValue);
		}
	}

	private static void OnOrientationChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfInteractiveScrollView sfInteractiveScrollView)
		{
			ScrollOrientation orientation = (ScrollOrientation)newValue;
			sfInteractiveScrollView.m_panZoomListener.PanMode = sfInteractiveScrollView.ConvertOrientationToPanMode(orientation);
		}
	}

	private static void OnVerticalProportionalOffsetChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfInteractiveScrollView sfInteractiveScrollView && sfInteractiveScrollView.Handler != null && sfInteractiveScrollView.m_currentVerticalProportion != (double)newValue)
		{
			sfInteractiveScrollView.ScrollToVerticalOffsetProportionately((double)newValue);
		}
	}

	private static void OnHorizontalProportionalOffsetChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfInteractiveScrollView sfInteractiveScrollView && sfInteractiveScrollView.Handler != null && sfInteractiveScrollView.m_currentHorizontalProportion != (double)newValue)
		{
			sfInteractiveScrollView.ScrollToHorizontalOffsetProportionately((double)newValue);
		}
	}

	private static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfInteractiveScrollView sfInteractiveScrollView && sfInteractiveScrollView.Handler != null)
		{
			sfInteractiveScrollView.UpdatePresentedContent((View)newValue);
		}
	}

	private static void OnExtentSizeChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfInteractiveScrollView sfInteractiveScrollView)
		{
			Size size = (Size)newValue;
			sfInteractiveScrollView.PresentedContent?.RequestSize(size.Width, size.Height);
		}
	}

	private static void OnExtentSizeRequestChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfInteractiveScrollView sfInteractiveScrollView)
		{
			Size? size = (Size?)newValue;
			if (size.HasValue)
			{
				sfInteractiveScrollView.InvalidateExtentSize(size.Value);
			}
		}
	}

	private void OnContentSizeChanged(object? sender, EventArgs e)
	{
		m_contentSize = Content.ComputeDesiredSize(double.PositiveInfinity, double.PositiveInfinity);
		if (!ExtentSize.HasValue && !m_contentSize.IsZero)
		{
			RequestLayout(base.Bounds.Width, base.Bounds.Height);
			InvalidateExtentSize(m_contentSize);
		}
		UpdateVerticalProportion(ScrollY);
		UpdateHorizontalProprotion(ScrollX);
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		if (base.Handler != null)
		{
			AddOrRemoveZoomGestures(AllowZoom);
			if (Content != null)
			{
				UpdatePresentedContent(Content);
			}
		}
		else
		{
			PresentedContent?.Unload();
		}
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		if (!m_contentSize.IsZero)
		{
			RequestLayout(width, height);
			if (!ExtentSize.HasValue)
			{
				InvalidateExtentSize(m_contentSize);
			}
			else
			{
				InvalidateExtentSize(ExtentSize.Value);
			}
		}
		base.OnSizeAllocated(base.Bounds.Width, base.Bounds.Height);
	}
}
