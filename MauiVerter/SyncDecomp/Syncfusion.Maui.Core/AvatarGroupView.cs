using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;

namespace Syncfusion.Maui.Core;

internal class AvatarGroupView : Grid
{
	internal Image TertiaryImage { get; set; } = new Image();


	internal Grid TertiaryGrid { get; set; } = new Grid();


	internal Grid SecondaryLayoutGrid { get; set; } = new Grid();


	internal FontIconLabel PrimaryInitialsLabel { get; set; } = new FontIconLabel();


	internal Image PrimaryImage { get; set; } = new Image();


	internal Grid PrimaryGrid { get; set; } = new Grid();


	internal FontIconLabel SecondaryInitialsLabel { get; set; } = new FontIconLabel();


	internal Image SecondaryImage { get; set; } = new Image();


	internal Grid SecondaryGrid { get; set; } = new Grid();


	internal FontIconLabel TertiaryInitialsLabel { get; set; } = new FontIconLabel();


	internal AvatarGroupView()
	{
		PrimaryInitialsLabel.HorizontalTextAlignment = TextAlignment.Center;
		PrimaryInitialsLabel.VerticalTextAlignment = TextAlignment.Center;
		PrimaryInitialsLabel.HorizontalOptions = LayoutOptions.Center;
		PrimaryInitialsLabel.VerticalOptions = LayoutOptions.Center;
		SecondaryInitialsLabel.HorizontalTextAlignment = TextAlignment.Center;
		SecondaryInitialsLabel.VerticalTextAlignment = TextAlignment.Center;
		SecondaryInitialsLabel.HorizontalOptions = LayoutOptions.Center;
		SecondaryInitialsLabel.VerticalOptions = LayoutOptions.Center;
		TertiaryInitialsLabel.HorizontalTextAlignment = TextAlignment.Center;
		TertiaryInitialsLabel.VerticalTextAlignment = TextAlignment.Center;
		TertiaryInitialsLabel.HorizontalOptions = LayoutOptions.Center;
		TertiaryInitialsLabel.VerticalOptions = LayoutOptions.Center;
		PrimaryImage.Aspect = (SecondaryImage.Aspect = (TertiaryImage.Aspect = Aspect.AspectFill));
		PopulatePrimaryGrid();
		PopulateSecondaryGrid();
		base.RowSpacing = (SecondaryLayoutGrid.RowSpacing = (base.ColumnSpacing = (SecondaryLayoutGrid.ColumnSpacing = 1.0)));
	}

	internal void ArrageElementsSpacing(bool hasSecondaryElement = false, bool hasTertiaryElement = false)
	{
		base.ColumnDefinitions.Clear();
		SecondaryLayoutGrid.RowDefinitions.Clear();
		base.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		if (hasSecondaryElement)
		{
			base.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = new GridLength(1.0, GridUnitType.Star)
			});
		}
		else
		{
			base.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = new GridLength(0.0, GridUnitType.Absolute)
			});
		}
		SecondaryLayoutGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		if (hasTertiaryElement)
		{
			SecondaryLayoutGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = new GridLength(1.0, GridUnitType.Star)
			});
		}
		else
		{
			SecondaryLayoutGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = new GridLength(0.0, GridUnitType.Absolute)
			});
		}
	}

	internal void SetInitialsFontAttributeValues(Label referenceLabel)
	{
		FontIconLabel primaryInitialsLabel = PrimaryInitialsLabel;
		FontIconLabel secondaryInitialsLabel = SecondaryInitialsLabel;
		double num = (TertiaryInitialsLabel.FontSize = referenceLabel.FontSize);
		double fontSize2 = (secondaryInitialsLabel.FontSize = num);
		primaryInitialsLabel.FontSize = fontSize2;
		FontIconLabel primaryInitialsLabel2 = PrimaryInitialsLabel;
		FontIconLabel secondaryInitialsLabel2 = SecondaryInitialsLabel;
		FontAttributes fontAttributes2 = (TertiaryInitialsLabel.FontAttributes = referenceLabel.FontAttributes);
		FontAttributes fontAttributes4 = (secondaryInitialsLabel2.FontAttributes = fontAttributes2);
		primaryInitialsLabel2.FontAttributes = fontAttributes4;
	}

	internal void SetInitialsFontFamily(string actualFontFamily, string fontIconFontFamily)
	{
		if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
		{
			PrimaryInitialsLabel.FontFamily = actualFontFamily;
			SecondaryInitialsLabel.FontFamily = actualFontFamily;
			TertiaryInitialsLabel.FontFamily = actualFontFamily;
		}
		else
		{
			PrimaryInitialsLabel.FontFamily = actualFontFamily;
			SecondaryInitialsLabel.FontFamily = actualFontFamily;
			TertiaryInitialsLabel.FontFamily = actualFontFamily;
		}
	}

	internal void SetColors(AvatarColorMode colorMode = AvatarColorMode.Default)
	{
		switch (colorMode)
		{
		case AvatarColorMode.DarkBackground:
			if (AvatarViewColorTable.AutomaticColors != null)
			{
				PrimaryGrid.BackgroundColor = AvatarViewColorTable.AutomaticColors[0].DarkColor;
				SecondaryGrid.BackgroundColor = AvatarViewColorTable.AutomaticColors[1].DarkColor;
				TertiaryGrid.BackgroundColor = AvatarViewColorTable.AutomaticColors[2].DarkColor;
			}
			PrimaryInitialsLabel.TextColor = AvatarViewColorTable.InitialsLightColor;
			SecondaryInitialsLabel.TextColor = AvatarViewColorTable.InitialsLightColor;
			TertiaryInitialsLabel.TextColor = AvatarViewColorTable.InitialsLightColor;
			break;
		case AvatarColorMode.LightBackground:
			if (AvatarViewColorTable.AutomaticColors != null)
			{
				PrimaryGrid.BackgroundColor = AvatarViewColorTable.AutomaticColors[0].LightColor;
				SecondaryGrid.BackgroundColor = AvatarViewColorTable.AutomaticColors[1].LightColor;
				TertiaryGrid.BackgroundColor = AvatarViewColorTable.AutomaticColors[2].LightColor;
			}
			PrimaryInitialsLabel.TextColor = AvatarViewColorTable.InitialsDarkColor;
			SecondaryInitialsLabel.TextColor = AvatarViewColorTable.InitialsDarkColor;
			TertiaryInitialsLabel.TextColor = AvatarViewColorTable.InitialsDarkColor;
			break;
		}
	}

	private void PopulatePrimaryGrid()
	{
		ArrageElementsSpacing();
		base.Children.Add(PrimaryGrid);
		base.Children.Add(PrimaryInitialsLabel);
		base.Children.Add(PrimaryImage);
		base.Children.Add(SecondaryLayoutGrid);
		Grid.SetColumn((BindableObject)SecondaryLayoutGrid, 1);
	}

	private void PopulateSecondaryGrid()
	{
		ArrageElementsSpacing();
		SecondaryLayoutGrid.Children.Add(SecondaryGrid);
		SecondaryLayoutGrid.Children.Add(SecondaryInitialsLabel);
		SecondaryLayoutGrid.Children.Add(SecondaryImage);
		SecondaryLayoutGrid.Children.Add(TertiaryGrid);
		SecondaryLayoutGrid.Children.Add(TertiaryInitialsLabel);
		TertiaryGrid.Children.Add(TertiaryImage);
		Grid.SetRow((BindableObject)TertiaryGrid, 1);
		Grid.SetRow((BindableObject)TertiaryInitialsLabel, 1);
		Grid.SetRow((BindableObject)TertiaryImage, 1);
	}
}
