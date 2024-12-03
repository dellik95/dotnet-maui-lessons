using Microsoft.Maui.Controls;
using Microsoft.UI.Input;
using Windows.System;
using Windows.UI.Core;

namespace Syncfusion.Maui.Core.Internals;

public static class KeyboardListenerExtension
{
	internal static BindableProperty KeyboardDetectorProperty = BindableProperty.Create("KeyboardDetector", typeof(KeyboardDetector), typeof(View));

	public static void AddKeyboardListener(this View target, IKeyboardListener listener)
	{
		KeyboardDetector keyboardDetector = target.GetValue(KeyboardDetectorProperty) as KeyboardDetector;
		if (keyboardDetector == null)
		{
			keyboardDetector = new KeyboardDetector(target);
			target.SetValue(KeyboardDetectorProperty, keyboardDetector);
		}
		keyboardDetector.AddListener(listener);
	}

	public static void RemoveKeyboardListener(this View target, IKeyboardListener listener)
	{
		if (target.GetValue(KeyboardDetectorProperty) is KeyboardDetector keyboardDetector)
		{
			keyboardDetector.RemoveListener(listener);
			if (!keyboardDetector.HasListener())
			{
				keyboardDetector.Dispose();
				target.SetValue(KeyboardDetectorProperty, null);
			}
		}
	}

	public static void ClearKeyboardListeners(this View target)
	{
		if (target.GetValue(KeyboardDetectorProperty) is KeyboardDetector keyboardDetector)
		{
			keyboardDetector.ClearListeners();
			keyboardDetector.Dispose();
			target.SetValue(KeyboardDetectorProperty, null);
		}
	}

	internal static KeyboardKey ConvertToKeyboardKey(VirtualKey argsKey)
	{
		KeyboardKey keyboardKey = KeyboardKey.None;
		switch (argsKey)
		{
		case VirtualKey.Space:
			keyboardKey = KeyboardKey.Space;
			break;
		case VirtualKey.Tab:
			keyboardKey = KeyboardKey.Tab;
			break;
		case VirtualKey.Home:
			keyboardKey = KeyboardKey.Home;
			break;
		case VirtualKey.PageDown:
			keyboardKey = KeyboardKey.PageDown;
			break;
		case VirtualKey.PageUp:
			keyboardKey = KeyboardKey.PageUp;
			break;
		case VirtualKey.Back:
			keyboardKey = KeyboardKey.Back;
			break;
		case VirtualKey.End:
			keyboardKey = KeyboardKey.End;
			break;
		case VirtualKey.Up:
			keyboardKey = KeyboardKey.Up;
			break;
		case VirtualKey.Down:
			keyboardKey = KeyboardKey.Down;
			break;
		case VirtualKey.Right:
			keyboardKey = KeyboardKey.Right;
			break;
		case VirtualKey.Left:
			keyboardKey = KeyboardKey.Left;
			break;
		case VirtualKey.Delete:
			keyboardKey = KeyboardKey.Delete;
			break;
		case VirtualKey.Add:
			keyboardKey = KeyboardKey.Add;
			break;
		case VirtualKey.Subtract:
			keyboardKey = KeyboardKey.Subtract;
			break;
		case VirtualKey.Multiply:
			keyboardKey = KeyboardKey.Multiply;
			break;
		case VirtualKey.Divide:
			keyboardKey = KeyboardKey.Divide;
			break;
		case VirtualKey.Print:
			keyboardKey = KeyboardKey.Print;
			break;
		case VirtualKey.CapitalLock:
			keyboardKey = KeyboardKey.CapsLock;
			break;
		case VirtualKey.Decimal:
			keyboardKey = KeyboardKey.Decimal;
			break;
		case VirtualKey.Shift:
			keyboardKey = KeyboardKey.Shift;
			break;
		case VirtualKey.Control:
			keyboardKey = KeyboardKey.Ctrl;
			break;
		case VirtualKey.Enter:
			keyboardKey = KeyboardKey.Enter;
			break;
		case VirtualKey.Escape:
			keyboardKey = KeyboardKey.Escape;
			break;
		case VirtualKey.Insert:
			keyboardKey = KeyboardKey.Insert;
			break;
		case VirtualKey.Help:
			keyboardKey = KeyboardKey.Help;
			break;
		case VirtualKey.Menu:
			keyboardKey = KeyboardKey.Alt;
			break;
		case VirtualKey.A:
			keyboardKey = KeyboardKey.A;
			break;
		case VirtualKey.B:
			keyboardKey = KeyboardKey.B;
			break;
		case VirtualKey.C:
			keyboardKey = KeyboardKey.C;
			break;
		case VirtualKey.D:
			keyboardKey = KeyboardKey.D;
			break;
		case VirtualKey.E:
			keyboardKey = KeyboardKey.E;
			break;
		case VirtualKey.F:
			keyboardKey = KeyboardKey.F;
			break;
		case VirtualKey.G:
			keyboardKey = KeyboardKey.G;
			break;
		case VirtualKey.H:
			keyboardKey = KeyboardKey.H;
			break;
		case VirtualKey.I:
			keyboardKey = KeyboardKey.I;
			break;
		case VirtualKey.J:
			keyboardKey = KeyboardKey.J;
			break;
		case VirtualKey.K:
			keyboardKey = KeyboardKey.K;
			break;
		case VirtualKey.L:
			keyboardKey = KeyboardKey.L;
			break;
		case VirtualKey.M:
			keyboardKey = KeyboardKey.M;
			break;
		case VirtualKey.N:
			keyboardKey = KeyboardKey.N;
			break;
		case VirtualKey.O:
			keyboardKey = KeyboardKey.O;
			break;
		case VirtualKey.P:
			keyboardKey = KeyboardKey.P;
			break;
		case VirtualKey.Q:
			keyboardKey = KeyboardKey.Q;
			break;
		case VirtualKey.R:
			keyboardKey = KeyboardKey.R;
			break;
		case VirtualKey.S:
			keyboardKey = KeyboardKey.S;
			break;
		case VirtualKey.T:
			keyboardKey = KeyboardKey.T;
			break;
		case VirtualKey.U:
			keyboardKey = KeyboardKey.U;
			break;
		case VirtualKey.V:
			keyboardKey = KeyboardKey.V;
			break;
		case VirtualKey.W:
			keyboardKey = KeyboardKey.W;
			break;
		case VirtualKey.X:
			keyboardKey = KeyboardKey.X;
			break;
		case VirtualKey.Y:
			keyboardKey = KeyboardKey.Y;
			break;
		case VirtualKey.Z:
			keyboardKey = KeyboardKey.Z;
			break;
		case VirtualKey.Number0:
			keyboardKey = KeyboardKey.Num0;
			break;
		case VirtualKey.Number1:
			keyboardKey = KeyboardKey.Num1;
			break;
		case VirtualKey.Number2:
			keyboardKey = KeyboardKey.Num2;
			break;
		case VirtualKey.Number3:
			keyboardKey = KeyboardKey.Num3;
			break;
		case VirtualKey.Number4:
			keyboardKey = KeyboardKey.Num4;
			break;
		case VirtualKey.Number5:
			keyboardKey = KeyboardKey.Num5;
			break;
		case VirtualKey.Number6:
			keyboardKey = KeyboardKey.Num6;
			break;
		case VirtualKey.Number7:
			keyboardKey = KeyboardKey.Num7;
			break;
		case VirtualKey.Number8:
			keyboardKey = KeyboardKey.Num8;
			break;
		case VirtualKey.Number9:
			keyboardKey = KeyboardKey.Num9;
			break;
		case VirtualKey.F1:
			keyboardKey = KeyboardKey.F1;
			break;
		case VirtualKey.F2:
			keyboardKey = KeyboardKey.F2;
			break;
		case VirtualKey.F3:
			keyboardKey = KeyboardKey.F3;
			break;
		case VirtualKey.F4:
			keyboardKey = KeyboardKey.F4;
			break;
		case VirtualKey.F5:
			keyboardKey = KeyboardKey.F5;
			break;
		case VirtualKey.F6:
			keyboardKey = KeyboardKey.F6;
			break;
		case VirtualKey.F7:
			keyboardKey = KeyboardKey.F7;
			break;
		case VirtualKey.F8:
			keyboardKey = KeyboardKey.F8;
			break;
		case VirtualKey.F9:
			keyboardKey = KeyboardKey.F9;
			break;
		case VirtualKey.F10:
			keyboardKey = KeyboardKey.F10;
			break;
		case VirtualKey.F11:
			keyboardKey = KeyboardKey.F11;
			break;
		case VirtualKey.F12:
			keyboardKey = KeyboardKey.F12;
			break;
		}
		if (keyboardKey == KeyboardKey.None && int.TryParse(argsKey.ToString(), out var result))
		{
			keyboardKey = (KeyboardKey)result;
		}
		return keyboardKey;
	}

	internal static bool CheckShiftKeyPressed()
	{
		if (InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down))
		{
			return true;
		}
		return false;
	}

	internal static bool CheckControlKeyPressed()
	{
		if (InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down))
		{
			return true;
		}
		return false;
	}

	internal static bool CheckAltKeyPressed()
	{
		if (InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Menu).HasFlag(CoreVirtualKeyStates.Down))
		{
			return true;
		}
		return false;
	}
}
