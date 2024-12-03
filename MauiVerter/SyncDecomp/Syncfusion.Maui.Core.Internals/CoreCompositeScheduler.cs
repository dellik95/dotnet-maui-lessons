using System;

namespace Syncfusion.Maui.Core.Internals;

internal class CoreCompositeScheduler : CoreScheduler
{
	private CoreFrameScheduler frameScheduler = new CoreFrameScheduler();

	private CoreMainScheduler mainScheduler = new CoreMainScheduler();

	private Action? callback;

	protected override bool OnSchedule(Action action)
	{
		callback = action;
		frameScheduler.ScheduleCallback(InvokeCallback);
		mainScheduler.ScheduleCallback(InvokeCallback);
		return true;
	}

	private void InvokeCallback()
	{
		if (base.IsScheduled && callback != null)
		{
			callback();
		}
	}
}
