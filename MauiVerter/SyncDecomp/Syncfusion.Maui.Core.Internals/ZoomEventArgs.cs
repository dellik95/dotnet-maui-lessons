using System;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals;

public class ZoomEventArgs : EventArgs
{
	public Point ZoomOrigin { get; internal set; }

	public double? Angle { get; internal set; }

	public double ZoomFactor { get; internal set; }

	internal ZoomEventArgs(double zoomFactor, Point zoomOrigin)
	{
		ZoomOrigin = zoomOrigin;
		ZoomFactor = zoomFactor;
	}
}
