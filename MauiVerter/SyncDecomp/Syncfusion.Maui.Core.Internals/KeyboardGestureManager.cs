namespace Syncfusion.Maui.Core.Internals;

internal class KeyboardGestureManager : IKeyboardListener
{
	private SfInteractiveScrollView m_scrollView;

	internal KeyboardGestureManager(SfInteractiveScrollView scrollView)
	{
		m_scrollView = scrollView;
		m_scrollView.AddKeyboardListener(this);
	}

	public void OnKeyDown(KeyEventArgs args)
	{
		if (m_scrollView == null)
		{
			return;
		}
		float num = 60f;
		switch (args.Key)
		{
		case KeyboardKey.Down:
			m_scrollView.ScrollToY(m_scrollView.ScrollY + (double)num);
			break;
		case KeyboardKey.Up:
			m_scrollView.ScrollToY(m_scrollView.ScrollY - (double)num);
			break;
		case KeyboardKey.Left:
			m_scrollView.ScrollToX(m_scrollView.ScrollX - (double)num);
			break;
		case KeyboardKey.Right:
			m_scrollView.ScrollToX(m_scrollView.ScrollX + (double)num);
			break;
		case KeyboardKey.Home:
			if (args.IsShiftKeyPressed)
			{
				m_scrollView.ScrollToX(0.0);
			}
			else
			{
				m_scrollView.ScrollToY(0.0);
			}
			break;
		case KeyboardKey.End:
			if (args.IsShiftKeyPressed)
			{
				m_scrollView.ScrollToX(m_scrollView.ContentSize.Width);
			}
			else
			{
				m_scrollView.ScrollToY(m_scrollView.ContentSize.Height);
			}
			break;
		case KeyboardKey.PageUp:
			if (args.IsShiftKeyPressed)
			{
				m_scrollView.ScrollToX(m_scrollView.ScrollX - m_scrollView.ViewportWidth);
			}
			else
			{
				m_scrollView.ScrollToY(m_scrollView.ScrollY - m_scrollView.ViewportHeight);
			}
			break;
		case KeyboardKey.PageDown:
			if (args.IsShiftKeyPressed)
			{
				m_scrollView.ScrollToX(m_scrollView.ScrollX + m_scrollView.ViewportWidth);
			}
			else
			{
				m_scrollView.ScrollToY(m_scrollView.ScrollY + m_scrollView.ViewportHeight);
			}
			break;
		case KeyboardKey.Shift:
		case KeyboardKey.Ctrl:
		case KeyboardKey.Command:
		case KeyboardKey.Alt:
		case KeyboardKey.Tab:
			break;
		}
	}

	public void OnKeyUp(KeyEventArgs args)
	{
	}

	internal void Dispose()
	{
		m_scrollView?.RemoveKeyboardListener(this);
	}
}
