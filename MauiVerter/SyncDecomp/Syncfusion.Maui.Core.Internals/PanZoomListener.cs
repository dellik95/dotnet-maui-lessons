using System;
using System.ComponentModel;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals;

public class PanZoomListener : IPinchGestureListener, IGestureListener, IPanGestureListener, IDoubleTapGestureListener, ITouchListener, IKeyboardListener, INotifyPropertyChanged
{
	private bool m_stopScroll = false;

	private double m_currentZoomFactor = 1.0;

	private double m_minZoomFactor = 0.1;

	private double m_maxZoomFactor = 10.0;

	private bool m_allowPinchZoom = true;

	private bool m_allowDoubleTapZoom = true;

	private bool m_allowMouseWheelZoom = true;

	private PanMode m_panMode = PanMode.Both;

	private MouseWheelSettings? m_mouseWheelSettings;

	private DoubleTapSettings? m_doubleTapSettings;

	internal KeyboardKey m_currentKeyModifier = KeyboardKey.None;

	public double CurrentZoomFactor
	{
		get
		{
			return m_currentZoomFactor;
		}
		set
		{
			m_currentZoomFactor = value;
			OnPropertyChanged(nameof(CurrentZoomFactor));
		}
	}

	public MouseWheelSettings? MouseWheelSettings => m_mouseWheelSettings;

	public DoubleTapSettings? DoubleTapSettings => m_doubleTapSettings;

	public double MinZoomFactor
	{
		get
		{
			return m_minZoomFactor;
		}
		set
		{
			m_minZoomFactor = value;
			OnPropertyChanged(nameof(MinZoomFactor));
		}
	}

	public double MaxZoomFactor
	{
		get
		{
			return m_maxZoomFactor;
		}
		set
		{
			m_maxZoomFactor = value;
			OnPropertyChanged(nameof(MaxZoomFactor));
		}
	}

	public PanMode PanMode
	{
		get
		{
			return m_panMode;
		}
		set
		{
			m_panMode = value;
			OnPropertyChanged(nameof(PanMode));
		}
	}

	public bool AllowDoubleTapZoom
	{
		get
		{
			return m_allowDoubleTapZoom;
		}
		set
		{
			m_allowDoubleTapZoom = value;
			OnPropertyChanged(nameof(AllowDoubleTapZoom));
		}
	}

	public bool AllowPinchZoom
	{
		get
		{
			return m_allowPinchZoom;
		}
		set
		{
			m_allowPinchZoom = value;
			OnPropertyChanged(nameof(AllowPinchZoom));
		}
	}

	public bool AllowMouseWheelZoom
	{
		get
		{
			return m_allowMouseWheelZoom;
		}
		set
		{
			m_allowMouseWheelZoom = value;
			OnPropertyChanged(nameof(AllowMouseWheelZoom));
		}
	}

	bool IGestureListener.IsTouchHandled => m_stopScroll;

	public event EventHandler<ZoomEventArgs>? ZoomChanged;

	public event EventHandler<ZoomEventArgs>? ZoomStarted;

	public event EventHandler<ZoomEventArgs>? ZoomEnded;

	public event EventHandler<PanEventArgs>? PanUpdated;

	public event PropertyChangedEventHandler? PropertyChanged;

	public PanZoomListener()
	{
		m_mouseWheelSettings = new MouseWheelSettings();
		m_doubleTapSettings = new DoubleTapSettings();
	}

	void IDoubleTapGestureListener.OnDoubleTap(TapEventArgs e)
	{
		if (AllowDoubleTapZoom && (this.ZoomStarted != null || this.ZoomChanged != null || this.ZoomEnded != null))
		{
			double zoomFactor = ((CurrentZoomFactor != DoubleTapSettings.DefaultZoomFactor) ? DoubleTapSettings.DefaultZoomFactor : (DoubleTapSettings.DefaultZoomFactor * ((100.0 + DoubleTapSettings.ZoomDeltaPercent) / 100.0)));
			ZoomTo(zoomFactor, e.TapPoint);
		}
	}

	public virtual void OnPinch(PinchEventArgs e)
	{
		if (AllowPinchZoom && (this.ZoomStarted != null || this.ZoomChanged != null || this.ZoomEnded != null))
		{
			switch (e.Status)
			{
			case GestureStatus.Started:
				BeginZoom(e.TouchPoint, e.Angle);
				break;
			case GestureStatus.Canceled:
				m_stopScroll = false;
				break;
			case GestureStatus.Running:
				OnZoom(CurrentZoomFactor * (double)e.Scale, e.TouchPoint, e.Angle);
				break;
			case GestureStatus.Completed:
				EndZoom(e.TouchPoint, e.Angle);
				break;
			}
		}
	}

	void ITouchListener.OnScrollWheel(ScrollEventArgs e)
	{
		if (AllowMouseWheelZoom && e.ScrollDelta != 0.0 && (this.ZoomStarted != null || this.ZoomChanged != null || this.ZoomEnded != null) && m_currentKeyModifier == MouseWheelSettings.ZoomKeyModifier)
		{
			double zoomFactor = ((e.ScrollDelta > 0.0) ? (CurrentZoomFactor * ((100.0 + MouseWheelSettings.ZoomDeltaPercent) / 100.0)) : (CurrentZoomFactor * ((100.0 - MouseWheelSettings.ZoomDeltaPercent) / 100.0)));
			ZoomTo(zoomFactor, e.TouchPoint);
			e.Handled = true;
		}
	}

	void IPanGestureListener.OnPan(PanEventArgs e)
	{
		if (PanMode != 0)
		{
			switch (e.Status)
			{
			case GestureStatus.Started:
				m_stopScroll = true;
				break;
			case GestureStatus.Completed:
			case GestureStatus.Canceled:
				m_stopScroll = false;
				break;
			}
			OnPanUpdated(e);
		}
	}

	void ITouchListener.OnTouch(PointerEventArgs e)
	{
	}

	void IKeyboardListener.OnKeyUp(KeyEventArgs args)
	{
		UpdateModifierKey(args);
	}

	void IKeyboardListener.OnKeyDown(KeyEventArgs args)
	{
		UpdateModifierKey(args);
	}

	public void ZoomTo(double zoomFactor, Point? zoomOrigin = null)
	{
		if (!zoomOrigin.HasValue)
		{
			zoomOrigin = Point.Zero;
		}
		BeginZoom(zoomOrigin.Value);
		OnZoom(zoomFactor, zoomOrigin.Value);
		EndZoom(zoomOrigin.Value);
	}

	protected bool IsZoomingEnabled()
	{
		return this.ZoomStarted != null || this.ZoomChanged != null || this.ZoomEnded != null;
	}

	private void BeginZoom(Point zoomOrigin, double? angle = null)
	{
		m_stopScroll = true;
		if (this.ZoomStarted != null)
		{
			ZoomEventArgs zoomEventArgs = new ZoomEventArgs(CurrentZoomFactor, zoomOrigin);
			zoomEventArgs.Angle = angle;
			this.ZoomStarted(this, zoomEventArgs);
		}
	}

	protected void OnZoom(double zoomFactor, Point zoomOrigin, double? angle = null)
	{
		double currentZoomFactor = CurrentZoomFactor;
		CurrentZoomFactor = Math.Clamp(zoomFactor, MinZoomFactor, MaxZoomFactor);
		if (this.ZoomChanged != null && currentZoomFactor != CurrentZoomFactor)
		{
			ZoomEventArgs zoomEventArgs = new ZoomEventArgs(CurrentZoomFactor, zoomOrigin);
			zoomEventArgs.Angle = angle;
			this.ZoomChanged(this, zoomEventArgs);
		}
	}

	private void EndZoom(Point zoomOrigin, double? angle = null)
	{
		if (this.ZoomEnded != null)
		{
			ZoomEventArgs zoomEventArgs = new ZoomEventArgs(CurrentZoomFactor, zoomOrigin);
			zoomEventArgs.Angle = angle;
			this.ZoomEnded(this, zoomEventArgs);
		}
		m_stopScroll = false;
	}

	private void OnPanUpdated(PanEventArgs panEventArgs)
	{
		if (this.PanUpdated == null)
		{
			return;
		}
		if (PanMode == PanMode.Both)
		{
			this.PanUpdated(this, panEventArgs);
			return;
		}
		Point zero = Point.Zero;
		switch (PanMode)
		{
		case PanMode.Vertical:
			zero.Y = panEventArgs.TranslatePoint.Y;
			break;
		case PanMode.Horizontal:
			zero.X = panEventArgs.TranslatePoint.X;
			break;
		}
		PanEventArgs e = new PanEventArgs(panEventArgs.Status, panEventArgs.TouchPoint, zero, panEventArgs.Velocity);
		this.PanUpdated(this, e);
	}

	private void UpdateModifierKey(KeyEventArgs args)
	{
		if (args.IsCtrlKeyPressed)
		{
			m_currentKeyModifier = KeyboardKey.Ctrl;
		}
		else if (args.IsAltKeyPressed)
		{
			m_currentKeyModifier = KeyboardKey.Alt;
		}
		else if (args.IsShiftKeyPressed)
		{
			m_currentKeyModifier = KeyboardKey.Shift;
		}
		else if (args.IsCommandKeyPressed)
		{
			m_currentKeyModifier = KeyboardKey.Command;
		}
		else
		{
			m_currentKeyModifier = KeyboardKey.None;
		}
	}

	private void OnPropertyChanged(string propertyName = "")
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
