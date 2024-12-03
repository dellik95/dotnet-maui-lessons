using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Core;

internal class BadgeLabelView
{
	private bool animationEnabled;

	private ITextElement? textElement;

	private View? content;

	private BadgePosition badgePosition = BadgePosition.TopRight;

	private BadgeIcon badgeIcon = BadgeIcon.None;

	private double xPos;

	private double yPos;

	private Color stroke = Colors.Transparent;

	private double strokeThickness;

	private Brush badgeBackground = new SolidColorBrush(Colors.Transparent);

	private Color textColor = Colors.Transparent;

	private string text = string.Empty;

	private string screenReaderText = string.Empty;

	private Thickness textPadding;

	private CornerRadius cornerRadius;

	private float fontSize = 12f;

	private string fontFamily = string.Empty;

	private FontAttributes fontAttributes = FontAttributes.None;

	private bool autoHide;

	private Size textSize;

	private double sizeRatio = 1.0;

	private readonly SfBadgeView badgeView;

	private RectF badgeSize;

	internal double WidthRequest { get; set; }

	internal double HeightRequest { get; set; }

	internal Thickness Margin { get; set; }

	internal bool AnimationEnabled
	{
		get
		{
			return animationEnabled;
		}
		set
		{
			animationEnabled = value;
		}
	}

	internal ITextElement? TextElement
	{
		get
		{
			return textElement;
		}
		set
		{
			textElement = value;
		}
	}

	internal View? Content
	{
		get
		{
			return content;
		}
		set
		{
			content = value;
		}
	}

	internal BadgeIcon BadgeIcon
	{
		get
		{
			return badgeIcon;
		}
		set
		{
			badgeIcon = value;
		}
	}

	internal BadgePosition BadgePosition
	{
		get
		{
			return badgePosition;
		}
		set
		{
			badgePosition = value;
		}
	}

	internal double XPosition
	{
		get
		{
			return xPos;
		}
		set
		{
			xPos = value;
		}
	}

	internal double YPosition
	{
		get
		{
			return yPos;
		}
		set
		{
			yPos = value;
		}
	}

	internal double StrokeThickness
	{
		get
		{
			return strokeThickness;
		}
		set
		{
			strokeThickness = value;
		}
	}

	internal Color Stroke
	{
		get
		{
			return stroke;
		}
		set
		{
			stroke = value;
		}
	}

	internal Brush BadgeBackground
	{
		get
		{
			return badgeBackground;
		}
		set
		{
			badgeBackground = value;
		}
	}

	internal Color TextColor
	{
		get
		{
			return textColor;
		}
		set
		{
			textColor = value;
		}
	}

	internal string Text
	{
		get
		{
			return text;
		}
		set
		{
			text = value;
			CalculateBadgeBounds();
			ShowBadgeBasedOnText(text, AutoHide);
			UpdateSemanticProperties();
		}
	}

	internal string ScreenReaderText
	{
		get
		{
			return screenReaderText;
		}
		set
		{
			screenReaderText = value;
			UpdateSemanticProperties();
		}
	}

	internal Thickness TextPadding
	{
		get
		{
			return textPadding;
		}
		set
		{
			textPadding = value;
			CalculateBadgeBounds();
		}
	}

	internal CornerRadius CornerRadius
	{
		get
		{
			return cornerRadius;
		}
		set
		{
			cornerRadius = value;
		}
	}

	internal float FontSize
	{
		get
		{
			return fontSize;
		}
		set
		{
			fontSize = value;
		}
	}

	internal string FontFamily
	{
		get
		{
			return fontFamily;
		}
		set
		{
			fontFamily = value;
		}
	}

	internal FontAttributes FontAttributes
	{
		get
		{
			return fontAttributes;
		}
		set
		{
			fontAttributes = value;
		}
	}

	internal bool AutoHide
	{
		get
		{
			return autoHide;
		}
		set
		{
			autoHide = value;
			ShowBadgeBasedOnText(Text, autoHide);
		}
	}

	public BadgeLabelView(SfBadgeView badgeView)
	{
		this.badgeView = badgeView;
	}

	internal PointF GetTextStartPoint(Rect viewBounds)
	{
		PointF result = default(PointF);
		result.X = (float)(viewBounds.Center.X - textSize.Width / 2.0);
		result.Y = (float)(viewBounds.Center.Y - textSize.Height / 2.0);
		if (!string.IsNullOrEmpty(Text))
		{
		}
		return result;
	}

	internal void CalculateBadgeBounds()
	{
		RectF rectF = default(RectF);
		if (!string.IsNullOrEmpty(Text))
		{
			ITextMeasurer textMeasurer = TextMeasurer.CreateTextMeasurer();
			textSize = textMeasurer.MeasureText(Text, TextElement);
		}
		else if (BadgeIcon != BadgeIcon.None && BadgeIcon != BadgeIcon.Dot)
		{
			textSize = new Size(GetProccessedFontSize(), GetProccessedFontSize());
		}
		rectF.Width = (float)(textSize.Width + TextPadding.Left + TextPadding.Right);
		rectF.Height = (float)(textSize.Height + TextPadding.Top + TextPadding.Bottom);
		if (!string.IsNullOrEmpty(Text))
		{
			if (Text.Length == 1 && TextPadding.Left == TextPadding.Right && TextPadding.Top == TextPadding.Bottom && TextPadding.Left == TextPadding.Top)
			{
				if (rectF.Width > rectF.Height)
				{
					rectF.Height = rectF.Width;
				}
				else
				{
					rectF.Width = rectF.Height;
				}
			}
			else if (Text.Length > 1)
			{
				rectF.Width += 10f;
			}
		}
		else
		{
			rectF.Width = ((badgeIcon == BadgeIcon.Dot) ? 10f : ((badgeIcon == BadgeIcon.None) ? 0f : rectF.Width));
			rectF.Height = ((badgeIcon == BadgeIcon.Dot) ? 10f : ((badgeIcon == BadgeIcon.None) ? 0f : rectF.Height));
		}
		badgeSize = rectF;
		WidthRequest = rectF.Width;
		HeightRequest = rectF.Height;
	}

	private RectF GetScaledRect()
	{
		RectF rectF = default(RectF);
		rectF.Width = (float)((double)badgeSize.Width * sizeRatio);
		rectF.Height = (float)((double)badgeSize.Height * sizeRatio);
		RectF result = rectF;
		result.X = (float)(WidthRequest - (double)result.Width) / 2f;
		result.Y = (float)(HeightRequest - (double)result.Height) / 2f;
		return result;
	}

	internal void Show()
	{
		if (AnimationEnabled)
		{
			StartScaleAnimation();
		}
	}

	internal void Hide()
	{
		if (AnimationEnabled)
		{
			StartScaleAnimation();
		}
	}

	internal void ShowBadgeBasedOnText(string value, bool canHide)
	{
		if (string.IsNullOrEmpty(value) || value == "0")
		{
			if (!canHide)
			{
				Show();
			}
		}
		else
		{
			Show();
		}
	}

	internal void DrawBadge(ICanvas canvas)
	{
		RectF scaledRect = GetScaledRect();
		RectF fillBounds = CalculatePosition();
		fillBounds = CalculateMargin(fillBounds);
		RectF rectF = default(RectF);
		rectF.X = (float)((double)fillBounds.X + StrokeThickness / 2.0);
		rectF.Y = (float)((double)fillBounds.Y + StrokeThickness / 2.0);
		rectF.Right = (float)((double)fillBounds.Right - StrokeThickness / 2.0);
		rectF.Bottom = (float)((double)fillBounds.Bottom - StrokeThickness / 2.0);
		canvas.SetFillPaint(badgeBackground, fillBounds);
		canvas.FillRoundedRectangle(fillBounds, CornerRadius.TopLeft, CornerRadius.TopRight, CornerRadius.BottomLeft, CornerRadius.BottomRight);
		canvas.StrokeSize = (float)StrokeThickness;
		canvas.StrokeColor = Stroke;
		if (StrokeThickness > 0.0)
		{
			canvas.DrawRoundedRectangle(fillBounds, CornerRadius.TopLeft, CornerRadius.TopRight, CornerRadius.BottomLeft, CornerRadius.BottomRight);
		}
		PointF textStartPoint = GetTextStartPoint(fillBounds);
		if (!string.IsNullOrEmpty(text))
		{
			canvas.DrawText(text, textStartPoint.X, textStartPoint.Y, TextElement);
			return;
		}
		RectF rectF2 = new RectF(textStartPoint.X, textStartPoint.Y, GetScaledFontSize(), GetScaledFontSize());
		canvas.StrokeColor = TextColor;
		canvas.StrokeSize = 1.5f;
		canvas.SetFillPaint(new SolidColorBrush(TextColor), rectF2);
		GetBadgeIcon(canvas, rectF2, BadgeIcon);
	}

	private static void GetBadgeIcon(ICanvas canvas, RectF rect, BadgeIcon value)
	{
		float x = rect.X;
		float y = rect.Y;
		float num = x + rect.Width;
		float num2 = y + rect.Height;
		float y2 = y + rect.Height / 2f;
		switch (value)
		{
		case BadgeIcon.Add:
			rect.X += rect.Width / 12f;
			rect.Y += rect.Height / 12f;
			rect.Width -= rect.Width / 6f;
			rect.Height -= rect.Height / 6f;
			canvas.DrawPlus(rect, hasBorder: false, rect.Width + 4f);
			break;
		case BadgeIcon.Available:
			Syncfusion.Maui.Graphics.Internals.CanvasExtensions.DrawTick(canvas, rect);
			break;
		case BadgeIcon.Away:
			Syncfusion.Maui.Graphics.Internals.CanvasExtensions.DrawAwaySymbol(canvas, rect);
			break;
		case BadgeIcon.Busy:
			canvas.DrawLine(new PointF(x, y2), new PointF(num, y2));
			break;
		case BadgeIcon.Delete:
			rect.X += rect.Width / 6f;
			rect.Y += rect.Height / 6f;
			rect.Width -= rect.Width / 3f;
			rect.Height -= rect.Height / 3f;
			canvas.DrawCross(rect, hasBorder: false, rect.Width);
			break;
		case BadgeIcon.Prohibit1:
			canvas.DrawLine(new PointF(num - 1f, y + 1f), new PointF(x + 1f, num2 - 1f));
			break;
		case BadgeIcon.Prohibit2:
			canvas.DrawLine(new PointF(x + 1f, y + 1f), new PointF(num - 1f, num2 - 1f));
			break;
		case BadgeIcon.Dot:
		case BadgeIcon.None:
			break;
		}
	}

	private RectF CalculateMargin(RectF fillBounds)
	{
		if (BadgePosition == BadgePosition.Top)
		{
			fillBounds.X = (float)((double)fillBounds.X + Margin.Left / 2.0);
			fillBounds.Y = (float)((double)fillBounds.Y + Margin.Top);
		}
		else if (BadgePosition == BadgePosition.Left)
		{
			fillBounds.X = (float)((double)fillBounds.X + Margin.Left);
			fillBounds.Y = (float)((double)fillBounds.Y + Margin.Top / 2.0);
		}
		else if (BadgePosition == BadgePosition.TopLeft)
		{
			fillBounds.X = (float)((double)fillBounds.X + Margin.Left);
			fillBounds.Y = (float)((double)fillBounds.Y + Margin.Top);
		}
		else if (BadgePosition == BadgePosition.Right)
		{
			fillBounds.X = (float)((double)fillBounds.X - Margin.Right);
			fillBounds.Y = (float)((double)fillBounds.Y + Margin.Top / 2.0);
		}
		else if (BadgePosition == BadgePosition.TopRight)
		{
			fillBounds.X = (float)((double)fillBounds.X - Margin.Right);
			fillBounds.Y = (float)((double)fillBounds.Y + Margin.Top);
		}
		else if (BadgePosition == BadgePosition.Bottom)
		{
			fillBounds.X = (float)((double)fillBounds.X + Margin.Left / 2.0);
			fillBounds.Y = (float)((double)fillBounds.Y - Margin.Bottom);
		}
		else if (BadgePosition == BadgePosition.BottomLeft)
		{
			fillBounds.X = (float)((double)fillBounds.X + Margin.Left);
			fillBounds.Y = (float)((double)fillBounds.Y - Margin.Bottom);
		}
		else if (BadgePosition == BadgePosition.BottomRight)
		{
			fillBounds.X = (float)((double)fillBounds.X - Margin.Right);
			fillBounds.Y = (float)((double)fillBounds.Y - Margin.Bottom);
		}
		return fillBounds;
	}

	private RectF CalculatePosition()
	{
		RectF scaledRect = GetScaledRect();
		bool flag = (((IVisualElementController)badgeView).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft;
		RectF result = default(RectF);
		if (BadgePosition == BadgePosition.BottomLeft)
		{
			result.X = (flag ? ((float)(XPosition - (double)badgeSize.Width + (double)scaledRect.X - StrokeThickness / 2.0)) : ((float)(XPosition + (double)scaledRect.X - StrokeThickness / 2.0)));
			result.Y = (float)(YPosition - (double)badgeSize.Width + (double)scaledRect.Y + StrokeThickness / 2.0);
		}
		else if (BadgePosition == BadgePosition.BottomRight)
		{
			result.X = (flag ? ((float)(XPosition + (double)scaledRect.X - StrokeThickness / 2.0)) : ((float)(XPosition - (double)badgeSize.Width + (double)scaledRect.X - StrokeThickness / 2.0)));
			result.Y = (float)(YPosition - (double)badgeSize.Width + (double)scaledRect.Y + StrokeThickness / 2.0);
		}
		else if (BadgePosition == BadgePosition.TopRight)
		{
			result.X = (flag ? ((float)(XPosition + (double)scaledRect.X - StrokeThickness / 2.0)) : ((float)(XPosition - (double)badgeSize.Width + (double)scaledRect.X - StrokeThickness / 2.0)));
			result.Y = (float)(YPosition + (double)scaledRect.Y + StrokeThickness / 2.0);
		}
		else if (BadgePosition == BadgePosition.Right)
		{
			result.X = (flag ? ((float)(XPosition + (double)scaledRect.X - StrokeThickness / 2.0)) : ((float)(XPosition - (double)badgeSize.Width + (double)scaledRect.X - StrokeThickness / 2.0)));
			result.Y = (float)(YPosition - (double)(scaledRect.Height / 2f) - StrokeThickness / 2.0);
		}
		else if (BadgePosition == BadgePosition.Left)
		{
			result.X = (flag ? ((float)(XPosition - (double)badgeSize.Width + (double)scaledRect.X - StrokeThickness / 2.0)) : ((float)(XPosition + (double)scaledRect.X - StrokeThickness / 2.0)));
			result.Y = (float)(YPosition - (double)(scaledRect.Height / 2f) - StrokeThickness / 2.0);
		}
		else if (BadgePosition == BadgePosition.Bottom)
		{
			result.X = (float)(XPosition - (double)(scaledRect.Width / 2f) - StrokeThickness / 2.0);
			result.Y = (float)(YPosition - (double)badgeSize.Width + (double)scaledRect.Y + StrokeThickness / 2.0);
		}
		else if (BadgePosition == BadgePosition.Top)
		{
			result.X = (float)(XPosition - (double)(scaledRect.Width / 2f) - StrokeThickness / 2.0);
			result.Y = (float)(YPosition + (double)scaledRect.Y + StrokeThickness / 2.0);
		}
		else if (BadgePosition == BadgePosition.TopLeft)
		{
			result.X = (flag ? ((float)(XPosition - (double)badgeSize.Width + (double)scaledRect.X - StrokeThickness / 2.0)) : ((float)(XPosition + (double)scaledRect.X - StrokeThickness / 2.0)));
			result.Y = (float)(YPosition + (double)scaledRect.Y + StrokeThickness / 2.0);
		}
		result.Width = scaledRect.Width;
		result.Height = scaledRect.Height;
		return result;
	}

	private void UpdateSemanticProperties()
	{
		if (!string.IsNullOrEmpty(ScreenReaderText))
		{
		}
	}

	private float GetProccessedFontSize()
	{
		float num = FontSize;
		if (num <= 1f)
		{
			num = 1f;
		}
		return num;
	}

	private float GetScaledFontSize()
	{
		float num = (float)((double)FontSize * sizeRatio);
		if (num <= 1f)
		{
			num = 1f;
		}
		return num;
	}

	private void StartScaleAnimation()
	{
		try
		{
			if ((badgeView.WidthRequest > 0.0 && badgeView.HeightRequest > 0.0) || (badgeView.Height > 0.0 && badgeView.Width > 0.0))
			{
				Animation animation = new Animation(OnShowHideAnimationUpdate);
				animation.Commit(badgeView, "showAnimator", 16u, 250u, Easing.Linear, OnShowHideAnimationEnded, () => false);
			}
		}
		catch (ArgumentException)
		{
		}
	}

	private void OnShowHideAnimationUpdate(double value)
	{
		sizeRatio = value;
		badgeView.InvalidateDrawable();
	}

	private void OnShowHideAnimationEnded(double value, bool isCompleted)
	{
		if (value == 0.0 && isCompleted)
		{
			sizeRatio = value;
		}
	}
}
