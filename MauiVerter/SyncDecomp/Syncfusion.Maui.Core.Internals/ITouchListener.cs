namespace Syncfusion.Maui.Core.Internals;

public interface ITouchListener
{
	bool IsTouchHandled => false;

	void OnTouch(PointerEventArgs e);

	void OnScrollWheel(ScrollEventArgs e)
	{
	}
}
