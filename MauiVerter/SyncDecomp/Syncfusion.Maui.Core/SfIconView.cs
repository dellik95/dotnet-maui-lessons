using System;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Core;

internal class SfIconView : SfDrawableView
{
	private bool isOpen;

	private Color borderColor;

	private Color highlightColor;

	private string text;

	private bool visibility;

	private TextStyle textStyle;

	private bool isSelected;

	private bool isRTL;

	private Func<double, string, string>? getTrimWeekNumberText;

	private double iconSize => textStyle.FontSize / 1.5;

	internal TextStyle TextStyle
	{
		get
		{
			return textStyle;
		}
		set
		{
			if (textStyle.FontSize != value.FontSize || textStyle.TextColor != value.TextColor || textStyle.FontAttributes != value.FontAttributes || !(textStyle.FontFamily == value.FontFamily))
			{
				textStyle = value;
				InvalidateDrawable();
			}
		}
	}

	internal Color BorderColor
	{
		get
		{
			return borderColor;
		}
		set
		{
			if (borderColor != value)
			{
				borderColor = value;
				InvalidateDrawable();
			}
		}
	}

	internal Color HighlightColor
	{
		get
		{
			return highlightColor;
		}
		set
		{
			if (highlightColor != value)
			{
				highlightColor = value;
				InvalidateDrawable();
			}
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
			if (!(text == value))
			{
				text = value;
			}
		}
	}

	internal bool IsOpen
	{
		get
		{
			return isOpen;
		}
		set
		{
			if (isOpen != value)
			{
				isOpen = value;
			}
		}
	}

	internal bool IsSelected
	{
		get
		{
			return isSelected;
		}
		set
		{
			if (isSelected != value)
			{
				isSelected = value;
				InvalidateDrawable();
			}
		}
	}

	internal bool IsRTL
	{
		get
		{
			return isRTL;
		}
		set
		{
			if (isRTL != value)
			{
				isRTL = value;
				InvalidateDrawable();
			}
		}
	}

	internal bool Visibility
	{
		get
		{
			return visibility;
		}
		set
		{
			base.IsVisible = value;
			visibility = value;
		}
	}

	internal SfIcon Icon { get; set; }

	internal SfIconView(SfIcon icon, ITextElement textStyle, string text, Color borderColor, Color highlightColor, bool isSelected = false, bool isRTL = false, Func<double, string, string>? getTrimWeekNumberText = null)
	{
		Icon = icon;
		this.textStyle = new TextStyle
		{
			TextColor = textStyle.TextColor,
			FontSize = textStyle.FontSize,
			FontAttributes = textStyle.FontAttributes,
			FontFamily = textStyle.FontFamily
		};
		this.text = text;
		this.isRTL = isRTL;
		this.isSelected = isSelected;
		this.borderColor = borderColor;
		this.highlightColor = highlightColor;
		this.getTrimWeekNumberText = getTrimWeekNumberText;
		visibility = true;
	}

	internal void UpdateStyle(ITextElement textStyle)
	{
		TextStyle = new TextStyle
		{
			TextColor = textStyle.TextColor,
			FontSize = textStyle.FontSize,
			FontAttributes = textStyle.FontAttributes,
			FontFamily = textStyle.FontFamily
		};
	}

	internal void UpdateIconColor(Color iconColor)
	{
		TextStyle = new TextStyle
		{
			TextColor = iconColor,
			FontSize = textStyle.FontSize,
			FontAttributes = textStyle.FontAttributes,
			FontFamily = textStyle.FontFamily
		};
	}

	protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
	{
		canvas.SaveState();
		canvas.StrokeSize = 1.5f;
		canvas.FillColor = textStyle.TextColor;
		canvas.StrokeColor = textStyle.TextColor;
		float width = dirtyRect.Width;
		float height = dirtyRect.Height;
		switch (Icon)
		{
		case SfIcon.Forward:
			DrawForward(width, height, canvas);
			break;
		case SfIcon.Backward:
			DrawBackward(width, height, canvas);
			break;
		case SfIcon.Downward:
			DrawDownward(width, height, canvas);
			break;
		case SfIcon.Upward:
			DrawUpward(width, height, canvas);
			break;
		case SfIcon.Today:
			DrawToday(width, height, canvas);
			break;
		case SfIcon.Option:
			DrawOption(width, height, canvas);
			break;
		case SfIcon.Button:
			DrawTilesButton(width, height, canvas);
			break;
		case SfIcon.ComboBox:
			DrawComboBox(width, height, canvas);
			break;
		case SfIcon.TodayButton:
			DrawTodayButton(width, height, canvas);
			break;
		case SfIcon.Divider:
			DrawDivider(width, height, canvas);
			break;
		case SfIcon.WeekNumber:
			DrawWeekNumber(width, height, canvas);
			break;
		}
		canvas.RestoreState();
	}

	private void DrawForward(float width, float height, ICanvas canvas)
	{
		float num = (float)iconSize;
		float num2 = num / 2f;
		float y = height / 2f;
		float x = width / 2f - num2 / 2f;
		float y2 = height / 2f - num / 2f;
		float x2 = width / 2f + num2 / 2f;
		float y3 = height / 2f + num / 2f;
		PathF pathF = new PathF();
		pathF.MoveTo(x, y2);
		pathF.LineTo(x2, y);
		pathF.LineTo(x, y3);
		canvas.DrawPath(pathF);
	}

	private void DrawBackward(float width, float height, ICanvas canvas)
	{
		float num = (float)iconSize;
		float num2 = num / 2f;
		float y = height / 2f;
		float x = width / 2f - num2 / 2f;
		float y2 = height / 2f - num / 2f;
		float x2 = width / 2f + num2 / 2f;
		float y3 = height / 2f + num / 2f;
		PathF pathF = new PathF();
		pathF.MoveTo(x2, y2);
		pathF.LineTo(x, y);
		pathF.LineTo(x2, y3);
		canvas.DrawPath(pathF);
	}

	private void DrawDownward(float width, float height, ICanvas canvas)
	{
		float num = (float)iconSize;
		float num2 = num / 2f;
		float x = width / 2f;
		float x2 = width / 2f - num / 2f;
		float y = height / 2f - num2 / 2f;
		float x3 = width / 2f + num / 2f;
		float y2 = height / 2f + num2 / 2f;
		PathF pathF = new PathF();
		pathF.MoveTo(x2, y);
		pathF.LineTo(x, y2);
		pathF.LineTo(x3, y);
		canvas.DrawPath(pathF);
	}

	private void DrawUpward(float width, float height, ICanvas canvas)
	{
		float num = (float)iconSize;
		float num2 = num / 2f;
		float x = width / 2f;
		float x2 = width / 2f - num / 2f;
		float y = height / 2f - num2 / 2f;
		float x3 = width / 2f + num / 2f;
		float y2 = height / 2f + num2 / 2f;
		PathF pathF = new PathF();
		pathF.MoveTo(x2, y2);
		pathF.LineTo(x, y);
		pathF.LineTo(x3, y2);
		canvas.DrawPath(pathF);
	}

	private void DrawToday(float width, float height, ICanvas canvas)
	{
		float num = (float)iconSize;
		float num2 = num / 5f;
		float num3 = width / 2f - num / 2f;
		float num4 = num3 + num;
		float num5 = height / 2f - num / 2f;
		canvas.DrawRoundedRectangle(num3, num5, num, num, 1f);
		canvas.DrawLine(num3, num5 + num2, num4, num5 + num2);
		canvas.DrawLine(num3 + num2, num5, num3 + num2, num5 - num2);
		canvas.DrawLine(num4 - num2, num5, num4 - num2, num5 - num2);
		canvas.FillRectangle(num3 + num2, num5 + 2f * num2, num2, num2);
	}

	private void DrawOption(float width, float height, ICanvas canvas)
	{
		float num = (float)iconSize;
		float num2 = num / 8f;
		float centerY = height / 2f;
		float centerX = width / 2f;
		float num3 = height / 2f - num / 2f;
		float num4 = height / 2f + num / 2f;
		canvas.FillCircle(centerX, num3 + num2, num2);
		canvas.FillCircle(centerX, centerY, num2);
		canvas.FillCircle(centerX, num4 - num2, num2);
	}

	private void DrawTodayButton(float width, float height, ICanvas canvas)
	{
		DrawTilesButton(width, height, canvas);
	}

	private void DrawDivider(float width, float height, ICanvas canvas)
	{
		canvas.StrokeSize = 1f;
		canvas.SaveState();
		Color fillColor = (canvas.StrokeColor = GetCellBorderColor());
		canvas.FillColor = fillColor;
		float num = 2f;
		canvas.DrawLine(width / 2f, num, width / 2f, height - num);
		canvas.RestoreState();
	}

	private Color GetCellBorderColor()
	{
		if (borderColor != Colors.Transparent)
		{
			return borderColor;
		}
		return Colors.LightGray;
	}

	private void DrawComboBox(float width, float height, ICanvas canvas)
	{
		int num = 5;
		int num2 = 2 * num;
		int num3 = 10;
		canvas.StrokeSize = 1f;
		float num4 = 1f;
		canvas.SaveState();
		TextStyle textStyle = new TextStyle
		{
			FontSize = this.textStyle.FontSize,
			TextColor = (isSelected ? highlightColor : this.textStyle.TextColor),
			FontAttributes = this.textStyle.FontAttributes,
			FontFamily = this.textStyle.FontFamily
		};
		DrawText(canvas, Text, textStyle, new RectF(num2, 0f, width - 2f, height - 2f), HorizontalAlignment.Left, VerticalAlignment.Center);
		canvas.RestoreState();
		canvas.SaveState();
		canvas.StrokeColor = textStyle.TextColor;
		canvas.FillColor = textStyle.TextColor;
		canvas.DrawRoundedRectangle(num4, num4, width - 2f, height - 2f, 5f);
		canvas.RestoreState();
		canvas.StrokeColor = this.textStyle.TextColor.WithAlpha(0.5f);
		canvas.FillColor = this.textStyle.TextColor.WithAlpha(0.5f);
		if (!IsOpen)
		{
			canvas.DrawInverseTriangle(new RectF(width - (float)(2 * num2), (height - 4f) / 2f, num3, num3 / 2), hasBorder: false);
		}
		else
		{
			canvas.DrawTriangle(new RectF(width - (float)(2 * num2), (height - 4f) / 2f, num3, num3 / 2), hasBorder: false);
		}
	}

	private void DrawText(ICanvas canvas, string text, ITextElement textStyle, Rect rect, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, VerticalAlignment verticalAlignment = VerticalAlignment.Top)
	{
		if (!(rect.Height <= 0.0) && !(rect.Width <= 0.0))
		{
			canvas.DrawText(text, rect, horizontalAlignment, verticalAlignment, textStyle);
		}
	}

	private void DrawTilesButton(float width, float height, ICanvas canvas)
	{
		canvas.StrokeSize = 1f;
		Color textColor;
		Color color;
		if (isSelected)
		{
			textColor = highlightColor;
			color = textColor;
		}
		else
		{
			textColor = this.textStyle.TextColor;
			color = GetCellBorderColor();
		}
		float num = 1f;
		canvas.SaveState();
		TextStyle textStyle = new TextStyle
		{
			FontSize = this.textStyle.FontSize,
			TextColor = textColor,
			FontAttributes = this.textStyle.FontAttributes,
			FontFamily = this.textStyle.FontFamily
		};
		DrawText(canvas, Text, textStyle, new RectF(num, num, width - 2f, height - 2f), HorizontalAlignment.Center, VerticalAlignment.Center);
		canvas.RestoreState();
		canvas.SaveState();
		canvas.StrokeColor = color;
		canvas.FillColor = color;
		canvas.DrawRoundedRectangle(num, num, width - 2f, height - 2f, 5f);
		canvas.RestoreState();
	}

	private void DrawWeekNumber(float width, float height, ICanvas canvas)
	{
		string arg = Text;
		if (getTrimWeekNumberText != null)
		{
			arg = getTrimWeekNumberText(width, arg);
		}
		HorizontalAlignment horizontalAlignment = (isRTL ? HorizontalAlignment.Right : HorizontalAlignment.Left);
		double x = ((!isRTL) ? 5 : 0);
		DrawText(canvas, arg, textStyle, new Rect(x, 1.0, width - 5f, height - 1f), horizontalAlignment);
	}
}
