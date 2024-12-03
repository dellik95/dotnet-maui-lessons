using System;
using System.Collections.Specialized;

namespace Syncfusion.Maui.Graphics.Internals;

internal static class NotifyCollectionChangedEventArgsExtensions
{
	public static void ApplyCollectionChanges(this NotifyCollectionChangedEventArgs self, Action<object, int, bool> insertAction, Action<object, int> removeAction, Action resetAction)
	{
		switch (self.Action)
		{
		default:
			return;
		case NotifyCollectionChangedAction.Add:
		{
			if (self.NewStartingIndex < 0)
			{
				break;
			}
			for (int m = 0; m < self.NewItems.Count; m++)
			{
				insertAction(self.NewItems[m], m + self.NewStartingIndex, arg3: true);
			}
			return;
		}
		case NotifyCollectionChangedAction.Move:
		{
			if (self.NewStartingIndex < 0 || self.OldStartingIndex < 0)
			{
				break;
			}
			for (int k = 0; k < self.OldItems.Count; k++)
			{
				removeAction(self.OldItems[k], self.OldStartingIndex);
			}
			int num = self.NewStartingIndex;
			if (self.OldStartingIndex < self.NewStartingIndex)
			{
				num -= self.OldItems.Count - 1;
			}
			for (int l = 0; l < self.OldItems.Count; l++)
			{
				insertAction(self.OldItems[l], num + l, arg3: false);
			}
			return;
		}
		case NotifyCollectionChangedAction.Remove:
		{
			if (self.OldStartingIndex < 0)
			{
				break;
			}
			for (int j = 0; j < self.OldItems.Count; j++)
			{
				removeAction(self.OldItems[j], self.OldStartingIndex);
			}
			return;
		}
		case NotifyCollectionChangedAction.Replace:
		{
			if (self.OldStartingIndex < 0 || self.OldItems.Count != self.NewItems.Count)
			{
				break;
			}
			for (int i = 0; i < self.OldItems.Count; i++)
			{
				removeAction(self.OldItems[i], i + self.OldStartingIndex);
				insertAction(self.NewItems[i], i + self.OldStartingIndex, arg3: true);
			}
			return;
		}
		case NotifyCollectionChangedAction.Reset:
			break;
		}
		resetAction();
	}
}
