using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Core;

internal class MaterialToolTipRenderer : ITextElement
{
	private Point nosePoint = Point.Zero;

	private float noseOffset = 2f;

	private float noseHeight = 0f;

	private float noseWidth = 4f;

	private Microsoft.Maui.Font font = Microsoft.Maui.Font.Default;

	private double fontSize = 14.0;

	private string fontFamily = string.Empty;

	private FontAttributes fontAttributes = FontAttributes.None;

	private Color textColor = Colors.White;

	public Microsoft.Maui.Font Font
	{
		get
		{
			return font;
		}
		set
		{
			if (font != value)
			{
				font = value;
			}
		}
	}

	public double FontSize
	{
		get
		{
			return fontSize;
		}
		set
		{
			if (fontSize != value)
			{
				fontSize = value;
			}
			Font = font.WithSize(fontSize);
		}
	}

	public string FontFamily
	{
		get
		{
			return fontFamily;
		}
		set
		{
			if (fontFamily != value)
			{
				fontFamily = value;
			}
			Font = Microsoft.Maui.Font.OfSize(fontFamily, fontSize, FontWeight.Regular, FontSlant.Default, enableScaling: false).WithAttributes(fontAttributes);
		}
	}

	public FontAttributes FontAttributes
	{
		get
		{
			return fontAttributes;
		}
		set
		{
			if (fontAttributes != value)
			{
				fontAttributes = value;
			}
			Font = font.WithAttributes(fontAttributes);
		}
	}

	public Color TextColor
	{
		get
		{
			return textColor;
		}
		set
		{
			if (textColor != value)
			{
				textColor = value;
			}
		}
	}

	internal TooltipPosition Position { get; set; } = TooltipPosition.Auto;


	internal Thickness Padding { get; set; } = new Thickness(15.0, 8.0);


	internal float CornerRadius { get; set; } = 3f;


	internal Size ContentSize { get; set; }

	void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
	{
	}

	void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
	{
	}

	double ITextElement.FontSizeDefaultValueCreator()
	{
		return 14.0;
	}

	void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
	{
	}

	void ITextElement.OnFontChanged(Microsoft.Maui.Font oldValue, Microsoft.Maui.Font newValue)
	{
	}

	internal void Show(ICanvas canvas, Rect containerRect, Rect[] targetRects, string[] texts, double[] animationValue, Brush fill, Brush stroke, double strokeSize)
	{
		Rect[] toolTipRects = GetToolTipRects(containerRect, targetRects, texts);
		bool flag = false;
		if (toolTipRects.Length == 2)
		{
			flag = toolTipRects[1].IntersectsWith(toolTipRects[0]);
		}
		for (int i = 0; i < toolTipRects.Length; i++)
		{
			SetNosePoint(Position, targetRects[i], toolTipRects[i]);
			PathGeometry clipPathGeometry = GetClipPathGeometry(GetGeometryPoints(Position, toolTipRects[i]), Position);
			PathF pathF = new PathF();
			Size size = new Size(toolTipRects[i].Width, toolTipRects[i].Height);
			Rect rect;
			if (Position == TooltipPosition.Right)
			{
				Point translatePoint = new Point(0.0, ((LineSegment)clipPathGeometry.Figures[0].Segments[7]).Point.Y);
				rect = new Rect((double)(noseOffset + noseHeight) * animationValue[i] - (size.Width - (double)(noseOffset + noseHeight)) / 2.0 * (1.0 - animationValue[i]), 0.0 - translatePoint.Y, size.Width - (double)(noseOffset + noseHeight), size.Height);
				AppendPath(pathF, clipPathGeometry.Figures, translatePoint);
			}
			else if (Position == TooltipPosition.Left)
			{
				Point translatePoint2 = new Point(((LineSegment)clipPathGeometry.Figures[0].Segments[3]).Point.X + (double)noseOffset, ((LineSegment)clipPathGeometry.Figures[0].Segments[3]).Point.Y);
				rect = new Rect((0.0 - translatePoint2.X) * animationValue[i] - (size.Width - (double)(noseOffset + noseHeight)) / 2.0 * (1.0 - animationValue[i]), 0.0 - translatePoint2.Y, size.Width - (double)(noseOffset + noseHeight), size.Height);
				AppendPath(pathF, clipPathGeometry.Figures, translatePoint2);
			}
			else
			{
				Point translatePoint3 = new Point(((LineSegment)clipPathGeometry.Figures[0].Segments[5]).Point.X, toolTipRects[i].Height);
				rect = new Rect(0.0 - translatePoint3.X, (0.0 - size.Height) * animationValue[i] - (size.Height - (double)(noseOffset + noseHeight)) / 2.0 * (1.0 - animationValue[i]), size.Width, size.Height - (double)(noseOffset + noseHeight));
				AppendPath(pathF, clipPathGeometry.Figures, translatePoint3);
			}
			canvas.SaveState();
			canvas.Translate((float)targetRects[i].X, (float)targetRects[i].Y);
			canvas.Scale((float)animationValue[i], (float)animationValue[i]);
			if (PaintExtensions.ToColor(fill) != Colors.Transparent)
			{
				canvas.FillColor = Colors.Transparent;
				canvas.SetFillPaint(fill, pathF.Bounds);
				canvas.StrokeColor = Colors.Transparent;
				canvas.FillPath(pathF);
				canvas.DrawPath(pathF);
			}
			Paint paint = stroke;
			if (paint.ToColor() != Colors.Transparent)
			{
				canvas.FillColor = Colors.Transparent;
				canvas.StrokeColor = paint.ToColor();
				canvas.StrokeSize = (float)strokeSize;
				canvas.FillPath(pathF);
				canvas.DrawPath(pathF);
			}
			if (flag && paint.ToColor() == Colors.Transparent)
			{
				canvas.FillColor = Colors.Transparent;
				canvas.StrokeColor = Colors.White;
				canvas.StrokeSize = 0.5f;
				canvas.FillPath(pathF);
				canvas.DrawPath(pathF);
			}
			canvas.RestoreState();
			if (animationValue[i] == 1.0)
			{
				canvas.DrawText(texts[i], new Rect(rect.X + (double)(float)targetRects[i].X, rect.Y + (double)(float)targetRects[i].Y, rect.Width, rect.Height), HorizontalAlignment.Center, VerticalAlignment.Center, this);
			}
			else if (animationValue[i] > 0.0)
			{
				TooltipFont tooltipFont = new TooltipFont(this);
				tooltipFont.FontSize *= animationValue[i];
				canvas.DrawText(texts[i], new Rect(rect.X + (double)(float)targetRects[i].X, rect.Y + (double)(float)targetRects[i].Y, rect.Width, rect.Height), HorizontalAlignment.Center, VerticalAlignment.Center, tooltipFont);
			}
		}
	}

	internal void UpdateDefaultVisual(float noseHeight, float noseWidth, float noseOffset, float cornerRadius)
	{
		this.noseHeight = noseHeight;
		this.noseWidth = noseWidth;
		this.noseOffset = noseOffset;
		CornerRadius = cornerRadius;
	}

	internal Rect[] GetToolTipRects(Rect containerRect, Rect[] targetRects, string[] texts)
	{
		int num = texts.Length;
		Rect[] array = new Rect[num];
		for (int i = 0; i < num; i++)
		{
			string text = texts[i];
			if (text == null)
			{
				break;
			}
			Size size = text.Measure((float)fontSize, fontAttributes, fontFamily);
			ContentSize = new Size(size.Width + Padding.HorizontalThickness, size.Height / 2.0 + Padding.VerticalThickness);
			array[i] = GetToolTipRect(Position, targetRects[i], containerRect);
		}
		return array;
	}

	internal void SetNosePoint(TooltipPosition position, Rect targetRect, Rect toolTipRect)
	{
		double num = 0.0;
		switch (position)
		{
		case TooltipPosition.Auto:
		case TooltipPosition.Top:
		case TooltipPosition.Bottom:
			num = ((!(toolTipRect.Width < targetRect.Width)) ? (Math.Abs(toolTipRect.X - targetRect.X) + targetRect.Width / 2.0) : (ContentSize.Width / 2.0));
			nosePoint = new Point(num, (position == TooltipPosition.Auto || position == TooltipPosition.Top) ? (toolTipRect.Height - (double)noseOffset) : ((double)noseOffset));
			break;
		case TooltipPosition.Left:
		case TooltipPosition.Right:
			nosePoint = new Point(y: (!(toolTipRect.Height < targetRect.Height)) ? (Math.Abs(toolTipRect.Y - targetRect.Y) + targetRect.Height / 2.0) : (ContentSize.Height / 2.0), x: (position == TooltipPosition.Right) ? ((double)noseOffset) : (toolTipRect.Width - (double)noseOffset));
			break;
		}
	}

	internal Rect GetToolTipRect(TooltipPosition position, Rect targetRect, Rect containerRect)
	{
		double x = 0.0;
		double y = 0.0;
		double num = ContentSize.Width;
		double num2 = ContentSize.Height;
		switch (position)
		{
		case TooltipPosition.Auto:
		case TooltipPosition.Top:
			x = targetRect.Center.X - num / 2.0;
			y = targetRect.Y - num2 - (double)noseOffset - (double)noseHeight;
			num2 += (double)(noseOffset + noseHeight);
			break;
		case TooltipPosition.Bottom:
			x = targetRect.Center.X - num / 2.0;
			y = targetRect.Bottom;
			num2 += (double)(noseOffset + noseHeight);
			break;
		case TooltipPosition.Right:
			x = targetRect.Right;
			y = targetRect.Center.Y - num2 / 2.0;
			num += (double)(noseOffset + noseHeight);
			break;
		case TooltipPosition.Left:
			num += (double)(noseOffset + noseHeight);
			x = targetRect.X - num;
			y = targetRect.Center.Y - num2 / 2.0;
			break;
		}
		Rect positionRect = new Rect(x, y, num, num2);
		EdgedDetection(ref positionRect, containerRect);
		return positionRect;
	}

	internal PointCollection GetGeometryPoints(TooltipPosition position, Rect toolTipRect)
	{
		PointCollection pointCollection = new PointCollection();
		switch (position)
		{
		case TooltipPosition.Auto:
		case TooltipPosition.Top:
			pointCollection.Add(new Point(0.0, 0.0));
			pointCollection.Add(new Point(toolTipRect.Width, 0.0));
			pointCollection.Add(new Point(toolTipRect.Width, toolTipRect.Height - (double)noseHeight - (double)noseOffset));
			pointCollection.Add(new Point(nosePoint.X + (double)noseWidth, nosePoint.Y - (double)noseHeight));
			pointCollection.Add(nosePoint);
			pointCollection.Add(new Point(nosePoint.X - (double)noseWidth, nosePoint.Y - (double)noseHeight));
			pointCollection.Add(new Point(0.0, toolTipRect.Height - (double)noseHeight - (double)noseOffset));
			pointCollection.Add(new Point(0.0, 0.0));
			pointCollection.Add(new Point(toolTipRect.Width, 0.0));
			break;
		case TooltipPosition.Bottom:
			pointCollection.Add(new Point(0.0, nosePoint.Y + (double)noseHeight));
			pointCollection.Add(new Point(nosePoint.X - (double)noseWidth, nosePoint.Y + (double)noseHeight));
			pointCollection.Add(nosePoint);
			pointCollection.Add(new Point(nosePoint.X + (double)noseWidth, nosePoint.Y + (double)noseHeight));
			pointCollection.Add(new Point(toolTipRect.Width, nosePoint.Y + (double)noseHeight));
			pointCollection.Add(new Point(toolTipRect.Width, toolTipRect.Height));
			pointCollection.Add(new Point(0.0, toolTipRect.Height));
			pointCollection.Add(new Point(0.0, nosePoint.Y + (double)noseHeight));
			pointCollection.Add(new Point(nosePoint.X - (double)noseWidth, nosePoint.Y + (double)noseHeight));
			break;
		case TooltipPosition.Right:
			pointCollection.Add(new Point(nosePoint.X + (double)noseHeight, 0.0));
			pointCollection.Add(new Point(toolTipRect.Width, 0.0));
			pointCollection.Add(new Point(toolTipRect.Width, toolTipRect.Height));
			pointCollection.Add(new Point(nosePoint.X + (double)noseHeight, toolTipRect.Height));
			pointCollection.Add(new Point(nosePoint.X + (double)noseHeight, nosePoint.Y + (double)noseWidth));
			pointCollection.Add(nosePoint);
			pointCollection.Add(new Point(nosePoint.X + (double)noseHeight, nosePoint.Y - (double)noseWidth));
			pointCollection.Add(new Point(nosePoint.X + (double)noseHeight, 0.0));
			pointCollection.Add(new Point(toolTipRect.Width, 0.0));
			break;
		case TooltipPosition.Left:
			pointCollection.Add(new Point(0.0, 0.0));
			pointCollection.Add(new Point(toolTipRect.Width - (double)noseHeight - (double)noseOffset, 0.0));
			pointCollection.Add(new Point(toolTipRect.Width - (double)noseHeight - (double)noseOffset, toolTipRect.Height / 2.0 - (double)noseWidth));
			pointCollection.Add(nosePoint);
			pointCollection.Add(new Point(toolTipRect.Width - (double)noseHeight - (double)noseOffset, toolTipRect.Height / 2.0 + (double)noseWidth));
			pointCollection.Add(new Point(toolTipRect.Width - (double)noseHeight - (double)noseOffset, toolTipRect.Height));
			pointCollection.Add(new Point(0.0, toolTipRect.Height));
			pointCollection.Add(new Point(0.0, 0.0));
			pointCollection.Add(new Point(toolTipRect.Width - (double)noseHeight - (double)noseOffset, 0.0));
			break;
		}
		return pointCollection;
	}

	private void EdgedDetection(ref Rect positionRect, Rect containerRect)
	{
		if (positionRect.X < 0.0)
		{
			positionRect.X = 0.0;
		}
		else if (positionRect.Right > containerRect.Width)
		{
			positionRect.X = containerRect.Width - positionRect.Width;
		}
		if (positionRect.Y < 0.0)
		{
			positionRect.Y = 0.0;
		}
		else if (positionRect.Bottom > containerRect.Height)
		{
			positionRect.Y = containerRect.Height - positionRect.Height;
		}
	}

	private PathGeometry GetClipPathGeometry(PointCollection points, TooltipPosition position)
	{
		PathFigure pathFigure = new PathFigure();
		if (points.Count > 0)
		{
			pathFigure.StartPoint = points[0];
			if (points.Count > 1)
			{
				double val = CornerRadius;
				LineSegment lineSegment = new LineSegment();
				for (int i = 1; i < points.Count - 1; i++)
				{
					switch (position)
					{
					case TooltipPosition.Left:
						if (i == 2 || i == 3 || i == 4)
						{
							lineSegment = new LineSegment(points[i]);
							pathFigure.Segments.Add(lineSegment);
							continue;
						}
						break;
					case TooltipPosition.Auto:
					case TooltipPosition.Top:
						if (i == 3 || i == 4 || i == 5)
						{
							lineSegment = new LineSegment(points[i]);
							pathFigure.Segments.Add(lineSegment);
							continue;
						}
						break;
					case TooltipPosition.Right:
						if (i == 4 || i == 5 || i == 6)
						{
							lineSegment = new LineSegment(points[i]);
							pathFigure.Segments.Add(lineSegment);
							continue;
						}
						break;
					case TooltipPosition.Bottom:
						if (i == 1 || i == 2 || i == 3)
						{
							lineSegment = new LineSegment(points[i]);
							pathFigure.Segments.Add(lineSegment);
							continue;
						}
						break;
					}
					Point point = new Point(points[i].X - points[i - 1].X, points[i].Y - points[i - 1].Y);
					Point point2 = new Point(points[i + 1].X - points[i].X, points[i + 1].Y - points[i].Y);
					double num = Math.Sqrt(point.X * point.X + point.Y * point.Y);
					double num2 = Math.Sqrt(point2.X * point2.X + point2.Y * point2.Y);
					double num3 = Math.Min(Math.Min(num, num2) / 2.0, val);
					if (num != 0.0)
					{
						point.X /= num;
						point.Y /= num;
					}
					else
					{
						point = default(Point);
					}
					double num4 = num - num3;
					point.X *= num4;
					point.Y *= num4;
					lineSegment = new LineSegment(points[i - 1].Offset(point.X, point.Y));
					pathFigure.Segments.Add(lineSegment);
					if (num2 != 0.0)
					{
						point2.X /= num2;
						point2.Y /= num2;
					}
					else
					{
						point2 = default(Point);
					}
					point2.X *= num3;
					point2.Y *= num3;
					ArcSegment item = new ArcSegment(points[i].Offset(point2.X, point2.Y), new Size(num3), 0.0, SweepDirection.Clockwise, isLargeArc: false);
					pathFigure.Segments.Add(item);
				}
				pathFigure.Segments.Add(new LineSegment(points[points.Count - 1]));
			}
		}
		PathGeometry pathGeometry = new PathGeometry();
		pathGeometry.Figures.Add(pathFigure);
		return pathGeometry;
	}

	private void AppendPath(PathF path, PathFigureCollection figures, Point translatePoint)
	{
		foreach (PathFigure figure in figures)
		{
			for (int i = 0; i < figure.Segments.Count - 1; i++)
			{
				PathSegment pathSegment = figure.Segments[i];
				if (pathSegment is LineSegment lineSegment)
				{
					AddLine(path, lineSegment, translatePoint);
				}
				else if (pathSegment is ArcSegment arcSegment)
				{
					AddCurve(path, arcSegment, translatePoint, i);
				}
			}
			path.Close();
		}
	}

	private void AddLine(PathF path, LineSegment lineSegment, Point translatePoint)
	{
		path.LineTo((float)(lineSegment.Point.X - translatePoint.X), (float)(lineSegment.Point.Y - translatePoint.Y));
	}

	private void AddCurve(PathF path, ArcSegment arcSegment, Point translatePoint, int index)
	{
		float num = (float)(arcSegment.Point.X - translatePoint.X);
		float num2 = (float)(arcSegment.Point.Y - translatePoint.Y);
		float num3 = CornerRadius - CornerRadius * 0.55f;
		switch (index)
		{
		case 1:
			path.CurveTo(new PointF(num - num3, num2 - CornerRadius), new PointF(num, num2 - CornerRadius + num3), new PointF(num, num2));
			break;
		default:
			if (index != 6)
			{
				if (index == 8 || index == 5)
				{
					path.CurveTo(new PointF(num + num3, num2 + CornerRadius), new PointF(num, num2 + CornerRadius - num3), new PointF(num, num2));
				}
				else if (index == 10)
				{
					path.CurveTo(new PointF(num - CornerRadius, num2 + num3), new PointF(num - CornerRadius + num3, num2), new PointF(num, num2));
				}
				break;
			}
			goto case 3;
		case 3:
			path.CurveTo(new PointF(num + CornerRadius, num2 - num3), new PointF(num + CornerRadius - num3, num2), new PointF(num, num2));
			break;
		}
	}
}
