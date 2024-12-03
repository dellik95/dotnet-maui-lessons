using System.Numerics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Win2D;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;

namespace Syncfusion.Maui.Graphics.Internals;

public static class CanvasExtensions
{
	public static void DrawTriangle(this ICanvas canvas, RectF rect, bool hasBorder)
	{
		float x = rect.X;
		float y = rect.Y;
		float x2 = x + rect.Width;
		float y2 = y + rect.Height;
		float x3 = x + rect.Width / 2f;
		PathF pathF = new PathF();
		pathF.MoveTo(x, y2);
		pathF.LineTo(x3, y);
		pathF.LineTo(x2, y2);
		pathF.LineTo(x, y2);
		pathF.Close();
		canvas.FillPath(pathF);
		if (hasBorder)
		{
			canvas.DrawPath(pathF);
		}
	}

	public static void DrawInverseTriangle(this ICanvas canvas, RectF rect, bool hasBorder)
	{
		float x = rect.X;
		float y = rect.Y;
		float x2 = x + rect.Width;
		float y2 = y + rect.Height;
		float x3 = x + rect.Width / 2f;
		PathF pathF = new PathF();
		pathF.MoveTo(x, y);
		pathF.LineTo(x2, y);
		pathF.LineTo(x3, y2);
		pathF.LineTo(x, y);
		pathF.Close();
		canvas.FillPath(pathF);
		if (hasBorder)
		{
			canvas.DrawPath(pathF);
		}
	}

	public static void DrawCross(this ICanvas canvas, RectF rect, bool hasBorder, float thickness = 5f)
	{
		float x = rect.X;
		float y = rect.Y;
		float num = x + rect.Width;
		float num2 = y + rect.Height;
		float num3 = x + rect.Width / 2f;
		float num4 = y + rect.Height / 2f;
		float num5 = rect.Width / thickness;
		float num6 = rect.Height / thickness;
		PathF pathF = new PathF();
		pathF.MoveTo(num - num5, y);
		pathF.LineTo(num, y + num6);
		pathF.LineTo(num3 + num5, num4);
		pathF.LineTo(num, num2 - num6);
		pathF.LineTo(num - num5, num2);
		pathF.LineTo(num3, num4 + num6);
		pathF.LineTo(x + num5, num2);
		pathF.LineTo(x, num2 - num6);
		pathF.LineTo(num3 - num5, num4);
		pathF.LineTo(x, y + num6);
		pathF.LineTo(x + num5, y);
		pathF.LineTo(num3, num4 - num6);
		pathF.LineTo(num - num5, y);
		pathF.Close();
		canvas.FillPath(pathF);
		if (hasBorder)
		{
			canvas.DrawPath(pathF);
		}
	}

	public static void DrawPlus(this ICanvas canvas, RectF rect, bool hasBorder, float thickness = 5f)
	{
		float x = rect.X;
		float y = rect.Y;
		float x2 = x + rect.Width;
		float y2 = y + rect.Height;
		float num = x + rect.Width / 2f;
		float num2 = y + rect.Height / 2f;
		float num3 = rect.Width / thickness;
		float num4 = rect.Height / thickness;
		PathF pathF = new PathF();
		pathF.MoveTo(num + num3, y);
		pathF.LineTo(num + num3, num2 - num4);
		pathF.LineTo(x2, num2 - num4);
		pathF.LineTo(x2, num2 + num4);
		pathF.LineTo(num + num3, num2 + num4);
		pathF.LineTo(num + num3, y2);
		pathF.LineTo(num - num3, y2);
		pathF.LineTo(num - num3, num2 + num4);
		pathF.LineTo(x, num2 + num4);
		pathF.LineTo(x, num2 - num4);
		pathF.LineTo(num - num3, num2 - num4);
		pathF.LineTo(num - num3, y);
		pathF.LineTo(num + num3, y);
		pathF.Close();
		canvas.FillPath(pathF);
		if (hasBorder)
		{
			canvas.DrawPath(pathF);
		}
	}

	public static void DrawDiamond(this ICanvas canvas, RectF rect, bool hasBorder)
	{
		float x = rect.X;
		float y = rect.Y;
		float x2 = x + rect.Width;
		float y2 = y + rect.Height;
		float x3 = x + rect.Width / 2f;
		float y3 = y + rect.Height / 2f;
		PathF pathF = new PathF();
		pathF.MoveTo(x3, y);
		pathF.LineTo(x2, y3);
		pathF.LineTo(x3, y2);
		pathF.LineTo(x, y3);
		pathF.LineTo(x3, y);
		pathF.Close();
		canvas.FillPath(pathF);
		if (hasBorder)
		{
			canvas.DrawPath(pathF);
		}
	}

	public static void DrawHexagon(this ICanvas canvas, RectF rect, bool hasBorder)
	{
		float x = rect.X;
		float y = rect.Y;
		float x2 = x + rect.Width;
		float num = y + rect.Height;
		float x3 = x + rect.Width / 2f;
		float num2 = rect.Height / 4f;
		PathF pathF = new PathF();
		pathF.MoveTo(x3, y);
		pathF.LineTo(x2, y + num2);
		pathF.LineTo(x2, num - num2);
		pathF.LineTo(x3, num);
		pathF.LineTo(x, num - num2);
		pathF.LineTo(x, y + num2);
		pathF.LineTo(x3, y);
		pathF.Close();
		canvas.FillPath(pathF);
		if (hasBorder)
		{
			canvas.DrawPath(pathF);
		}
	}

	public static void DrawPentagon(this ICanvas canvas, RectF rect, bool hasBorder)
	{
		float x = rect.X;
		float y = rect.Y;
		float num = x + rect.Width;
		float y2 = y + rect.Height;
		float x2 = x + rect.Width / 2f;
		float num2 = rect.Width / 5f;
		float num3 = rect.Height / 3f;
		PathF pathF = new PathF();
		pathF.MoveTo(x2, y);
		pathF.LineTo(num, y + num3);
		pathF.LineTo(num - num2, y2);
		pathF.LineTo(x + num2, y2);
		pathF.LineTo(x, y + num3);
		pathF.LineTo(x2, y);
		pathF.Close();
		canvas.FillPath(pathF);
		if (hasBorder)
		{
			canvas.DrawPath(pathF);
		}
	}

	public static void DrawAwaySymbol(ICanvas canvas, RectF rect)
	{
		PathF pathF = new PathF();
		pathF.MoveTo(rect.X + rect.Width / 2f - 1f, rect.Y);
		pathF.LineTo(rect.X + rect.Width / 2f - 1f, rect.Y + rect.Height / 2f + 1f);
		pathF.LineTo(rect.X + rect.Width / 2f + 4.5f, rect.Y + rect.Height - 1.5f);
		canvas.DrawPath(pathF);
	}

	public static void DrawTick(ICanvas canvas, RectF rect)
	{
		PathF pathF = new PathF();
		pathF.MoveTo(rect.X + 1f, rect.Y + rect.Height - 6f);
		pathF.LineTo(rect.X + 4.5f, rect.Y + rect.Height - 2.5f);
		pathF.LineTo(rect.X + rect.Width - 0.5f, rect.Y + 2f);
		canvas.DrawPath(pathF);
	}

	public static void DrawText(this ICanvas canvas, string value, float x, float y, ITextElement textElement)
	{
		if (canvas is W2DCanvas w2DCanvas)
		{
			using CanvasTextFormat canvasTextFormat = new CanvasTextFormat();
			IFontManager requiredService = MauiWinUIApplication.Current.Services.GetRequiredService<IFontManager>();
			Microsoft.Maui.Font font = textElement.Font;
			FontFamily fontFamily = requiredService.GetFontFamily(font);
			canvasTextFormat.FontFamily = fontFamily.Source;
			canvasTextFormat.FontSize = (float)textElement.FontSize;
			canvasTextFormat.FontStyle = font.ToFontStyle();
			canvasTextFormat.FontWeight = font.ToFontWeight();
			w2DCanvas.Session.DrawText(value, new Vector2(x, y), textElement.TextColor.AsColor(), canvasTextFormat);
		}
	}

	public static void DrawText(this ICanvas canvas, string value, Microsoft.Maui.Graphics.Rect rect, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, ITextElement textElement)
	{
		if (!(canvas is W2DCanvas w2DCanvas))
		{
			return;
		}
		using CanvasTextFormat canvasTextFormat = new CanvasTextFormat();
		IFontManager requiredService = MauiWinUIApplication.Current.Services.GetRequiredService<IFontManager>();
		Microsoft.Maui.Font font = textElement.Font;
		FontFamily fontFamily = requiredService.GetFontFamily(font);
		canvasTextFormat.FontFamily = fontFamily.Source;
		canvasTextFormat.FontSize = (float)textElement.FontSize;
		canvasTextFormat.FontStyle = font.ToFontStyle();
		canvasTextFormat.FontWeight = font.ToFontWeight();
		CanvasVerticalAlignment verticalAlignment2 = CanvasVerticalAlignment.Top;
		switch (verticalAlignment)
		{
		case VerticalAlignment.Center:
			verticalAlignment2 = CanvasVerticalAlignment.Center;
			break;
		case VerticalAlignment.Bottom:
			verticalAlignment2 = CanvasVerticalAlignment.Bottom;
			break;
		}
		canvasTextFormat.VerticalAlignment = verticalAlignment2;
		CanvasHorizontalAlignment horizontalAlignment2 = CanvasHorizontalAlignment.Left;
		switch (horizontalAlignment)
		{
		case HorizontalAlignment.Center:
			horizontalAlignment2 = CanvasHorizontalAlignment.Center;
			break;
		case HorizontalAlignment.Right:
			horizontalAlignment2 = CanvasHorizontalAlignment.Right;
			break;
		}
		canvasTextFormat.HorizontalAlignment = horizontalAlignment2;
		canvasTextFormat.Options = CanvasDrawTextOptions.Clip;
		w2DCanvas.Session.DrawText(value, new Windows.Foundation.Rect(rect.X, rect.Y, rect.Width, rect.Height), textElement.TextColor.AsColor(), canvasTextFormat);
	}

	public static void DrawLines(this ICanvas canvas, float[] points, ILineDrawing lineDrawing)
	{
		if (canvas is W2DCanvas w2DCanvas)
		{
			int num = 0;
			w2DCanvas.StrokeSize = (float)lineDrawing.StrokeWidth;
			w2DCanvas.StrokeColor = lineDrawing.Stroke;
			w2DCanvas.Antialias = lineDrawing.EnableAntiAliasing;
			w2DCanvas.Alpha = lineDrawing.Opacity;
			if (lineDrawing.StrokeDashArray != null)
			{
				w2DCanvas.StrokeDashPattern = lineDrawing.StrokeDashArray.ToFloatArray();
			}
			PathF pathF = new PathF();
			while (num + 1 < points.Length)
			{
				pathF.LineTo(points[num++], points[num++]);
			}
			pathF = new PathF(pathF);
			w2DCanvas.DrawPath(pathF);
		}
	}
}
