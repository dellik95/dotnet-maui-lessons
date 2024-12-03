using System;
using System.Collections.Generic;

namespace Syncfusion.Maui.Core;

internal static class FlaggedEnumExt
{
	internal static IEnumerable<Enum> GetAllItems(this Enum targetEnum)
	{
		foreach (Enum value in Enum.GetValues(targetEnum.GetType()))
		{
			if (targetEnum.HasFlag(value))
			{
				yield return value;
			}
		}
	}

	internal static bool IsEmpty(this SfEffects source)
	{
		foreach (SfEffects allItem in source.GetAllItems())
		{
			if (allItem != SfEffects.None && allItem != SfEffects.Selection)
			{
				return false;
			}
		}
		return true;
	}

	internal static SfEffects Add(this SfEffects target, SfEffects newItem)
	{
		return target |= newItem;
	}

	internal static SfEffects ComplementsOf(this SfEffects target, SfEffects source)
	{
		return target &= ~source;
	}

	internal static SfEffects ComplementsOf(this SfEffects target, SfEffects source1, SfEffects source2)
	{
		return target &= ~(source1 | source2);
	}
}
