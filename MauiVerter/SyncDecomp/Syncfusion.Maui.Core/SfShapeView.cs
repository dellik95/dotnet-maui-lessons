using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Core;

internal class SfShapeView : SfDrawableView
{
	public static readonly BindableProperty ShapeTypeProperty = BindableProperty.Create(nameof(ShapeType), typeof(ShapeType), typeof(SfShapeView), ShapeType.Circle, BindingMode.Default, null, OnPropertyChanged);

	public static readonly BindableProperty IconBrushProperty = BindableProperty.Create(nameof(IconBrush), typeof(Brush), typeof(SfShapeView), null, BindingMode.Default, null, OnPropertyChanged);

	public static readonly BindableProperty StrokeProperty = BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(SfShapeView), null, BindingMode.Default, null, OnPropertyChanged);

	public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(float), typeof(SfShapeView), 1f, BindingMode.Default, null, OnPropertyChanged);

	public ShapeType ShapeType
	{
		get
		{
			return (ShapeType)GetValue(ShapeTypeProperty);
		}
		set
		{
			SetValue(ShapeTypeProperty, value);
		}
	}

	public Brush IconBrush
	{
		get
		{
			return (Brush)GetValue(IconBrushProperty);
		}
		set
		{
			SetValue(IconBrushProperty, value);
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

	public float StrokeWidth
	{
		get
		{
			return (float)GetValue(StrokeWidthProperty);
		}
		set
		{
			SetValue(StrokeWidthProperty, value);
		}
	}

	protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
	{
		if (dirtyRect.Width > 0f && dirtyRect.Height > 0f && ShapeType != 0)
		{
			Rect rect = new Rect(0.0, 0.0, dirtyRect.Height, dirtyRect.Width);
			DrawShape(canvas, rect, ShapeType, IconBrush, StrokeWidth, IconBrush);
		}
	}

	public virtual void DrawShape(ICanvas canvas, Rect rect, ShapeType shapeType, Brush strokeColor, float strokeWidth, Brush fillColor)
	{
		canvas.StrokeColor = (strokeColor as SolidColorBrush)?.Color;
		canvas.StrokeSize = strokeWidth;
		canvas.SetFillPaint(fillColor, rect);
		DrawShape(canvas, rect, shapeType, fillColor, strokeWidth > 0f, isSaveState: true);
	}

	public static void DrawShape(ICanvas canvas, RectF rect, ShapeType shapeType, Brush fillColor, bool hasBorder, bool isSaveState)
	{
		float x = rect.X;
		float y = rect.Y;
		float radius = Math.Min(rect.Width, rect.Height) / 2f;
		float x2 = x + rect.Width;
		float y2 = y + rect.Height;
		float x3 = x + rect.Width / 2f;
		float y3 = y + rect.Height / 2f;
		PointF center = rect.Center;
		switch (shapeType)
		{
		case ShapeType.Rectangle:
			DrawRectangle(canvas, rect, fillColor, hasBorder, isSaveState);
			break;
		case ShapeType.Circle:
			DrawCircle(canvas, rect, center, radius, fillColor, hasBorder, isSaveState);
			break;
		case ShapeType.HorizontalLine:
			DrawLine(canvas, new PointF(x, y3), new PointF(x2, y3), isSaveState);
			break;
		case ShapeType.VerticalLine:
			DrawLine(canvas, new PointF(x3, y), new PointF(x3, y2), isSaveState);
			break;
		case ShapeType.Triangle:
			canvas.DrawTriangle(rect, hasBorder);
			break;
		case ShapeType.InvertedTriangle:
			canvas.DrawInverseTriangle(rect, hasBorder);
			break;
		case ShapeType.Cross:
			canvas.DrawCross(rect, hasBorder);
			break;
		case ShapeType.Plus:
			canvas.DrawPlus(rect, hasBorder);
			break;
		case ShapeType.Diamond:
			canvas.DrawDiamond(rect, hasBorder);
			break;
		case ShapeType.Hexagon:
			canvas.DrawHexagon(rect, hasBorder);
			break;
		case ShapeType.Pentagon:
			canvas.DrawPentagon(rect, hasBorder);
			break;
		}
	}

	private static void DrawLine(ICanvas canvas, PointF start, PointF end, bool isSaveState)
	{
		if (isSaveState)
		{
			canvas.SaveState();
		}
		canvas.DrawLine(start, end);
		if (isSaveState)
		{
			canvas.RestoreState();
		}
	}

	private static void DrawHorizontalLine(ICanvas canvas, RectF rect, bool hasBorder, bool isSaveState)
	{
		float x = rect.X;
		float y = rect.Y;
		float x2 = x + rect.Width;
		float num = y + rect.Height;
		float num2 = x + rect.Width / 2f;
		float num3 = y + rect.Height / 2f;
		float num4 = rect.Width / 5f;
		float num5 = rect.Height / 5f;
		if (isSaveState)
		{
			canvas.SaveState();
		}
		PathF pathF = new PathF();
		pathF.MoveTo(x, num3 - num5);
		pathF.LineTo(x2, num3 - num5);
		pathF.LineTo(x2, num3 + num5);
		pathF.LineTo(x, num3 + num5);
		pathF.LineTo(x, num3 - num5);
		pathF.Close();
		canvas.FillPath(pathF);
		if (hasBorder)
		{
			canvas.DrawPath(pathF);
		}
		if (isSaveState)
		{
			canvas.RestoreState();
		}
	}

	private static void DrawVerticalLine(ICanvas canvas, RectF rect, bool hasBorder, bool isSaveState)
	{
		float x = rect.X;
		float y = rect.Y;
		float num = x + rect.Width;
		float y2 = y + rect.Height;
		float num2 = x + rect.Width / 2f;
		float num3 = y + rect.Height / 2f;
		float num4 = rect.Width / 5f;
		float num5 = rect.Height / 5f;
		if (isSaveState)
		{
			canvas.SaveState();
		}
		PathF pathF = new PathF();
		pathF.MoveTo(num2 + num4, y);
		pathF.LineTo(num2 + num4, y2);
		pathF.LineTo(num2 - num4, y2);
		pathF.LineTo(num2 - num4, x);
		pathF.LineTo(num2 + num4, x);
		pathF.Close();
		canvas.FillPath(pathF);
		if (hasBorder)
		{
			canvas.DrawPath(pathF);
		}
		if (isSaveState)
		{
			canvas.RestoreState();
		}
	}

	private static void DrawCircle(ICanvas canvas, RectF rect, PointF center, float radius, Brush fillColor, bool hasBorder, bool isSaveState)
	{
		if (isSaveState)
		{
			canvas.SaveState();
		}
		canvas.SetFillPaint(fillColor, rect);
		canvas.FillCircle(center, radius);
		if (hasBorder)
		{
			canvas.DrawCircle(center, radius);
		}
		if (isSaveState)
		{
			canvas.RestoreState();
		}
	}

	private static void DrawRectangle(ICanvas canvas, RectF rect, Brush fillColor, bool hasBorder, bool isSaveState)
	{
		if (isSaveState)
		{
			canvas.SaveState();
		}
		canvas.SetFillPaint(fillColor, rect);
		canvas.FillRectangle(rect);
		if (hasBorder)
		{
			canvas.DrawRectangle(rect);
		}
		if (isSaveState)
		{
			canvas.RestoreState();
		}
	}

	private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfShapeView sfShapeView)
		{
			sfShapeView.InvalidateDrawable();
		}
	}

	private void DrawlineWithMarker(ShapeType iconType)
	{
	}
}
