using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Internals;

public static class GestureListenerExtension
{
	internal static BindableProperty GestureDetectorProperty = BindableProperty.Create("GestureDetector", typeof(GestureDetector), typeof(View));

	public static void AddGestureListener(this View target, IGestureListener listener)
	{
		GestureDetector gestureDetector = target.GetValue(GestureDetectorProperty) as GestureDetector;
		if (gestureDetector == null)
		{
			gestureDetector = new GestureDetector(target);
			target.SetValue(GestureDetectorProperty, gestureDetector);
		}
		gestureDetector.AddListener(listener);
	}

	public static void RemoveGestureListener(this View target, IGestureListener listener)
	{
		if (target.GetValue(GestureDetectorProperty) is GestureDetector gestureDetector)
		{
			gestureDetector.RemoveListener(listener);
			if (!gestureDetector.HasListener())
			{
				gestureDetector.Dispose();
				target.SetValue(GestureDetectorProperty, null);
			}
		}
	}

	public static void ClearGestureListeners(this View target)
	{
		if (target.GetValue(GestureDetectorProperty) is GestureDetector gestureDetector)
		{
			gestureDetector.Dispose();
			target.SetValue(GestureDetectorProperty, null);
		}
	}
}
