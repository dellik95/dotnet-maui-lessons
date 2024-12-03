namespace Syncfusion.Maui.Core.Internals;

public interface ITapGestureListener : IGestureListener
{
	void OnTap(TapEventArgs e);

	void OnTap(object sender, TapEventArgs e)
	{
	}
}
