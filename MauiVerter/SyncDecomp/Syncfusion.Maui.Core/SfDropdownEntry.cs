using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core.Internals;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Core;

public abstract class SfDropdownEntry : SfView, ITouchListener, ITextElement, IKeyboardListener
{
	private SfInputView? inputView = new SfInputView();

	private IDropdownRenderer? renderer;

	private RectF clearButtonRectF;

	private RectF dropdownButtonRectF;

	private RectF previousRectBounds;

	private readonly int buttonSize = 32;

	private EffectsRenderer? effectsenderer;

	private SfDropdownView? dropDownView;

	private SfTextInputLayout? textInputLayout;

	private bool isEditableMode = true;

	private bool isOpen = false;

	private Color? dropdownArrowColor = Colors.Black;

	private bool isRTL;

	private Point touchPoint;

	public static readonly BindableProperty IsDropDownOpenProperty = BindableProperty.Create(nameof(IsDropDownOpen), typeof(bool), typeof(SfDropdownEntry), false, BindingMode.TwoWay, null, OnIsDropDownOpenPropertyChanged);

	public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(SfDropdownEntry), string.Empty, BindingMode.OneWay, null, OnPlaceholderPropertyChanged);

	public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(SfDropdownEntry), Colors.Gray, BindingMode.OneWay, null, OnPlaceholderColorPropertyChanged);

	public static readonly BindableProperty ClearButtonIconColorProperty = BindableProperty.Create(nameof(ClearButtonIconColor), typeof(Color), typeof(SfDropdownEntry), Colors.Black, BindingMode.OneWay, null, OnClearButtonIconColorPropertyChanged);

	public static readonly BindableProperty StrokeProperty = BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(SfDropdownEntry), GetDefaultStroke(), BindingMode.OneWay, null, OnStrokePropertyChanged);

	public static readonly BindableProperty IsClearButtonVisibleProperty = BindableProperty.Create(nameof(IsClearButtonVisible), typeof(bool), typeof(SfDropdownEntry), true, BindingMode.OneWay, null, OnIsClearButtonVisiblePropertyChanged);

	internal static readonly BindableProperty IsDropDownIconVisibleProperty = BindableProperty.Create(nameof(IsDropdownIconVisible), typeof(bool), typeof(SfDropdownEntry), true, BindingMode.OneWay, null, OnIsDropdownButtonVisiblePropertyChanged);

	internal static readonly BindableProperty IsClearIconVisibleProperty = BindableProperty.Create(nameof(IsClearIconVisible), typeof(bool), typeof(SfDropdownEntry), true, BindingMode.OneWay, null, OnIsClearIconVisiblePropertyChanged);

	internal static readonly BindableProperty DropdownContentProperty = BindableProperty.Create(nameof(DropdownContent), typeof(View), typeof(SfDropdownEntry), null, BindingMode.OneWay, null, OnIsDropdownContentPropertyChanged);

	public static readonly BindableProperty MaxDropDownHeightProperty = BindableProperty.Create(nameof(MaxDropDownHeight), typeof(double), typeof(SfDropdownEntry), 400.0, BindingMode.OneWay, null, OnMaxDropDownHeightPropertyChanged);

	public static readonly BindableProperty FontSizeProperty = FontElement.FontSizeProperty;

	public static readonly BindableProperty FontFamilyProperty = FontElement.FontFamilyProperty;

	public static readonly BindableProperty FontAttributesProperty = FontElement.FontAttributesProperty;

	public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(SfDropdownEntry), Colors.Black, BindingMode.Default, null, OnITextElementPropertyChanged);

	public bool IsClearButtonVisible
	{
		get
		{
			return (bool)GetValue(IsClearButtonVisibleProperty);
		}
		set
		{
			SetValue(IsClearButtonVisibleProperty, value);
		}
	}

	internal bool IsClearIconVisible
	{
		get
		{
			return (bool)GetValue(IsClearIconVisibleProperty);
		}
		set
		{
			SetValue(IsClearIconVisibleProperty, value);
		}
	}

	public string Placeholder
	{
		get
		{
			return (string)GetValue(PlaceholderProperty);
		}
		set
		{
			SetValue(PlaceholderProperty, value);
		}
	}

	public Color PlaceholderColor
	{
		get
		{
			return (Color)GetValue(PlaceholderColorProperty);
		}
		set
		{
			SetValue(PlaceholderColorProperty, value);
		}
	}

	public Color ClearButtonIconColor
	{
		get
		{
			return (Color)GetValue(ClearButtonIconColorProperty);
		}
		set
		{
			SetValue(ClearButtonIconColorProperty, value);
		}
	}

	public double MaxDropDownHeight
	{
		get
		{
			return (double)GetValue(MaxDropDownHeightProperty);
		}
		set
		{
			SetValue(MaxDropDownHeightProperty, value);
		}
	}

	public bool IsDropDownOpen
	{
		get
		{
			return (bool)GetValue(IsDropDownOpenProperty);
		}
		set
		{
			SetValue(IsDropDownOpenProperty, value);
		}
	}

	internal IKeyboardListener KeyboardListener { get; set; }

	internal Color? DropDownArrowColor
	{
		get
		{
			return dropdownArrowColor;
		}
		set
		{
			dropdownArrowColor = value;
			InvalidateDrawable();
		}
	}

	internal bool IsTextChangedFromAppend { get; set; }

	internal SfDropdownView? DropDownView
	{
		get
		{
			return dropDownView;
		}
		set
		{
			dropDownView = value;
		}
	}

	internal SfInputView? InputView
	{
		get
		{
			return inputView;
		}
		set
		{
			inputView = value;
		}
	}

	public Color Stroke
	{
		get
		{
			return (Color)GetValue(StrokeProperty);
		}
		set
		{
			SetValue(StrokeProperty, value);
		}
	}

	internal bool IsEditableMode
	{
		get
		{
			return isEditableMode;
		}
		set
		{
			isEditableMode = value;
			if (InputView != null)
			{
				if (IsEditableMode)
				{
					InputView.IsReadOnly = false;
					InputView.InputTransparent = false;
				}
				else
				{
					InputView.IsReadOnly = true;
					InputView.InputTransparent = true;
				}
			}
			UpdateElementsBounds(base.Bounds);
		}
	}

	internal bool IsDropdownIconVisible
	{
		get
		{
			return (bool)GetValue(IsDropDownIconVisibleProperty);
		}
		set
		{
			SetValue(IsDropDownIconVisibleProperty, value);
		}
	}

	internal View? DropdownContent
	{
		get
		{
			return (View)GetValue(DropdownContentProperty);
		}
		set
		{
			SetValue(DropdownContentProperty, value);
		}
	}

	private bool IsTextInputLayout { get; set; }

	[TypeConverter(typeof(FontSizeConverter))]
	public double FontSize
	{
		get
		{
			return (double)GetValue(FontSizeProperty);
		}
		set
		{
			SetValue(FontSizeProperty, value);
		}
	}

	public FontAttributes FontAttributes
	{
		get
		{
			return (FontAttributes)GetValue(FontAttributesProperty);
		}
		set
		{
			SetValue(FontAttributesProperty, value);
		}
	}

	public string FontFamily
	{
		get
		{
			return (string)GetValue(FontFamilyProperty);
		}
		set
		{
			SetValue(FontFamilyProperty, value);
		}
	}

	public Color TextColor
	{
		get
		{
			return (Color)GetValue(TextColorProperty);
		}
		set
		{
			SetValue(TextColorProperty, value);
		}
	}

	Microsoft.Maui.Font ITextElement.Font => (Microsoft.Maui.Font)GetValue(FontElement.FontProperty);

	bool IKeyboardListener.CanBecomeFirstResponder => true;

	private static void OnIsDropDownOpenPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		SfDropdownEntry sfDropdownEntry = bindable as SfDropdownEntry;
		if (sfDropdownEntry?.DropDownView != null)
		{
			if (sfDropdownEntry.IsDropDownOpen && sfDropdownEntry.DropdownContent != null)
			{
				sfDropdownEntry.ShowDropdown();
			}
			else
			{
				sfDropdownEntry.HideDropdown();
			}
		}
	}

	private static void OnIsDropdownButtonVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		SfDropdownEntry sfDropdownEntry = bindable as SfDropdownEntry;
		if (sfDropdownEntry == null)
		{
		}
	}

	private static void OnMaxDropDownHeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfDropdownEntry sfDropdownEntry && sfDropdownEntry.dropDownView != null)
		{
			sfDropdownEntry.dropDownView.PopupHeight = (double)newValue;
		}
	}

	private static void OnStrokePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfDropdownEntry sfDropdownEntry)
		{
			sfDropdownEntry.InvalidateDrawable();
		}
	}

	private static void OnIsDropdownContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		SfDropdownEntry sfDropdownEntry = bindable as SfDropdownEntry;
		if (newValue is View listView && sfDropdownEntry?.dropDownView != null)
		{
			sfDropdownEntry.dropDownView.UpdatePopupContent(listView);
		}
	}

	private static void OnClearButtonIconColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfDropdownEntry sfDropdownEntry)
		{
			sfDropdownEntry.InvalidateDrawable();
		}
	}

	private static void OnIsClearIconVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfDropdownEntry sfDropdownEntry)
		{
			sfDropdownEntry.UpdateElementsBounds(sfDropdownEntry.Bounds);
		}
	}

	private static void OnIsClearButtonVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfDropdownEntry sfDropdownEntry)
		{
			sfDropdownEntry.IsClearIconVisible = (bool)newValue;
			sfDropdownEntry.UpdateElementsBounds(sfDropdownEntry.Bounds);
		}
	}

	private static void OnPlaceholderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfDropdownEntry sfDropdownEntry && sfDropdownEntry.InputView != null)
		{
			sfDropdownEntry.InputView.Placeholder = (string)newValue;
		}
	}

	private static void OnPlaceholderColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfDropdownEntry sfDropdownEntry && sfDropdownEntry.InputView != null)
		{
			sfDropdownEntry.InputView.PlaceholderColor = (Color)newValue;
		}
	}

	double ITextElement.FontSizeDefaultValueCreator()
	{
		return 14.0;
	}

	void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
	{
		if (InputView != null)
		{
			InputView.FontAttributes = newValue;
		}
		OnFontPropertiesChanged();
	}

	void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
	{
		if (InputView != null)
		{
			InputView.FontFamily = newValue;
		}
		OnFontPropertiesChanged();
	}

	void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
	{
		if (InputView != null)
		{
			InputView.FontSize = newValue;
		}
		OnFontPropertiesChanged();
	}

	private static void OnITextElementPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
	{
		if (bindable is SfDropdownEntry sfDropdownEntry && sfDropdownEntry.InputView != null)
		{
			sfDropdownEntry.InputView.TextColor = (Color)newValue;
			sfDropdownEntry.OnFontPropertiesChanged();
		}
	}

	public void OnFontChanged(Microsoft.Maui.Font oldValue, Microsoft.Maui.Font newValue)
	{
	}

	public SfDropdownEntry()
	{
		base.DrawingOrder = DrawingOrder.AboveContent;
		InitializeElements();
		this.AddTouchListener(this);
		InputView?.AddKeyboardListener(this);
		this.AddKeyboardListener(this);
		KeyboardListener = this;
		SetRendererBasedOnPlatform();
		base.SizeChanged += SfDropdownEntry_SizeChanged;
	}

	private void SfDropdownEntry_SizeChanged(object? sender, EventArgs e)
	{
		ValidateInputTextLayout();
	}

	private void ValidateInputTextLayout()
	{
		for (Element parent = base.Parent; parent != null; parent = parent.Parent)
		{
			if (parent is SfTextInputLayout sfTextInputLayout)
			{
				textInputLayout = sfTextInputLayout;
				UpdateTextInputLayoutUI(sfTextInputLayout);
				IsTextInputLayout = true;
				break;
			}
		}
	}

	private void UpdateTextInputLayoutUI(SfTextInputLayout textInputLayout)
	{
		textInputLayout.ShowDropDownButton = IsDropdownIconVisible;
		textInputLayout.ShowClearButton = IsClearButtonVisible;
	}

	private async void SetInputViewFocus()
	{
		await Task.Delay(50);
		InputView?.Focus();
	}

	private static Color GetDefaultStroke()
	{
		return Color.FromRgba(141, 141, 141, 255);
	}

	private void InitializeElements()
	{
		if (dropDownView == null)
		{
			dropDownView = new SfDropdownView();
			dropDownView.PopupOpened += DropDownView_PopupOpened;
			dropDownView.PopupClosed += DropDownView_PopupClosed;
			dropDownView.AnchorView = this;
			dropDownView.PopupHeight = MaxDropDownHeight;
			base.Children.Add(dropDownView);
		}
		effectsenderer ??= new EffectsRenderer(this);
		InputView ??= new SfInputView();
		InputView.Drawable = this;
		InputView.IsTextPredictionEnabled = false;
		InputView.IsSpellCheckEnabled = false;
		base.Children.Add(InputView);
		InputView.HandlerChanged += InputView_HandlerChanged;
	}

	private void DropDownView_PopupClosed(object? sender, EventArgs e)
	{
		if (IsDropDownOpen)
		{
			IsDropDownOpen = false;
		}
	}

	private void DropDownView_PopupOpened(object? sender, EventArgs e)
	{
		if (!IsDropDownOpen)
		{
			IsDropDownOpen = true;
		}
		if (!IsTextInputLayout && DropDownView != null)
		{
			DropDownView.PopupWidth = base.Width;
		}
		SetInputViewFocus();
	}

	private void InputView_HandlerChanged(object? sender, EventArgs e)
	{
		if (InputView != null)
		{
			base.BackgroundColor ??= Colors.White;
			if (InputView.Handler == null)
			{
			}
		}
	}

	private void SetRendererBasedOnPlatform()
	{
		renderer = new FluentDropdownEntryRenderer();
	}

	internal void UpdateElementsBounds(RectF bounds)
	{
		previousRectBounds = bounds;
		int num = 0;
		if (IsDropdownIconVisible)
		{
			if (!isRTL)
			{
				dropdownButtonRectF.X = bounds.X + bounds.Width - (float)buttonSize;
			}
			else
			{
				dropdownButtonRectF.X = 0f;
			}
			dropdownButtonRectF.Y = bounds.Center.Y - (float)(buttonSize / 2);
			dropdownButtonRectF.Width = buttonSize;
			dropdownButtonRectF.Height = buttonSize;
			num = buttonSize;
		}
		if (IsClearIconVisible && IsEditableMode)
		{
			if (IsDropdownIconVisible)
			{
				if (!isRTL)
				{
					clearButtonRectF.X = dropdownButtonRectF.X - (float)buttonSize;
				}
				else
				{
					clearButtonRectF.X = dropdownButtonRectF.X + (float)buttonSize;
				}
				num = buttonSize * 2;
			}
			else
			{
				if (!isRTL)
				{
					clearButtonRectF.X = bounds.Width - (float)buttonSize;
				}
				else
				{
					clearButtonRectF.X = 0f;
				}
				num = buttonSize;
			}
			clearButtonRectF.Y = bounds.Center.Y - (float)(buttonSize / 2);
			clearButtonRectF.Width = buttonSize;
			clearButtonRectF.Height = buttonSize;
			if (IsTextInputLayout)
			{
				clearButtonRectF = new RectF(0f, 0f, 0f, 0f);
				dropdownButtonRectF = new RectF(0f, 0f, 0f, 0f);
			}
		}
		if (InputView != null)
		{
			InputView.ButtonSize = buttonSize + 4;
			InputView.Margin = ((IsTextInputLayout && IsEditableMode) ? new Thickness(0.0, 0.0, 0.0, 0.0) : new Thickness(0.0, 0.0, num, 0.0));
		}
		UpdateEffectsRendererBounds();
		if (DropdownContent != null && DropdownContent.WidthRequest != (double)bounds.Width && !IsTextInputLayout)
		{
			DropdownContent.WidthRequest = bounds.Width;
		}
		InvalidateDrawable();
	}

	private void UpdateEffectsRendererBounds()
	{
		if (effectsenderer != null)
		{
			effectsenderer.RippleBoundsCollection.Clear();
			effectsenderer.HighlightBoundsCollection.Clear();
			if (IsClearIconVisible && IsEditableMode)
			{
				effectsenderer.RippleBoundsCollection.Add(clearButtonRectF);
				effectsenderer.HighlightBoundsCollection.Add(clearButtonRectF);
			}
			if (IsDropdownIconVisible && !IsTextInputLayout)
			{
				effectsenderer.RippleBoundsCollection.Add(dropdownButtonRectF);
				effectsenderer.HighlightBoundsCollection.Add(dropdownButtonRectF);
			}
		}
	}

	private void DrawEntryUI(ICanvas canvas, Rect dirtyRect)
	{
		if (InputView == null)
		{
			return;
		}
		canvas.Alpha = 1f;
		canvas.FillColor = Colors.Black;
		canvas.StrokeColor = Colors.Black;
		if (renderer != null)
		{
			if (IsDropdownIconVisible && !IsTextInputLayout)
			{
				canvas.StrokeColor = DropDownArrowColor;
				canvas.FillColor = DropDownArrowColor;
				renderer.DrawDropDownButton(canvas, dropdownButtonRectF);
			}
			if (IsClearIconVisible && IsEditableMode && !IsTextInputLayout)
			{
				canvas.StrokeColor = ClearButtonIconColor;
				renderer.DrawClearButton(canvas, clearButtonRectF);
			}
			if (InputView.FocusedStroke != null && !IsTextInputLayout)
			{
				renderer.DrawBorder(canvas, dirtyRect, InputView.IsFocused, Stroke, InputView.FocusedStroke);
			}
		}
		effectsenderer?.DrawEffects(canvas);
	}

	public new void Focus()
	{
		InputView?.Focus();
	}

	public new void Unfocus()
	{
		InputView?.Unfocus();
	}

	public void OnKeyDown(KeyEventArgs args)
	{
		if (InputView != null)
		{
			if (InputView.IsDeletedButtonPressed)
			{
				InputView.IsDeletedButtonPressed = false;
			}
			if (args.Key == KeyboardKey.Back)
			{
				InputView.IsDeletedButtonPressed = true;
			}
			if (args.Key == KeyboardKey.Delete)
			{
				InputView.IsDeletedButtonPressed = true;
			}
		}
		if (args.Key == KeyboardKey.Enter || args.Key == KeyboardKey.Tab)
		{
			ValidateSelectedItemOnEnter();
		}
		else if (args.Key == KeyboardKey.Down)
		{
			OnDownButtonPressed();
		}
		else if (args.Key == KeyboardKey.Escape && IsDropDownOpen)
		{
			IsDropDownOpen = false;
		}
	}

	public void OnKeyUp(KeyEventArgs args)
	{
	}

	internal void ShowDropdown()
	{
		if (dropDownView != null && DropdownContent != null)
		{
			dropDownView.UpdatePopupContent(DropdownContent);
			dropDownView.IsOpen = true;
		}
	}

	internal void HideDropdown()
	{
		if (dropDownView != null)
		{
			dropDownView.IsOpen = false;
		}
	}

	internal void SetRTL()
	{
		if ((((IVisualElementController)this).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
		{
			isRTL = true;
		}
		else
		{
			isRTL = false;
		}
	}

	protected override Size ArrangeContent(Rect bounds)
	{
		UpdateElementsBounds(bounds);
		return base.ArrangeContent(bounds);
	}

	protected override Size MeasureContent(double widthConstraint, double heightConstraint)
	{
		double num = 0.0;
		double num2 = 0.0;
		Size size = new Size(0.0, 0.0);
		if (InputView != null)
		{
			size = InputView.Measure(widthConstraint, heightConstraint, MeasureFlags.IncludeMargins);
		}
		num = ((widthConstraint != -1.0 && widthConstraint != double.PositiveInfinity) ? widthConstraint : size.Width);
		num2 = ((heightConstraint != -1.0 && heightConstraint != double.PositiveInfinity) ? heightConstraint : size.Height);
		return new Size(num, num2);
	}

	protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
	{
		base.OnDraw(canvas, dirtyRect);
		DrawEntryUI(canvas, dirtyRect);
		if (!isRTL)
		{
			base.Clip = new RoundRectangleGeometry(5.0, dirtyRect);
		}
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		if (dropDownView != null)
		{
			dropDownView.PopupOpened -= DropDownView_PopupOpened;
			dropDownView.PopupClosed -= DropDownView_PopupClosed;
		}
		if (InputView != null)
		{
			InputView.HandlerChanged -= InputView_HandlerChanged;
		}
		if (base.Handler != null)
		{
			if (dropDownView != null)
			{
				dropDownView.PopupOpened += DropDownView_PopupOpened;
				dropDownView.PopupClosed += DropDownView_PopupClosed;
			}
			if (InputView != null)
			{
				InputView.HandlerChanged += InputView_HandlerChanged;
			}
		}
	}

	void ITouchListener.OnTouch(PointerEventArgs e)
	{
		if (isRTL && DeviceInfo.Platform == DevicePlatform.WinUI)
		{
			touchPoint = e.TouchPoint;
			touchPoint.X = base.Width - touchPoint.X;
		}
		else
		{
			touchPoint = e.TouchPoint;
		}
		if (e.Action == PointerActions.Pressed)
		{
			isOpen = IsDropDownOpen;
			if (clearButtonRectF.Contains(touchPoint) && IsClearIconVisible && IsEditableMode)
			{
				OnClearButtonTouchDown(e);
			}
			else if ((dropdownButtonRectF.Contains(touchPoint) && IsDropdownIconVisible) || !IsEditableMode)
			{
				OnDropdownButtonTouchDown(e);
			}
		}
		if (e.Action == PointerActions.Released)
		{
			if (clearButtonRectF.Contains(touchPoint) && IsClearIconVisible && IsEditableMode)
			{
				OnClearButtonTouchUp(e);
			}
			else if (!isOpen && ((dropdownButtonRectF.Contains(touchPoint) && IsDropdownIconVisible) || !IsEditableMode))
			{
				OnDropdownButtonTouchUp(e);
			}
			isOpen = false;
		}
	}

	internal abstract void ValidateSelectedItemOnEnter();

	internal abstract void OnDownButtonPressed();

	internal abstract void OnFontPropertiesChanged();

	internal virtual void OnClearButtonTouchDown(PointerEventArgs e)
	{
	}

	internal virtual void OnClearButtonTouchUp(PointerEventArgs e)
	{
	}

	internal virtual void OnDropdownButtonTouchDown(PointerEventArgs e)
	{
	}

	internal virtual void OnDropdownButtonTouchUp(PointerEventArgs e)
	{
		if (!IsDropDownOpen)
		{
			OnDropdownOpening();
		}
	}

	protected virtual void OnDropdownOpening()
	{
		ShowDropdown();
	}
}
