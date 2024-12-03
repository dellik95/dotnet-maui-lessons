using Microsoft.Maui.Hosting;
using Syncfusion.Maui.Core.Internals;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Core.Hosting;

public static class AppHostBuilderExtensions
{
	public static MauiAppBuilder ConfigureSyncfusionCore(this MauiAppBuilder builder)
	{
		builder.ConfigureMauiHandlers(delegate(IMauiHandlersCollection handlers)
		{
			handlers.AddHandler(typeof(IDrawableView), typeof(SfDrawableViewHandler));
			handlers.AddHandler(typeof(IDrawableLayout), typeof(SfViewHandler));
			handlers.AddHandler(typeof(SfDropdownView), typeof(SfDropdownViewHandler));
			handlers.AddHandler(typeof(SfInteractiveScrollView), typeof(SfInteractiveScrollViewHandler));
			handlers.AddHandler(typeof(ListViewScrollViewExt), typeof(ListViewScrollViewHandler));
			handlers.AddHandler(typeof(ISignaturePad), typeof(SignaturePadHandler));
		});
		return builder;
	}
}
