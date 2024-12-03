using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Graphics.Internals;

public static class DoubleCollectionExtensions
{
	public static float[] ToFloatArray(this DoubleCollection doubleCollection)
	{
		float[] array = new float[doubleCollection.Count];
		for (int i = 0; i < doubleCollection.Count; i++)
		{
			array[i] = (float)doubleCollection[i];
		}
		return array;
	}
}
