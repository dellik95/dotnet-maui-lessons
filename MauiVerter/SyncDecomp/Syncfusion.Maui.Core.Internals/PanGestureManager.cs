using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals;

internal class PanGestureManager
{
	private Point? m_translationPositionAtStart = null;

	private Point m_totalTranslatedPosition = Point.Zero;

	private Point m_scrollOffsetAtStart = Point.Zero;

	private PanZoomListener m_panListener;

	private SfInteractiveScrollView m_scrollView;

	internal PanGestureManager(SfInteractiveScrollView scrollView, PanZoomListener panListener)
	{
		m_scrollView = scrollView;
		m_panListener = panListener;
		SubscribePanEvents();
	}

	private void SubscribePanEvents()
	{
		if (m_panListener != null)
		{
			m_panListener.PanUpdated += OnPanUpdated;
		}
	}

	internal void OnPanUpdated(object? sender, PanEventArgs e)
	{
		if (m_scrollView == null)
		{
			return;
		}
		View content = m_scrollView.Content;
		if (content == null)
		{
			return;
		}
		if (e.Status == GestureStatus.Started && !m_scrollView.IsScrolling)
		{
			m_translationPositionAtStart = new Point(content.TranslationX, content.TranslationY);
			m_scrollOffsetAtStart.X = m_scrollView.ScrollX;
			m_scrollOffsetAtStart.Y = m_scrollView.ScrollY;
		}
		if (e.Status == GestureStatus.Running && m_translationPositionAtStart.HasValue)
		{
			m_totalTranslatedPosition.X += e.TranslatePoint.X;
			m_totalTranslatedPosition.Y += e.TranslatePoint.Y;
			if (!m_scrollView.IsScrolling && (e.TranslatePoint.X != 0.0 || e.TranslatePoint.Y != 0.0))
			{
				if (m_scrollView.ContentSize.Width >= m_scrollView.Width)
				{
					content.TranslationX = m_translationPositionAtStart.Value.X + Math.Clamp(m_totalTranslatedPosition.X, m_scrollOffsetAtStart.X + m_scrollView.Width - m_scrollView.ContentSize.Width, m_scrollOffsetAtStart.X);
				}
				if (m_scrollView.ContentSize.Height >= m_scrollView.Height)
				{
					content.TranslationY = m_translationPositionAtStart.Value.Y + Math.Clamp(m_totalTranslatedPosition.Y, m_scrollOffsetAtStart.Y + m_scrollView.Height - m_scrollView.ContentSize.Height, m_scrollOffsetAtStart.Y);
				}
				ScrollChangedEventArgs scrolledEventArgs = new ScrollChangedEventArgs(m_scrollOffsetAtStart.X + (m_translationPositionAtStart.Value.X - content.TranslationX), m_scrollOffsetAtStart.Y + (m_translationPositionAtStart.Value.Y - content.TranslationY), m_scrollView.ScrollX, m_scrollView.ScrollY);
				m_scrollView.OnScrollChanged(scrolledEventArgs);
			}
		}
		if (e.Status == GestureStatus.Completed && m_translationPositionAtStart.HasValue)
		{
			m_scrollView.ScrollTo(m_scrollOffsetAtStart.X + (m_translationPositionAtStart.Value.X - content.TranslationX), m_scrollOffsetAtStart.Y + (m_translationPositionAtStart.Value.Y - content.TranslationY), animated: false);
			content.TranslationX = m_translationPositionAtStart.Value.X;
			content.TranslationY = m_translationPositionAtStart.Value.Y;
			ResetValues();
		}
	}

	private void ResetValues()
	{
		m_translationPositionAtStart = null;
		m_totalTranslatedPosition = Point.Zero;
		m_scrollOffsetAtStart = Point.Zero;
	}

	private void UnsubscribePanEvents()
	{
		if (m_panListener != null)
		{
			m_panListener.PanUpdated -= OnPanUpdated;
		}
	}

	internal void Dispose()
	{
		UnsubscribePanEvents();
	}
}
