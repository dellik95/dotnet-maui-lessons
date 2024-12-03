using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Internals;

public static class TouchListenerExtension
{
	internal static BindableProperty TouchDetectorProperty = BindableProperty.Create("TouchDetector", typeof(TouchDetector), typeof(View));

	public static void AddTouchListener(this View target, ITouchListener listener)
	{
		TouchDetector touchDetector = target.GetValue(TouchDetectorProperty) as TouchDetector;
		if (touchDetector == null)
		{
			touchDetector = new TouchDetector(target);
			target.SetValue(TouchDetectorProperty, touchDetector);
		}
		touchDetector.AddListener(listener);
	}

	public static void RemoveTouchListener(this View target, ITouchListener listener)
	{
		if (target.GetValue(TouchDetectorProperty) is TouchDetector touchDetector)
		{
			touchDetector.RemoveListener(listener);
			if (!touchDetector.HasListener())
			{
				touchDetector.Dispose();
				target.SetValue(TouchDetectorProperty, null);
			}
		}
	}

	public static void ClearTouchListeners(this View target)
	{
		if (target.GetValue(TouchDetectorProperty) is TouchDetector touchDetector)
		{
			touchDetector.Dispose();
			target.SetValue(TouchDetectorProperty, null);
		}
	}
}
