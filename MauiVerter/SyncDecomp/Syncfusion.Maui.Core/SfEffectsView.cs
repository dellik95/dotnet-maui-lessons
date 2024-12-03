using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core.Internals;

namespace Syncfusion.Maui.Core;

[DesignTimeVisible(true)]
[ContentProperty("Content")]
public class SfEffectsView : SfContentView, ITouchListener, ITapGestureListener, IGestureListener, ILongPressGestureListener
{
	public static readonly BindableProperty RippleAnimationDurationProperty = BindableProperty.Create(nameof(RippleAnimationDuration), typeof(double), typeof(SfEffectsView), 275.0, BindingMode.Default);

	public static readonly BindableProperty ScaleAnimationDurationProperty = BindableProperty.Create(nameof(ScaleAnimationDuration), typeof(double), typeof(SfEffectsView), 150.0, BindingMode.Default);

	public static readonly BindableProperty RotationAnimationDurationProperty = BindableProperty.Create(nameof(RotationAnimationDuration), typeof(double), typeof(SfEffectsView), 200.0, BindingMode.Default);

	public static readonly BindableProperty InitialRippleFactorProperty = BindableProperty.Create(nameof(InitialRippleFactor), typeof(double), typeof(SfEffectsView), 0.25, BindingMode.Default);

	public static readonly BindableProperty ScaleFactorProperty = BindableProperty.Create(nameof(ScaleFactor), typeof(double), typeof(SfEffectsView), 1.0, BindingMode.Default);

	public static readonly BindableProperty HighlightBackgroundProperty = BindableProperty.Create(nameof(HighlightBackground), typeof(Brush), typeof(SfEffectsView), new SolidColorBrush(Colors.Black), BindingMode.Default);

	public static readonly BindableProperty RippleBackgroundProperty = BindableProperty.Create(nameof(RippleBackground), typeof(Brush), typeof(SfEffectsView), new SolidColorBrush(Colors.Black), BindingMode.Default);

	public static readonly BindableProperty SelectionBackgroundProperty = BindableProperty.Create(nameof(SelectionBackground), typeof(Brush), typeof(SfEffectsView), new SolidColorBrush(Colors.Black), BindingMode.Default, null, OnSelectionBackgroundPropertyChanged);

	public static readonly BindableProperty AngleProperty = BindableProperty.Create(nameof(Angle), typeof(int), typeof(SfEffectsView), 0, BindingMode.Default);

	public static readonly BindableProperty FadeOutRippleProperty = BindableProperty.Create(nameof(FadeOutRipple), typeof(bool), typeof(SfEffectsView), false, BindingMode.Default);

	public static readonly BindableProperty AutoResetEffectsProperty = BindableProperty.Create(nameof(AutoResetEffects), typeof(AutoResetEffects), typeof(SfEffectsView), AutoResetEffects.None, BindingMode.Default);

	public static readonly BindableProperty TouchDownEffectsProperty = BindableProperty.Create(nameof(TouchDownEffects), typeof(SfEffects), typeof(SfEffectsView), SfEffects.Ripple, BindingMode.Default);

	public static readonly BindableProperty TouchUpEffectsProperty = BindableProperty.Create(nameof(TouchUpEffects), typeof(SfEffects), typeof(SfEffectsView), SfEffects.None, BindingMode.Default);

	public static readonly BindableProperty LongPressEffectsProperty = BindableProperty.Create(nameof(LongPressEffects), typeof(SfEffects), typeof(SfEffectsView), SfEffects.None, BindingMode.Default);

	public static readonly BindableProperty LongPressedCommandProperty = BindableProperty.Create(nameof(LongPressedCommand), typeof(ICommand), typeof(SfEffectsView), null, BindingMode.Default);

	public static readonly BindableProperty LongPressedCommandParameterProperty = BindableProperty.Create(nameof(LongPressedCommandParameter), typeof(object), typeof(SfEffectsView), null, BindingMode.Default);

	public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(SfEffectsView), false, BindingMode.TwoWay, null, OnIsSelectedPropertyChanged);

	public static readonly BindableProperty ShouldIgnoreTouchesProperty = BindableProperty.Create(nameof(ShouldIgnoreTouches), typeof(bool), typeof(SfEffectsView), false, BindingMode.Default, null, OnShouldIgnorePropertyChanged);

	public static readonly BindableProperty TouchDownCommandProperty = BindableProperty.Create(nameof(TouchDownCommand), typeof(ICommand), typeof(SfEffectsView), null, BindingMode.Default);

	public static readonly BindableProperty TouchUpCommandProperty = BindableProperty.Create(nameof(TouchUpCommand), typeof(ICommand), typeof(SfEffectsView), null, BindingMode.Default);

	public static readonly BindableProperty TouchDownCommandParameterProperty = BindableProperty.Create(nameof(TouchDownCommandParameter), typeof(object), typeof(SfEffectsView), null, BindingMode.Default);

	public static readonly BindableProperty TouchUpCommandParameterProperty = BindableProperty.Create(nameof(TouchUpCommandParameter), typeof(object), typeof(SfEffectsView), null, BindingMode.Default);
    private bool longPressHandled;

	private bool forceReset;

	private HighlightEffectLayer? highlightEffectLayer;

	private SelectionEffectLayer? selectionEffectLayer;

	private RippleEffectLayer? rippleEffectLayer;

	private bool isSelect;

	private bool isSelectedCalled;

	private bool canRepeat;

	private double tempScaleFactor;

	private readonly string rotationAnimation = "Rotation";

	private readonly string scaleAnimation = "Scaling";

	private readonly string highlightAnimation = "Highlight";

	private Microsoft.Maui.Graphics.Point touchDownPoint;

	public double RippleAnimationDuration
	{
		get
		{
			return (double)GetValue(RippleAnimationDurationProperty);
		}
		set
		{
			SetValue(RippleAnimationDurationProperty, value);
		}
	}

	public double ScaleAnimationDuration
	{
		get
		{
			return (double)GetValue(ScaleAnimationDurationProperty);
		}
		set
		{
			SetValue(ScaleAnimationDurationProperty, value);
		}
	}

	public double RotationAnimationDuration
	{
		get
		{
			return (double)GetValue(RotationAnimationDurationProperty);
		}
		set
		{
			SetValue(RotationAnimationDurationProperty, value);
		}
	}

	public double InitialRippleFactor
	{
		get
		{
			return (double)GetValue(InitialRippleFactorProperty);
		}
		set
		{
			SetValue(InitialRippleFactorProperty, value);
		}
	}

	public double ScaleFactor
	{
		get
		{
			return (double)GetValue(ScaleFactorProperty);
		}
		set
		{
			SetValue(ScaleFactorProperty, value);
		}
	}

	public Brush HighlightBackground
	{
		get
		{
			return (Brush)GetValue(HighlightBackgroundProperty);
		}
		set
		{
			SetValue(HighlightBackgroundProperty, value);
		}
	}

	public Brush RippleBackground
	{
		get
		{
			return (Brush)GetValue(RippleBackgroundProperty);
		}
		set
		{
			SetValue(RippleBackgroundProperty, value);
		}
	}

	public Brush SelectionBackground
	{
		get
		{
			return (Brush)GetValue(SelectionBackgroundProperty);
		}
		set
		{
			SetValue(SelectionBackgroundProperty, value);
		}
	}

	public int Angle
	{
		get
		{
			return (int)GetValue(AngleProperty);
		}
		set
		{
			SetValue(AngleProperty, value);
		}
	}

	public bool FadeOutRipple
	{
		get
		{
			return (bool)GetValue(FadeOutRippleProperty);
		}
		set
		{
			SetValue(FadeOutRippleProperty, value);
		}
	}

	public AutoResetEffects AutoResetEffects
	{
		get
		{
			return (AutoResetEffects)GetValue(AutoResetEffectsProperty);
		}
		set
		{
			SetValue(AutoResetEffectsProperty, value);
		}
	}

	public SfEffects TouchDownEffects
	{
		get
		{
			return (SfEffects)GetValue(TouchDownEffectsProperty);
		}
		set
		{
			SetValue(TouchDownEffectsProperty, value);
		}
	}

	public SfEffects TouchUpEffects
	{
		get
		{
			return (SfEffects)GetValue(TouchUpEffectsProperty);
		}
		set
		{
			SetValue(TouchUpEffectsProperty, value);
		}
	}

	public SfEffects LongPressEffects
	{
		get
		{
			return (SfEffects)GetValue(LongPressEffectsProperty);
		}
		set
		{
			SetValue(LongPressEffectsProperty, value);
		}
	}

	public bool IsSelected
	{
		get
		{
			return (bool)GetValue(IsSelectedProperty);
		}
		set
		{
			SetValue(IsSelectedProperty, value);
		}
	}

	public bool ShouldIgnoreTouches
	{
		get
		{
			return (bool)GetValue(ShouldIgnoreTouchesProperty);
		}
		set
		{
			SetValue(ShouldIgnoreTouchesProperty, value);
		}
	}

	public ICommand LongPressedCommand
	{
		get
		{
			return (ICommand)GetValue(LongPressedCommandProperty);
		}
		set
		{
			SetValue(LongPressedCommandProperty, value);
		}
	}

	public ICommand TouchDownCommand
	{
		get
		{
			return (ICommand)GetValue(TouchDownCommandProperty);
		}
		set
		{
			SetValue(TouchDownCommandProperty, value);
		}
	}

	public ICommand TouchUpCommand
	{
		get
		{
			return (ICommand)GetValue(TouchUpCommandProperty);
		}
		set
		{
			SetValue(TouchUpCommandProperty, value);
		}
	}

	public object TouchDownCommandParameter
	{
		get
		{
			return GetValue(TouchDownCommandParameterProperty);
		}
		set
		{
			SetValue(TouchDownCommandParameterProperty, value);
		}
	}

	public object LongPressedCommandParameter
	{
		get
		{
			return GetValue(LongPressedCommandParameterProperty);
		}
		set
		{
			SetValue(LongPressedCommandParameterProperty, value);
		}
	}

	public object TouchUpCommandParameter
	{
		get
		{
			return GetValue(TouchUpCommandParameterProperty);
		}
		set
		{
			SetValue(TouchUpCommandParameterProperty, value);
		}
	}

	internal bool IsSelection
	{
		get
		{
			return isSelect;
		}
		set
		{
			if (isSelect != value)
			{
				isSelect = value;
				if (value)
				{
					selectionEffectLayer?.UpdateSelectionBounds(base.Width, base.Height, SelectionBackground);
				}
				else
				{
					RemoveSelection();
				}
				InvokeSelectionChangedEvent();
			}
		}
	}

	internal bool LongPressHandled
	{
		get
		{
			return longPressHandled;
		}
		set
		{
			longPressHandled = value;
		}
	}

	internal bool ForceReset
	{
		get
		{
			return forceReset;
		}
		set
		{
			forceReset = value;
		}
	}

	public event EventHandler? AnimationCompleted;

	public event EventHandler? SelectionChanged;

	public event EventHandler? TouchDown;

	public event EventHandler? TouchUp;

	public event EventHandler? LongPressed;

	public SfEffectsView()
	{
		InitializeEffects();
		this.AddGestureListener(this);
		this.AddTouchListener(this);
	}

	public void Reset()
	{
		canRepeat = false;
		LongPressHandled = false;
		if (rippleEffectLayer != null)
		{
			rippleEffectLayer.CanRemoveRippleAnimation = this.AnimationIsRunning("RippleAnimator");
			if (!rippleEffectLayer.CanRemoveRippleAnimation || ForceReset)
			{
				rippleEffectLayer.OnRippleAnimationFinished();
			}
		}
		highlightEffectLayer?.UpdateHighlightBounds();
		if (selectionEffectLayer != null && IsSelected)
		{
			IsSelected = false;
			InvokeSelectionChangedEvent();
		}
		if (TouchDownEffects == SfEffects.Scale || TouchUpEffects == SfEffects.Scale || LongPressEffects == SfEffects.Scale)
		{
			if (base.Content != null)
			{
				base.Content.Scale = 1.0;
			}
			OnScaleAnimationEnd(0.0, finished: true);
		}
		if (TouchUpEffects == SfEffects.Rotation || TouchDownEffects == SfEffects.Rotation || LongPressEffects == SfEffects.Rotation)
		{
			if (base.Content != null)
			{
				base.Content.Rotation = 0.0;
			}
			OnRotationAnimationEnd(0.0, finished: true);
		}
	}

	public void OnLongPress(LongPressEventArgs e)
	{
		if (!ShouldIgnoreTouches)
		{
			InvokeLongPressedEventAndCommand();
			LongPressHandled = true;
			if (AutoResetEffects == AutoResetEffects.None && e != null)
			{
				AddEffects(LongPressEffects.ComplementsOf(TouchDownEffects), e.TouchPoint);
			}
		}
	}

	public void ApplyEffects(SfEffects effects = SfEffects.Ripple, RippleStartPosition rippleStartPosition = RippleStartPosition.Default, System.Drawing.Point? rippleStartPoint = null, bool repeat = false)
	{
		if (rippleEffectLayer != null)
		{
			rippleEffectLayer.CanRemoveRippleAnimation = false;
		}
		canRepeat = repeat;
		float num = (float)(base.Width / 2.0);
		float num2 = (float)(base.Height / 2.0);
		if (rippleStartPosition == RippleStartPosition.Left)
		{
			num = 0f;
		}
		if (rippleStartPosition == RippleStartPosition.Top)
		{
			num2 = 0f;
		}
		if (rippleStartPosition == RippleStartPosition.Right)
		{
			num = (float)base.Width;
		}
		if (rippleStartPosition == RippleStartPosition.Bottom)
		{
			num2 = (float)base.Height;
		}
		if (rippleStartPosition == RippleStartPosition.TopLeft)
		{
			num = 0f;
			num2 = 0f;
		}
		if (rippleStartPosition == RippleStartPosition.TopRight)
		{
			num = (float)base.Width;
			num2 = 0f;
		}
		if (rippleStartPosition == RippleStartPosition.BottomLeft)
		{
			num = 0f;
			num2 = (float)base.Height;
		}
		if (rippleStartPosition == RippleStartPosition.BottomRight)
		{
			num = (float)base.Width;
			num2 = (float)base.Height;
		}
		if (rippleStartPosition == RippleStartPosition.Default && rippleStartPoint.HasValue)
		{
			num = rippleStartPoint.Value.X;
			num2 = rippleStartPoint.Value.Y;
		}
		AddEffects(effects, new Microsoft.Maui.Graphics.Point(num, num2));
	}

	public void OnTouch(PointerEventArgs e)
	{
		if (ShouldIgnoreTouches || e == null)
		{
			return;
		}
		if (e.Action == PointerActions.Pressed)
		{
			touchDownPoint = e.TouchPoint;
			LongPressHandled = false;
			if (rippleEffectLayer != null)
			{
				rippleEffectLayer.CanRemoveRippleAnimation = false;
			}
			InvokeTouchDownEventAndCommand();
			if (AutoResetEffects != 0)
			{
				AddResetEffects(AutoResetEffects, e.TouchPoint);
			}
			else
			{
				AddEffects(TouchDownEffects, e.TouchPoint);
			}
		}
		if (e.Action == PointerActions.Released)
		{
			InvokeTouchUpEventAndCommand();
			if (AutoResetEffects.GetAllItems().Contains(AutoResetEffects.Ripple))
			{
				if (rippleEffectLayer != null)
				{
					rippleEffectLayer.CanRemoveRippleAnimation = this.AnimationIsRunning("RippleAnimator");
				}
			}
			else
			{
				if (AutoResetEffects != 0)
				{
					return;
				}
				if (TouchDownEffects == SfEffects.Highlight || TouchDownEffects.GetAllItems().Contains(SfEffects.Highlight))
				{
					highlightEffectLayer?.UpdateHighlightBounds();
					if ((rippleEffectLayer != null && !this.AnimationIsRunning("RippleAnimator")) || (rippleEffectLayer == null && (TouchUpEffects.GetAllItems().Contains(SfEffects.None) || TouchUpEffects.GetAllItems().Contains(SfEffects.Highlight)) && (LongPressEffects.GetAllItems().Contains(SfEffects.None) || !LongPressHandled || LongPressEffects.GetAllItems().Contains(SfEffects.Highlight))))
					{
						InvokeAnimationCompletedEvent();
					}
				}
				if (!IsSelected || (!IsSelected && (TouchDownEffects != SfEffects.Selection || !TouchDownEffects.GetAllItems().Contains(SfEffects.Selection))))
				{
					RemoveSelection();
				}
				if ((TouchDownEffects.GetAllItems().Contains(SfEffects.Ripple) || TouchUpEffects.GetAllItems().Contains(SfEffects.Ripple) || LongPressEffects.GetAllItems().Contains(SfEffects.Ripple)) && rippleEffectLayer != null)
				{
					rippleEffectLayer.CanRemoveRippleAnimation = this.AnimationIsRunning("RippleAnimator");
				}
				if (TouchUpEffects != SfEffects.Highlight || !TouchUpEffects.GetAllItems().Contains(SfEffects.Highlight))
				{
					RemoveHighlightEffect();
				}
				else
				{
					this.Animate(highlightAnimation, OnHighlightAnimationUpdate, 16u, 250u, Easing.Linear, OnAnimationFinished);
				}
				if ((TouchUpEffects != SfEffects.Ripple || !TouchUpEffects.GetAllItems().Contains(SfEffects.Ripple)) && rippleEffectLayer != null && !this.AnimationIsRunning("RippleAnimator"))
				{
					RemoveRippleEffect();
				}
			}
		}
		else if (e.Action == PointerActions.Cancelled)
		{
			LongPressHandled = false;
			RemoveRippleEffect();
			RemoveHighlightEffect();
			RemoveSelection();
		}
		else if (e.Action == PointerActions.Moved)
		{
			double num = Math.Abs(touchDownPoint.X - e.TouchPoint.X);
			double num2 = Math.Abs(touchDownPoint.Y - e.TouchPoint.Y);
			if (num >= 20.0 || num2 >= 20.0)
			{
				RemoveRippleEffect();
				RemoveHighlightEffect();
			}
		}
	}

	public void OnTap(TapEventArgs e)
	{
		if (!ShouldIgnoreTouches)
		{
			LongPressHandled = false;
			if (TouchUpEffects != SfEffects.None && e != null)
			{
				AddEffects(TouchUpEffects, e.TapPoint);
			}
			if (TouchUpEffects.GetAllItems().Contains(SfEffects.Scale))
			{
				StartScaleAnimation();
			}
		}
	}

	internal void RaiseAnimationCompletedEvent(EventArgs eventArgs)
	{
		this.AnimationCompleted?.Invoke(this, eventArgs);
	}

	internal void RaiseSelectedEvent(EventArgs eventArgs)
	{
		this.SelectionChanged?.Invoke(this, eventArgs);
	}

	internal void InvokeSelectionChangedEvent()
	{
		RaiseSelectedEvent(EventArgs.Empty);
	}

	internal void InvokeLongPressedEventAndCommand()
	{
		this.LongPressed?.Invoke(this, EventArgs.Empty);
		if (LongPressedCommand != null && LongPressedCommand.CanExecute(LongPressedCommandParameter))
		{
			LongPressedCommand.Execute(LongPressedCommandParameter);
		}
	}

	internal void InvokeTouchDownEventAndCommand()
	{
		this.TouchDown?.Invoke(this, EventArgs.Empty);
		if (TouchDownCommand != null && TouchDownCommand.CanExecute(TouchDownCommandParameter))
		{
			TouchDownCommand.Execute(TouchDownCommandParameter);
		}
	}

	internal void InvokeAnimationCompletedEvent()
	{
		RaiseAnimationCompletedEvent(EventArgs.Empty);
	}

	internal void InvokeTouchUpEventAndCommand()
	{
		this.TouchUp?.Invoke(this, EventArgs.Empty);
		if (TouchUpCommand != null && TouchUpCommand.CanExecute(TouchUpCommandParameter))
		{
			TouchUpCommand.Execute(TouchUpCommandParameter);
		}
	}

	protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
	{
		base.OnDraw(canvas, dirtyRect);
		highlightEffectLayer?.DrawHighlight(canvas);
		rippleEffectLayer?.DrawRipple(canvas, dirtyRect);
		selectionEffectLayer?.DrawSelection(canvas);
	}

	protected override Microsoft.Maui.Graphics.Size ArrangeContent(Rect bounds)
	{
		if (bounds.Width > 0.0 && bounds.Height > 0.0)
		{
			if (highlightEffectLayer != null)
			{
				highlightEffectLayer.Width = bounds.Width;
				highlightEffectLayer.Height = bounds.Height;
			}
			if (rippleEffectLayer != null)
			{
				rippleEffectLayer.Width = bounds.Width;
				rippleEffectLayer.Height = bounds.Height;
			}
			if (selectionEffectLayer != null)
			{
				selectionEffectLayer.Width = bounds.Width;
				selectionEffectLayer.Height = bounds.Height;
				if (IsSelected)
				{
					selectionEffectLayer?.UpdateSelectionBounds(bounds.Width, bounds.Height, SelectionBackground);
					if (!isSelectedCalled)
					{
						InvokeSelectionChangedEvent();
						isSelectedCalled = true;
					}
				}
			}
		}
		return base.ArrangeContent(bounds);
	}

	private static void OnSelectionBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable != null && bindable is SfEffectsView && newValue != null)
		{
			(bindable as SfEffectsView)?.UpdateSelectionBackground((Brush)newValue);
		}
	}

	private static void OnIsSelectedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable != null && bindable is SfEffectsView && newValue != null && bindable is SfEffectsView sfEffectsView)
		{
			sfEffectsView.IsSelection = (bool)newValue;
		}
	}

	private static void OnShouldIgnorePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable != null && bindable is SfEffectsView && newValue != null && bindable is SfEffectsView sfEffectsView)
		{
			sfEffectsView.RemoveGestureListener(sfEffectsView);
			sfEffectsView.RemoveTouchListener(sfEffectsView);
			if (!(bool)newValue)
			{
				sfEffectsView.AddGestureListener(sfEffectsView);
				sfEffectsView.AddTouchListener(sfEffectsView);
			}
		}
	}

	private void UpdateSelectionBackground(Brush selectionBackground)
	{
		if (IsSelected)
		{
			selectionEffectLayer?.UpdateSelectionBounds(base.Width, base.Height, selectionBackground);
		}
	}

	private void RemoveSelection()
	{
		selectionEffectLayer?.UpdateSelectionBounds();
	}

	private void RemoveHighlightEffect()
	{
		highlightEffectLayer?.UpdateHighlightBounds();
	}

	private void AddResetEffects(AutoResetEffects effects, Microsoft.Maui.Graphics.Point touchPoint)
	{
		foreach (AutoResetEffects allItem in effects.GetAllItems())
		{
			if (allItem != 0)
			{
				if (allItem == AutoResetEffects.Highlight)
				{
					highlightEffectLayer?.UpdateHighlightBounds(base.Width, base.Height, HighlightBackground);
					this.Animate(highlightAnimation, OnHighlightAnimationUpdate, 16u, 250u, Easing.Linear, OnAnimationFinished);
				}
				if (allItem == AutoResetEffects.Ripple)
				{
					rippleEffectLayer?.StartRippleAnimation(touchPoint, RippleBackground, RippleAnimationDuration, (float)InitialRippleFactor, FadeOutRipple);
				}
				if (allItem == AutoResetEffects.Scale)
				{
					StartScaleAnimation();
				}
			}
		}
	}

	private void AddEffects(SfEffects sfEffect, Microsoft.Maui.Graphics.Point touchPoint)
	{
		foreach (SfEffects allItem in sfEffect.GetAllItems())
		{
			if (allItem == SfEffects.None)
			{
				continue;
			}
			if (allItem == SfEffects.Highlight)
			{
				highlightEffectLayer?.UpdateHighlightBounds(base.Width, base.Height, HighlightBackground);
			}
			if (allItem == SfEffects.Ripple)
			{
				rippleEffectLayer?.StartRippleAnimation(touchPoint, RippleBackground, RippleAnimationDuration, (float)InitialRippleFactor, FadeOutRipple, canRepeat);
			}
			if (allItem == SfEffects.Selection)
			{
				selectionEffectLayer?.UpdateSelectionBounds(base.Width, base.Height, SelectionBackground);
				if (!IsSelected)
				{
					IsSelected = true;
				}
			}
			if (allItem == SfEffects.Scale)
			{
				StartScaleAnimation();
			}
			if (allItem == SfEffects.Rotation)
			{
				StartRotationAnimation();
			}
		}
	}

	private void RemoveRippleEffect()
	{
		rippleEffectLayer?.OnRippleAnimationFinished();
	}

	private void InitializeEffects()
	{
		rippleEffectLayer ??= new RippleEffectLayer(RippleBackground, RippleAnimationDuration, this, this);
		selectionEffectLayer ??= new SelectionEffectLayer(SelectionBackground, this);
		highlightEffectLayer ??= new HighlightEffectLayer(HighlightBackground, this);
		base.DrawingOrder = DrawingOrder.AboveContent;
	}

	private void StartScaleAnimation()
	{
		if (base.Content != null && tempScaleFactor != ScaleFactor)
		{
			base.Content.AnchorX = 0.5005000233650208;
			base.Content.AnchorY = 0.5005000233650208;
			tempScaleFactor = ScaleFactor;
			base.Content.Animate(scaleAnimation, OnScaleAnimationUpdate, base.Content.Scale, ScaleFactor, 16u, (uint)ScaleAnimationDuration, Easing.Linear, OnScaleAnimationEnd);
		}
	}

	private void StartRotationAnimation()
	{
		if (base.Content != null)
		{
			base.Content.AnchorX = 0.5005000233650208;
			base.Content.AnchorY = 0.5005000233650208;
			if (DeviceInfo.Platform == DevicePlatform.WinUI && (((IVisualElementController)this).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
			{
				base.Content.Rotation = 0.0 - base.Content.Rotation;
				Angle = -Angle;
			}
			base.Content.Animate(rotationAnimation, OnAnimationUpdate, base.Content.Rotation, Angle, 16u, (uint)RotationAnimationDuration, Easing.Linear, OnRotationAnimationEnd);
		}
	}

	private void OnRotationAnimationEnd(double value, bool finished)
	{
		this.AbortAnimation(rotationAnimation);
		if ((rippleEffectLayer != null && !this.AnimationIsRunning("RippleAnimator")) || (rippleEffectLayer == null && (TouchUpEffects.GetAllItems().Contains(SfEffects.None) || TouchUpEffects.GetAllItems().Contains(SfEffects.Rotation)) && (LongPressEffects.GetAllItems().Contains(SfEffects.None) || !LongPressHandled || LongPressEffects.GetAllItems().Contains(SfEffects.Rotation))))
		{
			InvokeAnimationCompletedEvent();
		}
	}

	private void OnAnimationUpdate(double value)
	{
		if (base.Content != null)
		{
			base.Content.Rotation = value;
		}
	}

	private void OnScaleAnimationEnd(double value, bool finished)
	{
		this.AbortAnimation(scaleAnimation);
		if ((rippleEffectLayer != null && !this.AnimationIsRunning("RippleAnimator")) || (rippleEffectLayer == null && (TouchUpEffects.GetAllItems().Contains(SfEffects.None) || TouchUpEffects.GetAllItems().Contains(SfEffects.Scale)) && (LongPressEffects.GetAllItems().Contains(SfEffects.None) || !LongPressHandled || LongPressEffects.GetAllItems().Contains(SfEffects.Scale))))
		{
			InvokeAnimationCompletedEvent();
		}
	}

	private void OnScaleAnimationUpdate(double value)
	{
		if (base.Content != null)
		{
			base.Content.Scale = value;
		}
	}

	private void OnHighlightAnimationUpdate(double value)
	{
	}

	private void OnAnimationFinished(double value, bool completed)
	{
		highlightEffectLayer?.UpdateHighlightBounds();
		if ((rippleEffectLayer != null && !this.AnimationIsRunning("RippleAnimator")) || (rippleEffectLayer == null && (TouchUpEffects.GetAllItems().Contains(SfEffects.None) || TouchUpEffects.GetAllItems().Contains(SfEffects.Highlight)) && (LongPressEffects.GetAllItems().Contains(SfEffects.None) || !LongPressHandled || LongPressEffects.GetAllItems().Contains(SfEffects.Highlight))))
		{
			InvokeAnimationCompletedEvent();
		}
	}
}
