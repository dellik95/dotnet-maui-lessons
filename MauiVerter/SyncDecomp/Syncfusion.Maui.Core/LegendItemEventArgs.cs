namespace Syncfusion.Maui.Core;

public class LegendItemEventArgs
{
	public readonly ILegendItem LegendItem;

	public LegendItemEventArgs(ILegendItem legendItem)
	{
		LegendItem = legendItem;
	}
}
