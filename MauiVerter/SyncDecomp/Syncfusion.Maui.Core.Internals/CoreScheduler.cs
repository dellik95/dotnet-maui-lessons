using System;

namespace Syncfusion.Maui.Core.Internals;

public abstract class CoreScheduler
{
	private Action? callback;

	public bool IsScheduled { get; private set; }

	public static CoreScheduler CreateScheduler(CoreSchedulerType type = CoreSchedulerType.Frame)
	{
		return type switch
		{
			CoreSchedulerType.Frame => new CoreFrameScheduler(), 
			CoreSchedulerType.Main => new CoreMainScheduler(), 
			_ => new CoreCompositeScheduler(), 
		};
	}

	public bool ScheduleCallback(Action action)
	{
		if (action == null)
		{
			throw new ArgumentNullException(nameof(action));
		}
		if (!IsScheduled)
		{
			IsScheduled = true;
			callback = action;
			return OnSchedule(InvokeCallback);
		}
		return false;
	}

	private void InvokeCallback()
	{
		if (callback != null)
		{
			callback();
			callback = null;
			IsScheduled = false;
		}
	}

	protected abstract bool OnSchedule(Action action);
}
