using System;
using Microsoft.Maui;
using Microsoft.UI.Xaml;

namespace Syncfusion.Maui.Core.Internals;

internal static class WindowOverlayUtils
{
	internal static FrameworkElement ToPlatform(this IElement view)
	{
		if (view is IReplaceableView replaceableView && replaceableView.ReplacedView != view)
		{
			return replaceableView.ReplacedView.ToPlatform();
		}
		if (view.Handler == null)
		{
			throw new InvalidOperationException("MauiContext should have been set on parent.");
		}
		if (view.Handler is IViewHandler viewHandler)
		{
			if (viewHandler.ContainerView is FrameworkElement result)
			{
				return result;
			}
			if (viewHandler.PlatformView is FrameworkElement result2)
			{
				return result2;
			}
		}
		return (view.Handler?.PlatformView as FrameworkElement) ?? throw new InvalidOperationException($"Unable to convert {view} to {typeof(FrameworkElement)}");
	}
}
