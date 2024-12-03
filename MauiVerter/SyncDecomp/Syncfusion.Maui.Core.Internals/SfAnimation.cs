using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Animations;

namespace Syncfusion.Maui.Core.Internals;

internal class SfAnimation : Animation
{
	private readonly IView localView;

	private bool isForwarding = false;

	private bool isReversing = false;

	internal double Start { get; set; }

	internal double End { get; set; }

	internal SfAnimation(IView view, Action<double>? step = null, Action? finished = null, double start = 0.0, double end = 1.0, Easing? easing = null, double duration = 1.0)
	{
		localView = view;
		base.Step = step;
		base.Finished = finished;
		Start = start;
		End = end;
		base.Easing = easing ?? Microsoft.Maui.Easing.Linear;
		base.Duration = duration;
	}

	internal void Forward()
	{
		animationManger ??= GetAnimationManager(localView.Handler.MauiContext);
		if (!isForwarding)
		{
			Pause();
			base.CurrentTime = ((base.Progress == 0.0) ? 0.0 : (base.Progress * base.Duration + base.StartDelay));
			base.HasFinished = false;
			isForwarding = true;
			isReversing = false;
			Resume();
		}
	}

	internal void Reverse()
	{
		animationManger ??= GetAnimationManager(localView.Handler.MauiContext);
		if (!isReversing)
		{
			Pause();
			base.CurrentTime = ((base.Progress == 1.0) ? 0.0 : ((1.0 - base.Progress) * base.Duration + base.StartDelay));
			base.HasFinished = false;
			isForwarding = false;
			isReversing = true;
			Resume();
		}
	}

	public override void Reset()
	{
		base.Reset();
		base.Progress = 0.0;
		isForwarding = false;
		isReversing = false;
	}

	protected override void OnTick(double millisecondsSinceLastUpdate)
	{
		if (base.HasFinished)
		{
			return;
		}
		double num = millisecondsSinceLastUpdate / 1000.0;
		base.CurrentTime += num;
		if (base.CurrentTime < base.StartDelay)
		{
			return;
		}
		double num2 = base.CurrentTime - base.StartDelay;
		double num3 = Math.Min(num2 / base.Duration, 1.0);
		double percent = (isForwarding ? num3 : (1.0 - num3));
		Update(percent);
		if (base.HasFinished)
		{
			base.Finished?.Invoke();
			isForwarding = false;
			isReversing = false;
			if (base.Repeats)
			{
				Reset();
			}
		}
	}

	public override void Update(double percent)
	{
		try
		{
			base.Progress = base.Easing.Ease(percent);
			base.Step?.Invoke(GetAnimatingValue());
			base.HasFinished = (isForwarding ? (percent == 1.0) : (percent == 0.0));
		}
		catch (Exception ex)
		{
			base.HasFinished = true;
			Console.WriteLine(ex.Message);
			throw new Exception(ex.Message);
		}
	}

	private IAnimationManager GetAnimationManager(IMauiContext mauiContext)
	{
		return mauiContext.Services.GetRequiredService<IAnimationManager>();
	}

	private double GetAnimatingValue()
	{
		return Start + (End - Start) * base.Progress;
	}
}
