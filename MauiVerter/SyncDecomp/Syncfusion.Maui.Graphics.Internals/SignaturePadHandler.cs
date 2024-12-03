using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;

namespace Syncfusion.Maui.Graphics.Internals;

public class SignaturePadHandler : ViewHandler<ISignaturePad, PlatformSignaturePad>, ISignaturePadHandler, IViewHandler, IElementHandler
{
	public static PropertyMapper<ISignaturePad, SignaturePadHandler> Mapper = new PropertyMapper<ISignaturePad, SignaturePadHandler>((PropertyMapper)ViewHandler.ViewMapper)
	{
		["MaximumStrokeThickness"] = MapMaximumStrokeThickness,
		["MinimumStrokeThickness"] = MapMinimumStrokeThickness,
		["StrokeColor"] = MapStrokeColor
	};

	public static CommandMapper<ISignaturePad, ISignaturePadHandler> CommandMapper = new CommandMapper<ISignaturePad, ISignaturePadHandler>(ViewHandler.ViewCommandMapper) { ["Clear"] = MapClear };

	ISignaturePad ISignaturePadHandler.VirtualView => base.VirtualView;

	PlatformSignaturePad ISignaturePadHandler.PlatformView => base.PlatformView;

	public SignaturePadHandler()
		: base((IPropertyMapper)Mapper, (CommandMapper?)CommandMapper)
	{
	}

	protected override void ConnectHandler(PlatformSignaturePad platformView)
	{
		platformView.Connect(base.VirtualView);
		base.ConnectHandler(platformView);
	}

	protected override void DisconnectHandler(PlatformSignaturePad platformView)
	{
		platformView.Disconnect();
		base.DisconnectHandler(platformView);
	}

	internal ImageSource? ToImageSource()
	{
		return base.PlatformView?.ToImageSource();
	}

	protected override PlatformSignaturePad CreatePlatformView()
	{
		return new PlatformSignaturePad();
	}

	public static void MapMaximumStrokeThickness(SignaturePadHandler handler, ISignaturePad virtualView)
	{
		handler.PlatformView.UpdateMaximumStrokeThickness(virtualView);
	}

	public static void MapMinimumStrokeThickness(SignaturePadHandler handler, ISignaturePad virtualView)
	{
		handler.PlatformView.UpdateMinimumStrokeThickness(virtualView);
	}

	public static void MapStrokeColor(SignaturePadHandler handler, ISignaturePad virtualView)
	{
		handler.PlatformView.UpdateStrokeColor(virtualView);
	}

	public static void MapClear(ISignaturePadHandler handler, ISignaturePad virtualView, object? arg)
	{
		handler.PlatformView.Clear();
	}

	public override void UpdateValue(string property)
	{
		base.UpdateValue(property);
		if (property == "Background")
		{
			base.PlatformView.UpdateBackground(base.VirtualView);
		}
	}
}
