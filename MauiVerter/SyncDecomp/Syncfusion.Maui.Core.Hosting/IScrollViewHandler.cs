using Microsoft.Maui;
using Microsoft.UI.Xaml.Controls;

namespace Syncfusion.Maui.Core.Hosting;

public interface IScrollViewHandler : IViewHandler, IElementHandler
{
	new IScrollView VirtualView { get; }

	new ScrollViewer PlatformView { get; }
}
