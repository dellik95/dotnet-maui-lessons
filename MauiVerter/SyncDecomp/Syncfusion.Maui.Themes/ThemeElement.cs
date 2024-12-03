using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Themes;

internal static class ThemeElement
{
	public static BindableProperty CommonThemeProperty = BindableProperty.Create("CommonTheme", typeof(string), typeof(IThemeElement), "Default", BindingMode.OneWay, null, OnCommonThemePropertyChanged);

	public static BindableProperty ControlThemeProperty = BindableProperty.Create("ControlTheme", typeof(string), typeof(IThemeElement), "Default", BindingMode.OneWay, null, OnControlThemeChanged);

	public static BindableProperty PrimaryColorProperty = BindableProperty.Create("PrimaryColor", typeof(Color), typeof(IThemeElement), Color.FromRgb(0, 0, 0));

	public static BindableProperty PrimaryLightColorProperty = BindableProperty.Create("PrimaryLightColor", typeof(Color), typeof(IThemeElement), Color.FromRgb(0, 0, 0));

	public static BindableProperty PrimaryDarkColorProperty = BindableProperty.Create("PrimaryDarkColor", typeof(Color), typeof(IThemeElement), Color.FromRgb(0, 0, 0));

	public static BindableProperty PrimaryForegroundColorProperty = BindableProperty.Create("PrimaryForegroundColor", typeof(Color), typeof(IThemeElement), Color.FromRgb(0, 0, 0));

	public static BindableProperty PrimaryLightForegroundColorProperty = BindableProperty.Create("PrimaryLightForegroundColor", typeof(Color), typeof(IThemeElement), Color.FromRgb(0, 0, 0));

	public static BindableProperty PrimaryDarkForegroundColorProperty = BindableProperty.Create("PrimaryDarkForegroundColor", typeof(Color), typeof(IThemeElement), Color.FromRgb(0, 0, 0));

	public static BindableProperty SuccessColorProperty = BindableProperty.Create("SuccessColor", typeof(Color), typeof(IThemeElement), Color.FromRgb(0, 0, 0));

	public static BindableProperty ErrorColorProperty = BindableProperty.Create("ErrorColor", typeof(Color), typeof(IThemeElement), Color.FromRgb(0, 0, 0));

	public static BindableProperty WarningColorProperty = BindableProperty.Create("WarningColor", typeof(Color), typeof(IThemeElement), Color.FromRgb(0, 0, 0));

	public static BindableProperty InfoColorProperty = BindableProperty.Create("InfoColor", typeof(Color), typeof(IThemeElement), Color.FromRgb(0, 0, 0));

	private static readonly Dictionary<string, WeakReference<ResourceDictionary>> ControlThemeCache = new Dictionary<string, WeakReference<ResourceDictionary>>();

	private static readonly List<WeakReference<ResourceDictionary>> StyleTargetDictionaries = new List<WeakReference<ResourceDictionary>>();

	private static BindableProperty controlKeyProperty = BindableProperty.Create("ControlKey", typeof(string), typeof(IThemeElement), string.Empty);

	private static BindableProperty implicitStyleProperty = BindableProperty.Create("ImplicitStyle", typeof(Style), typeof(IThemeElement), null, BindingMode.OneWay, null, OnImplicitStyleChanged);

	private static Dictionary<ResourceDictionary, List<ResourceDictionary>> pendingDictionariesToMerge = new Dictionary<ResourceDictionary, List<ResourceDictionary>>();

	private static bool isScheduled = false;

	private static object[] elements = new object[1];

	internal static void InitializeThemeResources(Element element, string controlKey)
	{
		if (element == null)
		{
			throw new ArgumentNullException(nameof(element));
		}
		Type type = element.GetType();
		element.SetValue(controlKeyProperty, controlKey);
		element.SetDynamicResource(CommonThemeProperty, "SyncfusionTheme");
		element.SetDynamicResource(ControlThemeProperty, controlKey);
		element.SetDynamicResource(PrimaryColorProperty, "SyncPrimaryColor");
		element.SetDynamicResource(PrimaryDarkColorProperty, "SyncPrimaryDarkColor");
		element.SetDynamicResource(PrimaryLightColorProperty, "SyncPrimaryLightColor");
		element.SetDynamicResource(PrimaryForegroundColorProperty, "SyncPrimaryForegroundColor");
		element.SetDynamicResource(PrimaryDarkForegroundColorProperty, "SyncPrimaryDarkForegroundColor");
		element.SetDynamicResource(PrimaryLightForegroundColorProperty, "SyncPrimaryLightForegroundColor");
		element.SetDynamicResource(SuccessColorProperty, "SyncSuccessColor");
		element.SetDynamicResource(ErrorColorProperty, "SyncErrorColor");
		element.SetDynamicResource(WarningColorProperty, "SyncWarningColor");
		element.SetDynamicResource(InfoColorProperty, "SyncInfoColor");
		if (!(element is VisualElement))
		{
			string fullName = type.FullName;
			element.SetDynamicResource(implicitStyleProperty, fullName);
		}
	}

	internal static void AddStyleDictionary(ResourceDictionary resourceDictionary)
	{
		if (resourceDictionary == null)
		{
			throw new ArgumentNullException(nameof(resourceDictionary));
		}
		StyleTargetDictionaries.Add(new WeakReference<ResourceDictionary>(resourceDictionary));
	}

	private static void MergePendingDictionaries()
	{
		if (pendingDictionariesToMerge != null)
		{
			foreach (KeyValuePair<ResourceDictionary, List<ResourceDictionary>> item in pendingDictionariesToMerge)
			{
				ResourceDictionary key = item.Key;
				foreach (ResourceDictionary item2 in item.Value)
				{
					key.MergedDictionaries.Add(item2);
				}
				item.Value.Clear();
			}
			pendingDictionariesToMerge.Clear();
		}
		isScheduled = false;
	}

	private static void OnImplicitStyleChanged(BindableObject bindable, object oldValue, object newValue)
	{
		Style style = newValue as Style;
		if (!(bindable is Element element) || style == null || ApplyStyle(element, style))
		{
			return;
		}
		foreach (Setter setter in style.Setters)
		{
			if (setter.Value is DynamicResource dynamicResource)
			{
				element.SetDynamicResource(setter.Property, dynamicResource.Key);
			}
			else
			{
				element.SetValue(setter.Property, setter.Value);
			}
		}
	}

	private static bool ApplyStyle(Element element, Style style)
	{
		MethodInfo methodInfo = typeof(Style).GetInterface("IStyle")?.GetMethod("Apply");
		if (methodInfo != null)
		{
			elements[0] = element;
			methodInfo.Invoke(style, elements);
			return true;
		}
		return false;
	}

	private static void OnCommonThemePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is IThemeElement themeElement)
		{
			themeElement.OnCommonThemeChanged((string)oldValue, (string)newValue);
		}
	}

	private static void MergeThemeDictionary(string key, ResourceDictionary themeDictionary)
	{
		if (themeDictionary == null)
		{
			return;
		}
		int count = StyleTargetDictionaries.Count;
		for (int num = count - 1; num >= 0; num--)
		{
			WeakReference<ResourceDictionary> weakReference = StyleTargetDictionaries[num];
			weakReference.TryGetTarget(out var target);
			if (target != null && target.TryGetValue(key, out var _) && !target.MergedDictionaries.Contains(themeDictionary))
			{
				List<ResourceDictionary> list = null;
				if (!pendingDictionariesToMerge.ContainsKey(target))
				{
					list = new List<ResourceDictionary>();
					pendingDictionariesToMerge.Add(target, list);
				}
				else
				{
					list = pendingDictionariesToMerge[target];
				}
				list.Add(themeDictionary);
				if (!isScheduled && Application.Current != null)
				{
					Application.Current.Dispatcher.Dispatch(MergePendingDictionaries);
					isScheduled = true;
				}
			}
		}
	}

	private static bool TryGetThemeDictionary(VisualElement element, out ResourceDictionary? resourceDictionary)
	{
		resourceDictionary = null;
		if (element != null)
		{
			string key = (string)element.GetValue(controlKeyProperty);
			WeakReference<ResourceDictionary> value = null;
			if (ControlThemeCache.TryGetValue(key, out value) && value.TryGetTarget(out resourceDictionary))
			{
				return true;
			}
			if (ControlThemeCache.ContainsKey(key))
			{
				ControlThemeCache.Remove(key);
			}
			if (element is IParentThemeElement parentThemeElement)
			{
				resourceDictionary = parentThemeElement.GetThemeDictionary();
				if (resourceDictionary != null)
				{
					value = new WeakReference<ResourceDictionary>(resourceDictionary);
					ControlThemeCache.Add(key, value);
					return true;
				}
			}
		}
		return false;
	}

	private static void OnControlThemeChanged(BindableObject bindable, object oldValue, object newValue)
	{
		IThemeElement themeElement = bindable as IThemeElement;
		if (bindable == null)
		{
			return;
		}
		string key = (string)bindable.GetValue(controlKeyProperty);
		if (bindable is VisualElement element)
		{
			List<WeakReference<ResourceDictionary>> styleTargetDictionaries = StyleTargetDictionaries;
			if (styleTargetDictionaries != null && styleTargetDictionaries.Count > 0 && TryGetThemeDictionary(element, out ResourceDictionary resourceDictionary) && resourceDictionary != null)
			{
				MergeThemeDictionary(key, resourceDictionary);
			}
		}
		themeElement?.OnControlThemeChanged((string)oldValue, (string)newValue);
	}
}
