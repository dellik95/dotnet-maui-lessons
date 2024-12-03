using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Internals;

public abstract class ListViewScrollViewExt : ScrollView
{
	internal bool IsProgrammaticScrolling = false;

	internal abstract bool ScrollingEnabled { get; }

	internal abstract bool DisableAnimation { get; set; }

	internal double ScrollEndPosition { get; set; }

	internal abstract double ScrollPosition { get; set; }

	internal abstract double GetContainerTotalExtent();

	internal abstract void ProcessAutoOnScroll();

	internal abstract bool IsListViewInSwiping();

	internal abstract bool IsSwipingEnabled();

	internal abstract bool IsNativeScrollViewLoaded();

	internal abstract bool IsViewLoadedAndHasHorizontalRTL();

	internal abstract void SetIsHorizontalRTLViewLoaded(bool IsHorizontalRTLViewLoaded, double scrollX);

	internal abstract void SetScrollState(string scrollState);

	internal abstract string GetScrollState();

	internal abstract void InvalidateContainerIfRequired();
}
