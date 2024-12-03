using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core.Helper;

namespace Syncfusion.Maui.Core;

[DesignTimeVisible(true)]
public class SfAvatarView : SfView
{
	public static readonly BindableProperty InitialsColorProperty = BindableProperty.Create(nameof(InitialsColor), typeof(Color), typeof(SfAvatarView), Colors.Black, BindingMode.OneWay, null, OnInitialsColorPropertyChanged);

	public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(SfAvatarView), 18.0, BindingMode.OneWay, null, OnFontSizePropertyChanged);

	public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(SfAvatarView), null, BindingMode.OneWay, null, OnFontFamilyPropertyChanged);

	public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(SfAvatarView), null, BindingMode.OneWay, null, OnFontAttributesPropertyChanged);

	public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(SfAvatarView), null, BindingMode.OneWay, null, OnImageSourcePropertyChanged);

	public static readonly BindableProperty ContentTypeProperty = BindableProperty.Create(nameof(ContentType), typeof(ContentType), typeof(SfAvatarView), ContentType.Initials, BindingMode.OneWay, null, OnContentTypePropertyChanged);

	public static readonly BindableProperty InitialsTypeProperty = BindableProperty.Create(nameof(InitialsType), typeof(InitialsType), typeof(SfAvatarView), InitialsType.SingleCharacter, BindingMode.OneWay, null, OnInitialsTypePropertyChanged);

	public static readonly BindableProperty AvatarCharacterProperty = BindableProperty.Create(nameof(AvatarCharacter), typeof(AvatarCharacter), typeof(SfAvatarView), AvatarCharacter.Avatar1, BindingMode.OneWay, null, OnAvatarCharacterPropertyChanged);

	public new static readonly BindableProperty HeightRequestProperty = BindableProperty.Create(nameof(HeightRequest), typeof(double), typeof(SfAvatarView), 0.0, BindingMode.OneWay, null, OnHeightRequestPropertyChanged);

	public static readonly BindableProperty GroupSourceProperty = BindableProperty.Create(nameof(GroupSource), typeof(IEnumerable), typeof(SfAvatarView), null, BindingMode.OneWay, null, OnGroupSourcePropertyChanged);

	public static readonly BindableProperty BackgroundColorMemberPathProperty = BindableProperty.Create(nameof(BackgroundColorMemberPath), typeof(string), typeof(SfAvatarView), string.Empty, BindingMode.OneWay, null, OnBackgroundColorMemberPathPropertyChanged);

	public static readonly BindableProperty InitialsColorMemberPathProperty = BindableProperty.Create(nameof(InitialsColorMemberPath), typeof(string), typeof(SfAvatarView), string.Empty, BindingMode.OneWay, null, OnInitialsColorMemberPathPropertyChanged);

	public static readonly BindableProperty AvatarNameProperty = BindableProperty.Create(nameof(AvatarName), typeof(string), typeof(SfAvatarView), string.Empty, BindingMode.OneWay, null, OnNamePropertyChanged);

	public static readonly BindableProperty AvatarSizeProperty = BindableProperty.Create(nameof(AvatarSize), typeof(AvatarSize), typeof(SfAvatarView), null, BindingMode.OneWay, null, OnAvatarStylePropertyChanged);

	public new static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(SfAvatarView), null, BindingMode.OneWay, null, OnBackgroundColorPropertyChanged);

	public new static readonly BindableProperty BackgroundProperty = BindableProperty.Create(nameof(Background), typeof(Brush), typeof(SfAvatarView), null, BindingMode.OneWay, null, OnBackgroundPropertyChanged);

	public static readonly BindableProperty AvatarShapeProperty = BindableProperty.Create(nameof(AvatarShape), typeof(AvatarShape), typeof(SfAvatarView), AvatarShape.Custom, BindingMode.OneWay, null, OnAvatarStylePropertyChanged);

	public new static readonly BindableProperty WidthRequestProperty = BindableProperty.Create(nameof(WidthRequest), typeof(double), typeof(SfAvatarView), 0.0, BindingMode.OneWay, null, OnWidthRequestPropertyChanged);

	public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(SfAvatarView), new CornerRadius(0.0), BindingMode.OneWay, null, OnCornerRadiusPropertyChanged);

	public static readonly BindableProperty ImageSourceMemberPathProperty = BindableProperty.Create(nameof(ImageSourceMemberPath), typeof(string), typeof(SfAvatarView), string.Empty, BindingMode.OneWay, null, OnImageSourceMemberPathPropertyChanged);

	public static readonly BindableProperty AvatarColorModeProperty = BindableProperty.Create(nameof(AvatarColorMode), typeof(AvatarColorMode), typeof(SfAvatarView), AvatarColorMode.Default, BindingMode.OneWay, null, OnColorModePropertyChanged);

	public static readonly BindableProperty InitialsMemberPathProperty = BindableProperty.Create(nameof(InitialsMemberPath), typeof(string), typeof(SfAvatarView), string.Empty, BindingMode.OneWay, null, OnInitialsMemberPathPropertyChanged);

	public static readonly BindableProperty StrokeThicknessProperty = BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(SfAvatarView), 2.0, BindingMode.OneWay, null, OnStrokeThicknessPropertyChanged);

	public static readonly BindableProperty StrokeProperty = BindableProperty.Create(nameof(Stroke), typeof(Brush), typeof(SfAvatarView), new SolidColorBrush(Colors.Black), BindingMode.OneWay, null, OnStrokePropertyChanged);

	private bool isGroupSourceEmpty = true;

	private readonly string imageLocation = "Syncfusion.Maui.Core.AvatarView.VectorImages.";

	internal SfBorder? Border;

	internal Grid? LayoutGrid;

	public double StrokeThickness
	{
		get
		{
			return (double)GetValue(StrokeThicknessProperty);
		}
		set
		{
			SetValue(StrokeThicknessProperty, value);
		}
	}

	public Brush Stroke
	{
		get
		{
			return (Brush)GetValue(StrokeProperty);
		}
		set
		{
			SetValue(StrokeProperty, value);
		}
	}

	public AvatarCharacter AvatarCharacter
	{
		get
		{
			return (AvatarCharacter)GetValue(AvatarCharacterProperty);
		}
		set
		{
			SetValue(AvatarCharacterProperty, value);
		}
	}

	public FontAttributes FontAttributes
	{
		get
		{
			return (FontAttributes)GetValue(FontAttributesProperty);
		}
		set
		{
			SetValue(FontAttributesProperty, value);
		}
	}

	public string FontFamily
	{
		get
		{
			return (string)GetValue(FontFamilyProperty);
		}
		set
		{
			SetValue(FontFamilyProperty, value);
		}
	}

	public Color InitialsColor
	{
		get
		{
			return (Color)GetValue(InitialsColorProperty);
		}
		set
		{
			SetValue(InitialsColorProperty, value);
		}
	}

	public double FontSize
	{
		get
		{
			return (double)GetValue(FontSizeProperty);
		}
		set
		{
			SetValue(FontSizeProperty, value);
		}
	}

	public ImageSource ImageSource
	{
		get
		{
			return (ImageSource)GetValue(ImageSourceProperty);
		}
		set
		{
			SetValue(ImageSourceProperty, value);
		}
	}

	public ContentType ContentType
	{
		get
		{
			return (ContentType)GetValue(ContentTypeProperty);
		}
		set
		{
			SetValue(ContentTypeProperty, value);
		}
	}

	public string AvatarName
	{
		get
		{
			return (string)GetValue(AvatarNameProperty);
		}
		set
		{
			SetValue(AvatarNameProperty, value);
		}
	}

	public new double HeightRequest
	{
		get
		{
			return (double)GetValue(HeightRequestProperty);
		}
		set
		{
			SetValue(HeightRequestProperty, value);
		}
	}

	public InitialsType InitialsType
	{
		get
		{
			return (InitialsType)GetValue(InitialsTypeProperty);
		}
		set
		{
			SetValue(InitialsTypeProperty, value);
		}
	}

	public new Color BackgroundColor
	{
		get
		{
			return (Color)GetValue(BackgroundColorProperty);
		}
		set
		{
			SetValue(BackgroundColorProperty, value);
		}
	}

	public new Brush Background
	{
		get
		{
			return (Brush)GetValue(BackgroundProperty);
		}
		set
		{
			SetValue(BackgroundProperty, value);
		}
	}

	public IEnumerable GroupSource
	{
		get
		{
			return (IEnumerable)GetValue(GroupSourceProperty);
		}
		set
		{
			SetValue(GroupSourceProperty, value);
		}
	}

	public string BackgroundColorMemberPath
	{
		get
		{
			return (string)GetValue(BackgroundColorMemberPathProperty);
		}
		set
		{
			SetValue(BackgroundColorMemberPathProperty, value);
		}
	}

	public string ImageSourceMemberPath
	{
		get
		{
			return (string)GetValue(ImageSourceMemberPathProperty);
		}
		set
		{
			SetValue(ImageSourceMemberPathProperty, value);
		}
	}

	public CornerRadius CornerRadius
	{
		get
		{
			return (CornerRadius)GetValue(CornerRadiusProperty);
		}
		set
		{
			SetValue(CornerRadiusProperty, value);
		}
	}

	public AvatarColorMode AvatarColorMode
	{
		get
		{
			return (AvatarColorMode)GetValue(AvatarColorModeProperty);
		}
		set
		{
			SetValue(AvatarColorModeProperty, value);
		}
	}

	public string InitialsColorMemberPath
	{
		get
		{
			return (string)GetValue(InitialsColorMemberPathProperty);
		}
		set
		{
			SetValue(InitialsColorMemberPathProperty, value);
		}
	}

	public new double WidthRequest
	{
		get
		{
			return (double)GetValue(WidthRequestProperty);
		}
		set
		{
			SetValue(WidthRequestProperty, value);
		}
	}

	public string InitialsMemberPath
	{
		get
		{
			return (string)GetValue(InitialsMemberPathProperty);
		}
		set
		{
			SetValue(InitialsMemberPathProperty, value);
		}
	}

	public AvatarShape AvatarShape
	{
		get
		{
			return (AvatarShape)GetValue(AvatarShapeProperty);
		}
		set
		{
			SetValue(AvatarShapeProperty, value);
		}
	}

	public AvatarSize AvatarSize
	{
		get
		{
			return (AvatarSize)GetValue(AvatarSizeProperty);
		}
		set
		{
			SetValue(AvatarSizeProperty, value);
		}
	}

	internal Image? AvatarImage { get; set; }

	internal Image? CustomImage { get; set; }

	internal FontIconLabel InitialsLabel { get; set; } = new FontIconLabel();


	internal AvatarGroupView? GroupView { get; set; }

	internal string FontIconFontFamily { get; set; } = string.Empty;


	public SfAvatarView()
	{
		InitializeElements();
		DisplayCurrentAvatarElement();
		SetInitialsBasedOnInitialsType(AvatarName, InitialsLabel);
	}

	private static void OnAvatarCharacterPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.SetAvatarCharacter(sfAvatarView.AvatarCharacter);
		}
	}

	private static void OnInitialsColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.SetColors();
		}
	}

	private static void OnStrokePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView && sfAvatarView.Border != null)
		{
			sfAvatarView.Border.Stroke = sfAvatarView.Stroke;
		}
	}

	private static void OnStrokeThicknessPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView && sfAvatarView.Border != null)
		{
			sfAvatarView.Border.StrokeThickness = sfAvatarView.StrokeThickness;
		}
	}

	private static void OnFontSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.ApplyAvatarStyleSetting();
			sfAvatarView.GroupView?.SetInitialsFontAttributeValues(sfAvatarView.InitialsLabel);
		}
	}

	private static void OnFontFamilyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.InitialsLabel.FontFamily = sfAvatarView.FontFamily;
			sfAvatarView.GroupView?.SetInitialsFontFamily(sfAvatarView.FontFamily, sfAvatarView.FontIconFontFamily);
		}
	}

	private static void OnFontAttributesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.InitialsLabel.FontAttributes = sfAvatarView.FontAttributes;
			sfAvatarView.GroupView?.SetInitialsFontAttributeValues(sfAvatarView.InitialsLabel);
		}
	}

	private static void OnImageSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.InitializeCustomImage();
			if (sfAvatarView.CustomImage != null)
			{
				sfAvatarView.CustomImage.Source = sfAvatarView.ImageSource;
			}
			sfAvatarView.DisplayCurrentAvatarElement();
		}
	}

	private static void OnContentTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.DisplayCurrentAvatarElement();
		}
	}

	private static void OnInitialsTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.SetInitialsBasedOnInitialsType(sfAvatarView.AvatarName, sfAvatarView.InitialsLabel);
		}
	}

	private static void OnNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.SetInitialsBasedOnInitialsType(sfAvatarView.AvatarName, sfAvatarView.InitialsLabel);
			sfAvatarView.DisplayCurrentAvatarElement();
		}
	}

	private static void OnAvatarStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.ApplyAvatarStyleSetting();
		}
	}

	private static void OnHeightRequestPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.ApplyAvatarStyleSetting();
		}
	}

	private static void OnColorModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.SetColors();
		}
	}

	private static void OnBackgroundColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.SetColors();
		}
	}

	private static void OnBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.SetColors();
		}
	}

	private static void OnCornerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.ApplyAvatarStyleSetting();
		}
	}

	private static void OnWidthRequestPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.ApplyAvatarStyleSetting();
		}
	}

	private static void OnGroupSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.InitializeGroupView();
			sfAvatarView.HookCollectionChangedEvent();
			sfAvatarView.DisplayCurrentAvatarElement();
			sfAvatarView.UpdateGroupViewValues();
		}
	}

	private static void OnInitialsMemberPathPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.UpdateGroupViewValues();
		}
	}

	private static void OnImageSourceMemberPathPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.UpdateGroupViewValues();
		}
	}

	private static void OnBackgroundColorMemberPathPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.UpdateGroupViewValues();
		}
	}

	private static void OnInitialsColorMemberPathPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfAvatarView sfAvatarView)
		{
			sfAvatarView.UpdateGroupViewValues();
		}
	}

	private void InitializeElements()
	{
		InitialsLabel.HorizontalTextAlignment = TextAlignment.Center;
		InitialsLabel.VerticalTextAlignment = TextAlignment.Center;
		InitialsLabel.HorizontalOptions = LayoutOptions.Fill;
		InitialsLabel.VerticalOptions = LayoutOptions.Fill;
		InitialsLabel.FontSize = FontSize;
		InitialsLabel.FontAttributes = FontAttributes;
		InitialsLabel.FontFamily = FontFamily;
		InitialsLabel.TextColor = InitialsColor;
		LayoutGrid = new Grid
		{
			HorizontalOptions = LayoutOptions.Fill,
			VerticalOptions = LayoutOptions.Fill
		};
		LayoutGrid.Children.Add(InitialsLabel);
		Border = new SfBorder
		{
			Content = LayoutGrid,
			HorizontalOptions = LayoutOptions.Fill,
			VerticalOptions = LayoutOptions.Fill,
			Stroke = Stroke,
			StrokeThickness = StrokeThickness
		};
		Add(Border);
		base.Background = Background;
		base.BackgroundColor = BackgroundColor;
		base.HeightRequest = HeightRequest;
		base.WidthRequest = WidthRequest;
		UpdateSizeofParent(base.WidthRequest, base.HeightRequest);
	}

	private void InitializeGroupView()
	{
		if (GroupView == null)
		{
			GroupView = new AvatarGroupView();
			GroupView.SetInitialsFontAttributeValues(InitialsLabel);
			GroupView.SetInitialsFontFamily(FontFamily, FontIconFontFamily);
			GroupView.SetColors(AvatarColorMode);
			LayoutGrid?.Children.Insert(LayoutGrid.Children.IndexOf(InitialsLabel), GroupView);
			UpdateSizeofParent(base.WidthRequest, base.HeightRequest);
		}
	}

	private void InitializeCustomImage()
	{
		if (CustomImage == null)
		{
			CustomImage = new Image
			{
				Aspect = Aspect.AspectFill
			};
			LayoutGrid?.Children.Insert(LayoutGrid.Children.IndexOf(InitialsLabel), CustomImage);
			UpdateSizeofParent(base.WidthRequest, base.HeightRequest);
		}
	}

	private void InitializeAvatarImage()
	{
		if (AvatarImage == null)
		{
			AvatarImage = new Image
			{
				Aspect = Aspect.AspectFill
			};
			LayoutGrid?.Children.Insert(LayoutGrid.Children.IndexOf(InitialsLabel), AvatarImage);
			UpdateSizeofParent(base.WidthRequest, base.HeightRequest);
		}
	}

	private void UpdateSizeofParent(double width, double height)
	{
		if (width > 0.0 && height > 0.0)
		{
			double num = StrokeThickness * 2.0;
			InitialsLabel.WidthRequest = width - num;
			InitialsLabel.HeightRequest = height - num;
			if (GroupView != null)
			{
				GroupView.WidthRequest = width - num;
				GroupView.HeightRequest = height - num;
			}
			if (CustomImage != null)
			{
				CustomImage.WidthRequest = width - num;
				CustomImage.HeightRequest = height - num;
			}
			if (AvatarImage != null)
			{
				AvatarImage.WidthRequest = width - num;
				AvatarImage.HeightRequest = height - num;
			}
		}
	}

	private void DisplayCurrentAvatarElement()
	{
		InitialsLabel.AvatarContentType = ContentType.ToString();
		CollaspeAllAvatarView();
		if (ContentType == ContentType.Initials)
		{
			InitialsLabel.IsVisible = true;
			return;
		}
		if (ContentType == ContentType.Custom)
		{
			if (CustomImage != null)
			{
				CustomImage.IsVisible = true;
			}
			return;
		}
		if (ContentType == ContentType.AvatarCharacter)
		{
			SetAvatarCharacter(AvatarCharacter);
			return;
		}
		if (ContentType == ContentType.Group)
		{
			if (GroupView != null)
			{
				GroupView.IsVisible = true;
			}
			return;
		}
		SetAvatarCharacter(AvatarCharacter);
		Array values = Enum.GetValues(typeof(AvatarCharacter));
		if (GroupSource != null)
		{
			CollaspeAllAvatarView();
			if (GroupView != null)
			{
				GroupView.IsVisible = true;
			}
		}
		else if (ImageSource != null && !string.IsNullOrEmpty(ImageSource.ToString()))
		{
			CollaspeAllAvatarView();
			if (DeviceInfo.Platform == DevicePlatform.WinUI)
			{
				foreach (object item in values)
				{
					string text = "File: " + item?.ToString() + ".png";
					if (text.ToString(CultureInfo.InvariantCulture) == ImageSource.ToString() && CustomImage != null)
					{
						CustomImage.Source = Microsoft.Maui.Controls.ImageSource.FromResource((imageLocation + item?.ToString() + ".png").ToString(CultureInfo.InvariantCulture), typeof(SfAvatarView));
					}
				}
			}
			if (CustomImage != null)
			{
				CustomImage.IsVisible = true;
			}
		}
		else
		{
			CollaspeAllAvatarView();
			UpdateAvatarImageVisibility();
		}
	}

	private void CollaspeAllAvatarView()
	{
		if (GroupView != null)
		{
			GroupView.IsVisible = false;
		}
		if (CustomImage != null)
		{
			CustomImage.IsVisible = false;
		}
		if (AvatarImage != null)
		{
			AvatarImage.IsVisible = false;
		}
		InitialsLabel.IsVisible = false;
	}

	private void SetAvatarCharacter(AvatarCharacter avatarCharacterType)
	{
		InitializeAvatarImage();
		if (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.Android)
		{
			if (AvatarImage != null)
			{
				AvatarImage.Source = Microsoft.Maui.Controls.ImageSource.FromResource((imageLocation + avatarCharacterType.ToString() + ".png").ToString(CultureInfo.InvariantCulture), typeof(SfAvatarView));
			}
		}
		else if (DeviceInfo.Platform == DevicePlatform.WinUI && AvatarImage != null)
		{
			AvatarImage.Source = Microsoft.Maui.Controls.ImageSource.FromResource((imageLocation + avatarCharacterType.ToString() + ".png").ToString(CultureInfo.InvariantCulture), typeof(SfAvatarView));
		}
		UpdateAvatarImageVisibility();
	}

	private void UpdateAvatarImageVisibility()
	{
		if (ContentType == ContentType.Default && AvatarImage != null && InitialsLabel != null && !string.IsNullOrEmpty(AvatarName))
		{
			AvatarImage.IsVisible = false;
			InitialsLabel.IsVisible = true;
		}
		else if (AvatarImage != null)
		{
			AvatarImage.IsVisible = true;
		}
	}

	private void UpdateGroupViewValues()
	{
		if (GroupSource == null)
		{
			return;
		}
		isGroupSourceEmpty = true;
		int num = 0;
		foreach (object item in GroupSource)
		{
			isGroupSourceEmpty = false;
			if (GroupView != null)
			{
				switch (num)
				{
				case 0:
					SetGroupElementValue(GroupView.PrimaryInitialsLabel, GroupView.PrimaryGrid, GroupView.PrimaryImage, GetPropertyValue(InitialsMemberPath, item), GetPropertyValue(ImageSourceMemberPath, item), GetPropertyValue(BackgroundColorMemberPath, item), GetPropertyValue(InitialsColorMemberPath, item));
					break;
				case 1:
					GroupView.ArrageElementsSpacing(hasSecondaryElement: true);
					SetGroupElementValue(GroupView.SecondaryInitialsLabel, GroupView.SecondaryGrid, GroupView.SecondaryImage, GetPropertyValue(InitialsMemberPath, item), GetPropertyValue(ImageSourceMemberPath, item), GetPropertyValue(BackgroundColorMemberPath, item), GetPropertyValue(InitialsColorMemberPath, item));
					break;
				case 2:
					GroupView.ArrageElementsSpacing(hasSecondaryElement: true, hasTertiaryElement: true);
					SetGroupElementValue(GroupView.TertiaryInitialsLabel, GroupView.TertiaryGrid, GroupView.TertiaryImage, GetPropertyValue(InitialsMemberPath, item), GetPropertyValue(ImageSourceMemberPath, item), GetPropertyValue(BackgroundColorMemberPath, item), GetPropertyValue(InitialsColorMemberPath, item));
					break;
				}
			}
			if (++num > 2)
			{
				break;
			}
		}
		if (GroupView != null)
		{
			if (isGroupSourceEmpty)
			{
				GroupView.ArrageElementsSpacing();
				GroupView.PrimaryImage.Source = string.Empty;
			}
			else
			{
				GroupView.SetInitialsFontFamily(FontFamily, FontIconFontFamily);
			}
		}
	}

	private void SetGroupElementValue(Label groupLabel, Grid groupGrid, Image groupImage, object? initialsValue, object? imageValue, object? backgroundColorValue, object? textColorValue)
	{
		if (initialsValue != null)
		{
			groupImage.Source = string.Empty;
			SetInitialsBasedOnInitialsType(initialsValue.ToString(), groupLabel);
		}
		if (imageValue != null)
		{
			Array values = Enum.GetValues(typeof(AvatarCharacter));
			bool flag = true;
			foreach (object item in values)
			{
				if (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.Android)
				{
					string text = item.ToString() + ".png";
					if (text == imageValue.ToString())
					{
						groupLabel.Text = string.Empty;
						groupImage.Source = Microsoft.Maui.Controls.ImageSource.FromResource((imageLocation + item?.ToString() + ".png").ToString(CultureInfo.InvariantCulture), typeof(SfAvatarView));
						flag = false;
					}
				}
				if (DeviceInfo.Platform == DevicePlatform.WinUI)
				{
					string text2 = item.ToString() + ".png";
					if (text2 == imageValue.ToString())
					{
						groupLabel.Text = string.Empty;
						groupImage.Source = Microsoft.Maui.Controls.ImageSource.FromResource((imageLocation + item?.ToString() + ".png").ToString(CultureInfo.InvariantCulture), typeof(SfAvatarView));
						flag = false;
					}
				}
			}
			if (flag)
			{
				if (imageValue is ImageSource)
				{
					groupImage.Source = imageValue as ImageSource;
				}
				else
				{
					groupImage.Source = imageValue.ToString();
				}
			}
		}
		if (AvatarColorMode == AvatarColorMode.Default)
		{
			if (backgroundColorValue != null && backgroundColorValue is Color)
			{
				groupGrid.BackgroundColor = (Color)backgroundColorValue;
			}
			if (textColorValue != null && textColorValue is Color)
			{
				groupLabel.TextColor = (Color)textColorValue;
			}
		}
		groupLabel.FontFamily = FontFamily;
	}

	private object? GetPropertyValue(string propertyName, object item)
	{
		if (string.IsNullOrEmpty(propertyName) || item == null)
		{
			return null;
		}
		object result = null;
		PropertyInfo property = item.GetType().GetProperty(propertyName);
		if (property != null)
		{
			result = property.GetValue(item);
		}
		return result;
	}

	private void HookCollectionChangedEvent()
	{
		if (GroupSource is INotifyCollectionChanged notifyCollectionChanged)
		{
			notifyCollectionChanged.CollectionChanged -= SfAvatarView_CollectionChanged;
			notifyCollectionChanged.CollectionChanged += SfAvatarView_CollectionChanged;
		}
	}

	private void SetColors()
	{
		if (AvatarViewColorTable.AutomaticColors == null)
		{
			AvatarViewColorTable.GenerateAutomaticBackgroundColors();
		}
		if (AvatarColorMode != 0 && Border != null)
		{
			if (AvatarColorMode == AvatarColorMode.DarkBackground)
			{
				if (AvatarViewColorTable.AutomaticColors != null)
				{
					Border.BackgroundColor = AvatarViewColorTable.AutomaticColors[AvatarViewColorTable.CurrentBackgroundColorIndex].DarkColor;
				}
				InitialsLabel.TextColor = AvatarViewColorTable.InitialsLightColor;
			}
			else if (AvatarColorMode == AvatarColorMode.LightBackground)
			{
				if (AvatarViewColorTable.AutomaticColors != null)
				{
					Border.BackgroundColor = AvatarViewColorTable.AutomaticColors[AvatarViewColorTable.CurrentBackgroundColorIndex].LightColor;
				}
				InitialsLabel.TextColor = AvatarViewColorTable.InitialsDarkColor;
			}
			if (AvatarViewColorTable.AutomaticColors != null && ++AvatarViewColorTable.CurrentBackgroundColorIndex >= AvatarViewColorTable.AutomaticColors.Count)
			{
				AvatarViewColorTable.CurrentBackgroundColorIndex = 0;
			}
		}
		else if (Border != null)
		{
			if (Background != null)
			{
				Border.Background = Background;
			}
			else
			{
				Border.BackgroundColor = BackgroundColor;
			}
			InitialsLabel.TextColor = InitialsColor;
		}
		GroupView?.SetColors(AvatarColorMode);
	}

	private void ApplyAvatarStyleSetting()
	{
		if (AvatarShape == AvatarShape.Custom)
		{
			SetAvatarSizing(WidthRequest, HeightRequest, CornerRadius, FontSize);
		}
		else
		{
			ApplyConstantAvatarStyleSetting();
		}
	}

	private void ApplyConstantAvatarStyleSetting()
	{
		if (AvatarSize == AvatarSize.ExtraLarge && AvatarShape == AvatarShape.Circle)
		{
			SetAvatarSizing(AvatarViewSizeTable.ExtraLargeSize, AvatarViewSizeTable.ExtraLargeFontSize);
		}
		else if (AvatarSize == AvatarSize.Large && AvatarShape == AvatarShape.Circle)
		{
			SetAvatarSizing(AvatarViewSizeTable.LargeSize, AvatarViewSizeTable.LargeFontSize);
		}
		else if (AvatarSize == AvatarSize.Medium && AvatarShape == AvatarShape.Circle)
		{
			SetAvatarSizing(AvatarViewSizeTable.MediumSize, AvatarViewSizeTable.MediumFontSize);
		}
		else if (AvatarSize == AvatarSize.Small && AvatarShape == AvatarShape.Circle)
		{
			SetAvatarSizing(AvatarViewSizeTable.SmallSize, AvatarViewSizeTable.SmallFontSize);
		}
		else if (AvatarSize == AvatarSize.ExtraSmall && AvatarShape == AvatarShape.Circle)
		{
			SetAvatarSizing(AvatarViewSizeTable.ExtraSmallSize, AvatarViewSizeTable.ExtraSmallFontSize);
		}
		else if (AvatarSize == AvatarSize.ExtraLarge && AvatarShape == AvatarShape.Square)
		{
			SetAvatarSizing(AvatarViewSizeTable.ExtraLargeSize, AvatarViewSizeTable.ExtraLargeFontSize, isCircleType: false);
		}
		else if (AvatarSize == AvatarSize.Large && AvatarShape == AvatarShape.Square)
		{
			SetAvatarSizing(AvatarViewSizeTable.LargeSize, AvatarViewSizeTable.LargeFontSize, isCircleType: false);
		}
		else if (AvatarSize == AvatarSize.Medium && AvatarShape == AvatarShape.Square)
		{
			SetAvatarSizing(AvatarViewSizeTable.MediumSize, AvatarViewSizeTable.MediumFontSize, isCircleType: false);
		}
		else if (AvatarSize == AvatarSize.Small && AvatarShape == AvatarShape.Square)
		{
			SetAvatarSizing(AvatarViewSizeTable.SmallSize, AvatarViewSizeTable.SmallFontSize, isCircleType: false);
		}
		else if (AvatarSize == AvatarSize.ExtraSmall && AvatarShape == AvatarShape.Square)
		{
			SetAvatarSizing(AvatarViewSizeTable.ExtraSmallSize, AvatarViewSizeTable.ExtraSmallFontSize, isCircleType: false);
		}
	}

	private void SetAvatarSizing(double avatarSizeRequest, double initialFontSize, bool isCircleType = true)
	{
		CornerRadius avatarCornerRadius = ((!isCircleType) ? ((CornerRadius)AvatarViewSizeTable.SquareAvatarStyleCornerRadius) : ((CornerRadius)(avatarSizeRequest / 2.0)));
		SetAvatarSizing(avatarSizeRequest, avatarSizeRequest, avatarCornerRadius, initialFontSize);
	}

	private void SetAvatarSizing(double avatarWidthRequest, double avatarHeightRequest, CornerRadius avatarCornerRadius, double initialFontSize)
	{
		base.HeightRequest = avatarHeightRequest;
		base.WidthRequest = avatarWidthRequest;
		UpdateSizeofParent(avatarHeightRequest, avatarHeightRequest);
		if (Border != null)
		{
			Border.StrokeShape = new RoundRectangle
			{
				CornerRadius = avatarCornerRadius
			};
		}
		if (InitialsLabel != null)
		{
			InitialsLabel.FontSize = initialFontSize;
		}
		if (GroupView != null && InitialsLabel != null)
		{
			GroupView.SetInitialsFontAttributeValues(InitialsLabel);
		}
	}

	private void SetInitialsBasedOnInitialsType(string? initialsValue, Label elementLabel)
	{
		if (initialsValue == null || initialsValue.Length <= 0)
		{
			return;
		}
		elementLabel.FontFamily = FontFamily;
		if (InitialsType == InitialsType.SingleCharacter)
		{
			for (int i = 0; i < initialsValue.Length; i++)
			{
				char c = initialsValue[i];
				if (c.ToString(CultureInfo.InvariantCulture) != AvatarViewStaticText.SpaceText)
				{
					elementLabel.Text = c.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture);
					break;
				}
			}
		}
		else
		{
			elementLabel.Text = GetValidatedInitials(initialsValue, elementLabel).ToUpper(CultureInfo.InvariantCulture);
		}
	}

	private string GetValidatedInitials(string initialsValue, Label labelElement)
	{
		if (initialsValue.Contains(AvatarViewStaticText.SpaceText))
		{
			string[] array = initialsValue.Split(' ');
			if (array.Length > 1)
			{
				string text = string.Empty;
				string text2 = string.Empty;
				string[] array2 = array;
				foreach (string text3 in array2)
				{
					if (text3.Length > 0)
					{
						if (text.Length == 0)
						{
							text = text3[0].ToString(new NumberFormatInfo());
						}
						else if (text.Length == 1)
						{
							text += text3[0].ToString(new NumberFormatInfo());
						}
						else
						{
							text = text2[0].ToString(new NumberFormatInfo());
							text += text3[0].ToString(new NumberFormatInfo());
						}
						if (string.IsNullOrEmpty(text2))
						{
							text2 = text3;
						}
					}
				}
				if (text.Length == 2)
				{
					return text;
				}
				return GetSingleWordInitial(text2, labelElement);
			}
			if (array.Length == 1)
			{
				return GetSingleWordInitial(array[0], labelElement);
			}
		}
		return GetSingleWordInitial(initialsValue, labelElement);
	}

	private string GetSingleWordInitial(string word, Label labelElement)
	{
		if (word.Length <= 1)
		{
			return word + word;
		}
		return word[0].ToString(CultureInfo.InvariantCulture) + word[word.Length - 1].ToString(CultureInfo.InvariantCulture);
	}

	private void SfAvatarView_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		UpdateGroupViewValues();
	}
}
