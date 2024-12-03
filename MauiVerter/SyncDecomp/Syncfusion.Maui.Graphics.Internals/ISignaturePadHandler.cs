using Microsoft.Maui;

namespace Syncfusion.Maui.Graphics.Internals;

public interface ISignaturePadHandler : IViewHandler, IElementHandler
{
	new ISignaturePad VirtualView { get; }

	new PlatformSignaturePad PlatformView { get; }
}
