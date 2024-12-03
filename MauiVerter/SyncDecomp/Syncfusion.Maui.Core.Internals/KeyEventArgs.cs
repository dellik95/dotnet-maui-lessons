namespace Syncfusion.Maui.Core.Internals;

public class KeyEventArgs
{
	public KeyboardKey Key { get; }

	public bool Handled { get; set; }

	public bool IsShiftKeyPressed { get; internal set; }

	public bool IsCtrlKeyPressed { get; internal set; }

	public bool IsAltKeyPressed { get; internal set; }

	public bool IsCommandKeyPressed { get; internal set; }

	public KeyEventArgs(KeyboardKey keyboardKey)
	{
		Key = keyboardKey;
	}
}
