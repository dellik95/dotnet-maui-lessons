using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Animations;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Internals;

public class CoreFrameScheduler : CoreScheduler
{
	private Action? callback;

	private readonly Microsoft.Maui.Animations.Animation animation;

	private IAnimationManager? animationManager;

	public CoreFrameScheduler()
	{
		animation = new Microsoft.Maui.Animations.Animation(OnFrameStart, 0.0010000000474974513, 0.0);
	}

	protected override bool OnSchedule(Action action)
	{
		if (Application.Current != null)
		{
			IElementHandler handler = Application.Current.Handler;
			if (handler != null && handler.MauiContext != null)
			{
				animationManager = handler.MauiContext.Services.GetRequiredService<IAnimationManager>();
			}
			if (animationManager != null)
			{
				callback = action;
				animation.Reset();
				animation.Commit(animationManager);
				return true;
			}
		}
		return false;
	}

	private void OnFrameStart(double value)
	{
		if (callback != null)
		{
			callback();
			callback = null;
		}
	}
}
