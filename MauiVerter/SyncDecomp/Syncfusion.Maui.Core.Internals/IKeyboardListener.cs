namespace Syncfusion.Maui.Core.Internals;

public interface IKeyboardListener
{
	bool CanBecomeFirstResponder => false;

	void OnKeyDown(KeyEventArgs args);

	void OnKeyUp(KeyEventArgs args);
}
