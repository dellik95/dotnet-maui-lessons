using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Animations;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Core;

internal class TooltipHelper : ITextElement
{
	private readonly Action callback;

	internal float noseHeight = 5f;

	internal float noseWidth = 6f;

	internal float noseOffset = 2f;

	private float cornerRadius = 4f;

	private Point nosePoint;

	private double animationValue = 1.0;

	private TooltipPosition actualPosition;

	private IAnimationManager? animationManager = null;

	private Microsoft.Maui.Animations.Animation? durationAnimation;

	internal Rect TooltipRect { get; set; }

	internal Rect RoundedRect { get; set; }

	internal Thickness ContentViewMargin { get; set; }

	internal Size ContentSize { get; set; }

	internal TooltipPosition PriorityPosition { get; set; } = TooltipPosition.Top;


	internal List<TooltipPosition> PriorityPositionList { get; set; } = new List<TooltipPosition>
	{
		TooltipPosition.Bottom,
		TooltipPosition.Left,
		TooltipPosition.Right
	};


	public string Text { get; set; } = string.Empty;


	public Microsoft.Maui.Font Font { get; set; }

	public double FontSize { get; set; } = 14.0;


	public string FontFamily { get; set; } = "TimesNewRoman";


	public FontAttributes FontAttributes { get; set; } = FontAttributes.None;


	public TooltipPosition Position { get; set; } = TooltipPosition.Auto;


	public TooltipDrawType DrawType { get; set; } = TooltipDrawType.Default;


	public Thickness Padding { get; set; } = new Thickness(5.0);


	public int Duration { get; set; } = 2;


	public Brush Background { get; set; } = Brush.Black;


	public Brush Stroke { get; set; } = Brush.Transparent;


	public Color TextColor { get; set; } = Colors.White;


	public float StrokeWidth { get; set; } = 0f;


	public TooltipHelper(Action action)
	{
		callback = action;
	}

	public void Show(Rect containerRect, Rect targetRect, bool animated)
	{
		if (containerRect.IsEmpty || (string.IsNullOrEmpty(Text) && ContentSize.IsZero))
		{
			return;
		}
		double x = containerRect.X;
		double y = containerRect.Y;
		double width = containerRect.Width;
		double height = containerRect.Height;
		if (!(targetRect.X > x + width) && !(targetRect.Y > y + height))
		{
			bool flag = !ContentSize.IsZero;
			Size size = (flag ? ContentSize : Text.Measure(this));
			Size contentSize = new Size(size.Width + Padding.Left + Padding.Right + (double)StrokeWidth, size.Height + Padding.Top + Padding.Bottom + (double)StrokeWidth);
			Rect tooltipRect = GetTooltipRect(Position, containerRect, targetRect, contentSize);
			actualPosition = GetAcutalTooltipPosition(containerRect, targetRect, ref tooltipRect, contentSize, Position, noseHeight, noseOffset);
			GetNosePoint(actualPosition, targetRect, tooltipRect, contentSize, flag);
			if (!flag)
			{
				TooltipRect = GetDrawRect(tooltipRect, actualPosition, noseHeight, noseOffset);
				Invalidate();
				Finished();
			}
			else
			{
				TooltipRect = tooltipRect;
				RoundedRect = GetRoundedRect();
			}
		}
	}

	public void Hide(bool animated)
	{
		TooltipRect = Rect.Zero;
		Invalidate();
	}

	public void Draw(ICanvas canvas)
	{
		OnDraw(canvas);
	}

	protected virtual void OnDraw(ICanvas canvas)
	{
		if ((!string.IsNullOrEmpty(Text) || !ContentSize.IsZero) && !TooltipRect.IsEmpty)
		{
			switch (DrawType)
			{
			case TooltipDrawType.Default:
				DrawDefaultTooltip(canvas);
				break;
			}
		}
	}

	private void Invalidate()
	{
		callback();
	}

	private void SetAnimationManager()
	{
		if (Application.Current != null && animationManager == null)
		{
			IElementHandler handler = Application.Current.Handler;
			if (handler != null && handler.MauiContext != null)
			{
				animationManager = handler.MauiContext.Services.GetRequiredService<IAnimationManager>();
			}
		}
	}

	private void UpdateToolTipAnimation(double value)
	{
		animationValue = value;
		Invalidate();
	}

	private void CommitToolTipAnimation(double start, double end)
	{
		SetAnimationManager();
		if (animationManager != null)
		{
			Microsoft.Maui.Animations.Animation animation = new Microsoft.Maui.Animations.Animation(UpdateToolTipAnimation, 0.0, 1.0, null, Finished);
			Microsoft.Maui.Animations.Animation animation2 = new Microsoft.Maui.Animations.Animation(UpdateToolTipAnimation, start, end, Easing.CubicInOut);
			animation.Add(0.0, 1.0, animation2);
			animation.Commit(animationManager);
		}
	}

	private void Finished()
	{
		SetAnimationManager();
		if (animationManager != null)
		{
			durationAnimation = null;
			durationAnimation = new Microsoft.Maui.Animations.Animation(Animate, 0.0, Duration, Easing.Linear, AutoHide);
			durationAnimation.Commit(animationManager);
		}
	}

	private void Animate(double value)
	{
	}

	private void AutoHide()
	{
		if (durationAnimation != null && durationAnimation.HasFinished)
		{
			Hide(animated: false);
		}
	}

	private Rect GetAnimationRect()
	{
		Rect result = TooltipRect;
		double x = TooltipRect.X;
		double y = TooltipRect.Y;
		double width = TooltipRect.Width;
		double height = TooltipRect.Height;
		double num = animationValue;
		switch ((actualPosition == TooltipPosition.Auto) ? PriorityPosition : actualPosition)
		{
		case TooltipPosition.Top:
			result = new Rect(x + width / 2.0 * (1.0 - num), y + height * (1.0 - num), width * num, height * num);
			break;
		case TooltipPosition.Left:
			result = new Rect(x + width * (1.0 - num), y + height / 2.0 * (1.0 - num), width * num, height * num);
			break;
		case TooltipPosition.Right:
			result = new Rect(x, y + height / 2.0 * (1.0 - num), width * num, height * num);
			break;
		case TooltipPosition.Bottom:
			result = new Rect(x + width / 2.0 * (1.0 - num), y, width * num, height * num);
			break;
		}
		return result;
	}

	private Rect GetTooltipRect(TooltipPosition position, Rect containerRect, Rect targetRect, Size contentSize)
	{
		double x = 0.0;
		double y = 0.0;
		double num = contentSize.Width;
		double num2 = contentSize.Height;
		switch ((position == TooltipPosition.Auto) ? PriorityPosition : position)
		{
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

	private void EdgedDetection(ref Rect positionRect, Rect containerRect)
	{
		if (Position == TooltipPosition.Auto || Position == TooltipPosition.Top || Position == TooltipPosition.Bottom)
		{
			if (positionRect.X < 0.0)
			{
				positionRect.X = 0f + StrokeWidth / 2f;
			}
			else if (positionRect.Right > containerRect.Width)
			{
				positionRect.X = containerRect.Width - positionRect.Width - (double)(StrokeWidth / 2f);
			}
		}
		if (Position == TooltipPosition.Auto)
		{
			if (positionRect.Y < 0.0)
			{
				positionRect.Y = 0.0;
			}
			else if (positionRect.Bottom > containerRect.Height)
			{
				positionRect.Y = containerRect.Height - positionRect.Height;
			}
		}
	}

	private TooltipPosition GetAcutalTooltipPosition(Rect containerRect, Rect targetRect, ref Rect tooltipRect, Size contentSize, TooltipPosition tooltipPosition, float noseHeight, float noseOffset)
	{
		TooltipPosition tooltipPosition2 = tooltipPosition;
		if (tooltipPosition2 == TooltipPosition.Auto && !IsTargetRectIntersectsWith(tooltipRect, targetRect))
		{
			List<TooltipPosition> priorityPositionList = PriorityPositionList;
			foreach (TooltipPosition item in priorityPositionList)
			{
				Rect tooltipRect2 = GetTooltipRect(item, containerRect, targetRect, contentSize);
				if (IsTargetRectIntersectsWith(tooltipRect2, targetRect))
				{
					tooltipRect = tooltipRect2;
					tooltipPosition2 = item;
					break;
				}
			}
		}
		return (tooltipPosition2 == TooltipPosition.Auto) ? PriorityPosition : tooltipPosition2;
	}

	private Rect GetDrawRect(Rect tooltipRect, TooltipPosition actualPosition, float noseHeight, float noseOffset)
	{
		float num = noseOffset + noseHeight;
		switch (actualPosition)
		{
		case TooltipPosition.Top:
			tooltipRect.Y -= StrokeWidth;
			tooltipRect.Height -= num;
			break;
		case TooltipPosition.Bottom:
			tooltipRect.Y += num + StrokeWidth;
			tooltipRect.Height -= num;
			break;
		case TooltipPosition.Right:
			tooltipRect.X += num + StrokeWidth;
			tooltipRect.Width -= num;
			break;
		case TooltipPosition.Left:
			tooltipRect.X -= StrokeWidth;
			tooltipRect.Width -= num;
			break;
		}
		return tooltipRect;
	}

	private Rect GetRoundedRect()
	{
		Rect result = Rect.Zero;
		float num = noseOffset + noseHeight;
		double width = TooltipRect.Width;
		double height = TooltipRect.Height;
		Thickness padding = Padding;
		switch (actualPosition)
		{
		case TooltipPosition.Top:
			padding.Bottom += num;
			result = new Rect(new Point(0.0, 0.0), new Size(width, height - (double)num));
			break;
		case TooltipPosition.Bottom:
			padding.Top += num;
			result = new Rect(new Point(0.0, num), new Size(width, height - (double)num));
			break;
		case TooltipPosition.Right:
			padding.Left += num;
			result = new Rect(new Point(num, 0.0), new Size(width - (double)num, height));
			break;
		case TooltipPosition.Left:
			padding.Right += num;
			result = new Rect(new Point(0.0, 0.0), new Size(width - (double)num, height));
			break;
		}
		ContentViewMargin = padding;
		return result;
	}

	private bool IsTargetRectIntersectsWith(Rect tooltipRect, Rect targetRect)
	{
		return !tooltipRect.IntersectsWith(targetRect);
	}

	private void GetNosePoint(TooltipPosition position, Rect targetRect, Rect tooltipRect, Size contentSize, bool isContentView)
	{
		double num = 0.0;
		if (!isContentView)
		{
			switch (position)
			{
			case TooltipPosition.Top:
			case TooltipPosition.Bottom:
				num = ((!(tooltipRect.Width < targetRect.Width)) ? Math.Abs(targetRect.X + targetRect.Width / 2.0) : (tooltipRect.X + contentSize.Width / 2.0));
				nosePoint = new Point(num, (position == TooltipPosition.Auto || position == TooltipPosition.Top) ? (targetRect.Y - (double)noseOffset - (double)StrokeWidth) : (tooltipRect.Y + (double)noseOffset + (double)StrokeWidth));
				break;
			case TooltipPosition.Left:
			case TooltipPosition.Right:
				nosePoint = new Point(y: (!(tooltipRect.Height < targetRect.Height)) ? Math.Abs(targetRect.Y + targetRect.Height / 2.0) : (tooltipRect.Y + contentSize.Height / 2.0), x: (position == TooltipPosition.Right) ? (tooltipRect.X + (double)noseOffset + (double)StrokeWidth) : (tooltipRect.X + tooltipRect.Width - (double)noseOffset - (double)StrokeWidth));
				break;
			}
		}
		else
		{
			switch (position)
			{
			case TooltipPosition.Top:
			case TooltipPosition.Bottom:
				num = ((!(tooltipRect.Width < targetRect.Width)) ? (Math.Abs(tooltipRect.X - targetRect.X) + targetRect.Width / 2.0) : (contentSize.Width / 2.0));
				nosePoint = new Point(num, (position == TooltipPosition.Auto || position == TooltipPosition.Top) ? (tooltipRect.Height - (double)noseOffset) : ((double)noseOffset));
				break;
			case TooltipPosition.Left:
			case TooltipPosition.Right:
				nosePoint = new Point(y: (!(tooltipRect.Height < targetRect.Height)) ? (Math.Abs(tooltipRect.Y - targetRect.Y) + targetRect.Height / 2.0) : (contentSize.Height / 2.0), x: (position == TooltipPosition.Right) ? ((double)noseOffset) : (tooltipRect.Width - (double)noseOffset));
				break;
			}
		}
	}

	private PointCollection GetPointCollection(Rect roundedRect)
	{
		PointCollection pointCollection = new PointCollection();
		double num = (double)cornerRadius * 0.44777152600000003;
		Rect rect = roundedRect;
		double x = rect.X;
		double y = rect.Y;
		double width = rect.Width;
		double height = rect.Height;
		switch (actualPosition)
		{
		case TooltipPosition.Top:
			pointCollection.Add(new Point(x + (double)cornerRadius, y));
			pointCollection.Add(new Point(x + width - (double)cornerRadius, y));
			pointCollection.Add(new Point(x + width - num, y));
			pointCollection.Add(new Point(x + width, y + num));
			pointCollection.Add(new Point(x + width, y + (double)cornerRadius));
			pointCollection.Add(new Point(x + width, y + height - (double)cornerRadius));
			pointCollection.Add(new Point(x + width, y + height - num));
			pointCollection.Add(new Point(x + width - num, y + height));
			pointCollection.Add(new Point(x + width - (double)cornerRadius, y + height));
			pointCollection.Add(new Point(nosePoint.X + (double)noseWidth, nosePoint.Y - (double)noseHeight));
			pointCollection.Add(nosePoint);
			pointCollection.Add(new Point(nosePoint.X - (double)noseWidth, nosePoint.Y - (double)noseHeight));
			pointCollection.Add(new Point(x + (double)cornerRadius, y + height));
			pointCollection.Add(new Point(x + num, y + height));
			pointCollection.Add(new Point(x, y + height - num));
			pointCollection.Add(new Point(x, y + height - (double)cornerRadius));
			pointCollection.Add(new Point(x, y + (double)cornerRadius));
			pointCollection.Add(new Point(x, y + num));
			pointCollection.Add(new Point(x + num, y));
			pointCollection.Add(new Point(x + (double)cornerRadius, y));
			break;
		case TooltipPosition.Bottom:
			pointCollection.Add(new Point(x + width - (double)cornerRadius, y + height));
			pointCollection.Add(new Point(x + (double)cornerRadius, y + height));
			pointCollection.Add(new Point(x + num, y + height));
			pointCollection.Add(new Point(x, y + height - num));
			pointCollection.Add(new Point(x, y + height - (double)cornerRadius));
			pointCollection.Add(new Point(x, y + (double)cornerRadius));
			pointCollection.Add(new Point(x, y + num));
			pointCollection.Add(new Point(x + num, y));
			pointCollection.Add(new Point(x + (double)cornerRadius, y));
			pointCollection.Add(new Point(nosePoint.X - (double)noseWidth, nosePoint.Y + (double)noseHeight));
			pointCollection.Add(nosePoint);
			pointCollection.Add(new Point(nosePoint.X + (double)noseWidth, nosePoint.Y + (double)noseHeight));
			pointCollection.Add(new Point(x + width - (double)cornerRadius, y));
			pointCollection.Add(new Point(x + width - num, y));
			pointCollection.Add(new Point(x + width, y + num));
			pointCollection.Add(new Point(x + width, y + (double)cornerRadius));
			pointCollection.Add(new Point(x + width, y + height - (double)cornerRadius));
			pointCollection.Add(new Point(x + width, y + height - num));
			pointCollection.Add(new Point(x + width - num, y + height));
			pointCollection.Add(new Point(x + width - (double)cornerRadius, y + height));
			break;
		case TooltipPosition.Right:
			pointCollection.Add(new Point(x + width, y + (double)cornerRadius));
			pointCollection.Add(new Point(x + width, y + height - (double)cornerRadius));
			pointCollection.Add(new Point(x + width, y + height - num));
			pointCollection.Add(new Point(x + width - num, y + height));
			pointCollection.Add(new Point(x + width - (double)cornerRadius, y + height));
			pointCollection.Add(new Point(x + (double)cornerRadius, y + height));
			pointCollection.Add(new Point(x + num, y + height));
			pointCollection.Add(new Point(x, y + height - num));
			pointCollection.Add(new Point(x, y + height - (double)cornerRadius));
			pointCollection.Add(new Point(nosePoint.X + (double)noseHeight, nosePoint.Y + (double)noseWidth));
			pointCollection.Add(nosePoint);
			pointCollection.Add(new Point(nosePoint.X + (double)noseHeight, nosePoint.Y - (double)noseWidth));
			pointCollection.Add(new Point(x, y + (double)cornerRadius));
			pointCollection.Add(new Point(x, y + num));
			pointCollection.Add(new Point(x + num, y));
			pointCollection.Add(new Point(x + (double)cornerRadius, y));
			pointCollection.Add(new Point(x + width - (double)cornerRadius, y));
			pointCollection.Add(new Point(x + width - num, y));
			pointCollection.Add(new Point(x + width, y + num));
			pointCollection.Add(new Point(x + width, y + (double)cornerRadius));
			break;
		case TooltipPosition.Left:
			pointCollection.Add(new Point(x, y + height - (double)cornerRadius));
			pointCollection.Add(new Point(x, y + (double)cornerRadius));
			pointCollection.Add(new Point(x, y + num));
			pointCollection.Add(new Point(x + num, y));
			pointCollection.Add(new Point(x + (double)cornerRadius, y));
			pointCollection.Add(new Point(x + width - (double)cornerRadius, y));
			pointCollection.Add(new Point(x + width - num, y));
			pointCollection.Add(new Point(x + width, y + num));
			pointCollection.Add(new Point(x + width, y + (double)cornerRadius));
			pointCollection.Add(new Point(nosePoint.X - (double)noseHeight, nosePoint.Y + (double)noseWidth));
			pointCollection.Add(nosePoint);
			pointCollection.Add(new Point(nosePoint.X - (double)noseHeight, nosePoint.Y - (double)noseWidth));
			pointCollection.Add(new Point(x + width, y + height - (double)cornerRadius));
			pointCollection.Add(new Point(x + width, y + height - num));
			pointCollection.Add(new Point(x + width - num, y + height));
			pointCollection.Add(new Point(x + width - (double)cornerRadius, y + height));
			pointCollection.Add(new Point(x + (double)cornerRadius, y + height));
			pointCollection.Add(new Point(x + num, y + height));
			pointCollection.Add(new Point(x, y + height - num));
			pointCollection.Add(new Point(x, y + height - (double)cornerRadius));
			break;
		}
		return pointCollection;
	}

	private PathF GetDrawPath(PointCollection points)
	{
		PathF pathF = new PathF();
		if (points != null)
		{
			pathF.MoveTo(points[0]);
			pathF.LineTo(points[1]);
			pathF.CurveTo(points[2], points[3], points[4]);
			pathF.LineTo(points[5]);
			pathF.CurveTo(points[6], points[7], points[8]);
			pathF.LineTo(points[9]);
			pathF.LineTo(points[10]);
			pathF.LineTo(points[11]);
			pathF.LineTo(points[12]);
			pathF.CurveTo(points[13], points[14], points[15]);
			pathF.LineTo(points[16]);
			pathF.CurveTo(points[17], points[18], points[19]);
		}
		pathF.Close();
		return pathF;
	}

	private void DrawDefaultTooltip(ICanvas canvas)
	{
		Rect rect = ((animationValue >= 1.0) ? TooltipRect : GetAnimationRect());
		PointCollection pointCollection = GetPointCollection(ContentSize.IsZero ? rect : RoundedRect);
		PathF drawPath = GetDrawPath(pointCollection);
		canvas.SaveState();
		canvas.StrokeColor = PaintExtensions.ToColor(Stroke);
		canvas.StrokeSize = StrokeWidth;
		canvas.FillColor = PaintExtensions.ToColor(Background);
		canvas.FillPath(drawPath);
		canvas.DrawPath(drawPath);
		if (!string.IsNullOrEmpty(Text))
		{
			if (animationValue < 1.0)
			{
				TooltipFont tooltipFont = new TooltipFont(this);
				double num = tooltipFont.FontSize * animationValue;
				tooltipFont.FontSize = ((num < 1.0) ? 1.0 : num);
				canvas.DrawText(Text, rect, HorizontalAlignment.Center, VerticalAlignment.Center, tooltipFont);
			}
			else
			{
				canvas.DrawText(Text, rect, HorizontalAlignment.Center, VerticalAlignment.Center, this);
			}
		}
		canvas.RestoreState();
	}

	void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
	{
	}

	void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
	{
	}

	double ITextElement.FontSizeDefaultValueCreator()
	{
		throw new NotImplementedException();
	}

	void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
	{
	}

	void ITextElement.OnFontChanged(Microsoft.Maui.Font oldValue, Microsoft.Maui.Font newValue)
	{
	}
}
