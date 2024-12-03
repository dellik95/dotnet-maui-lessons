using System;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Internals;

public class CoreMainScheduler : CoreScheduler
{
	[Obsolete]
	protected override bool OnSchedule(Action action)
	{
		Device.BeginInvokeOnMainThread(action);
		return true;
	}
}
