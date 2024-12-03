using System.ComponentModel;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core;

public class LegendItem : INotifyPropertyChanged, ILegendItem
{
	private int index;

	private object? item = null;

	private string text = string.Empty;

	private string fontFamily = string.Empty;

	private FontAttributes fontAttributes = FontAttributes.None;

	private Brush iconBrush = new SolidColorBrush(Colors.Transparent);

	private Color textColor = Colors.Black;

	private ShapeType iconType = ShapeType.Rectangle;

	private double iconHeight = 12.0;

	private double iconWidth = 12.0;

	private float fontSize = 12f;

	private Thickness textMargin = new Thickness(0.0);

	private bool isToggled = false;

	private bool isIconVisible = true;

	private Brush disableBrush = new SolidColorBrush(Color.FromRgba(0.0, 0.0, 0.0, 0.38));

	public Brush IconBrush
	{
		get
		{
			return iconBrush;
		}
		set
		{
			if (iconBrush != value)
			{
				iconBrush = value;
				OnPropertyChanged(nameof(IconBrush));
			}
		}
	}

	public double IconHeight
	{
		get
		{
			return iconHeight;
		}
		set
		{
			if (iconHeight != value)
			{
				iconHeight = value;
				OnPropertyChanged(nameof(IconHeight));
			}
		}
	}

	public double IconWidth
	{
		get
		{
			return iconWidth;
		}
		set
		{
			if (iconWidth != value)
			{
				iconWidth = value;
				OnPropertyChanged(nameof(IconWidth));
			}
		}
	}

	public int Index
	{
		get
		{
			return index;
		}
		internal set
		{
			if (index != value)
			{
				index = value;
				OnPropertyChanged(nameof(Index));
			}
		}
	}

	public bool IsIconVisible
	{
		get
		{
			return isIconVisible;
		}
		set
		{
			if (isIconVisible != value)
			{
				isIconVisible = value;
				OnPropertyChanged(nameof(IsIconVisible));
			}
		}
	}

	public bool IsToggled
	{
		get
		{
			return isToggled;
		}
		set
		{
			if (isToggled != value)
			{
				isToggled = value;
				OnPropertyChanged(nameof(IsToggled));
			}
		}
	}

	public object? Item
	{
		get
		{
			return item;
		}
		internal set
		{
			if (item != value)
			{
				item = value;
				OnPropertyChanged(nameof(Item));
			}
		}
	}

	public string Text
	{
		get
		{
			return text;
		}
		set
		{
			if (!(text == value))
			{
				text = value;
				OnPropertyChanged(nameof(Text));
			}
		}
	}

	public FontAttributes FontAttributes
	{
		get
		{
			return fontAttributes;
		}
		internal set
		{
			if (fontAttributes != value)
			{
				fontAttributes = value;
				OnPropertyChanged(nameof(FontAttributes));
			}
		}
	}

	public string FontFamily
	{
		get
		{
			return fontFamily;
		}
		internal set
		{
			if (!(fontFamily == value))
			{
				fontFamily = value;
				OnPropertyChanged(nameof(FontFamily));
			}
		}
	}

	public float FontSize
	{
		get
		{
			return fontSize;
		}
		internal set
		{
			if (fontSize != value)
			{
				fontSize = value;
				OnPropertyChanged(nameof(FontSize));
			}
		}
	}

	public ShapeType IconType
	{
		get
		{
			return iconType;
		}
		internal set
		{
			if (iconType != value)
			{
				iconType = value;
				OnPropertyChanged(nameof(IconType));
			}
		}
	}

	public Color TextColor
	{
		get
		{
			return textColor;
		}
		internal set
		{
			if (textColor != value)
			{
				textColor = value;
				OnPropertyChanged(nameof(TextColor));
			}
		}
	}

	public Thickness TextMargin
	{
		get
		{
			return textMargin;
		}
		internal set
		{
			if (!(textMargin == value))
			{
				textMargin = value;
				OnPropertyChanged(nameof(TextMargin));
			}
		}
	}

	internal Brush DisableBrush
	{
		get
		{
			return disableBrush;
		}
		set
		{
			if (disableBrush != value)
			{
				disableBrush = value;
				OnPropertyChanged(nameof(DisableBrush));
			}
		}
	}

	Brush ILegendItem.DisableBrush
	{
		get
		{
			return DisableBrush;
		}
		set
		{
			DisableBrush = value;
		}
	}

	FontAttributes ILegendItem.FontAttributes
	{
		get
		{
			return FontAttributes;
		}
		set
		{
			FontAttributes = value;
		}
	}

	string ILegendItem.FontFamily
	{
		get
		{
			return FontFamily;
		}
		set
		{
			FontFamily = value;
		}
	}

	float ILegendItem.FontSize
	{
		get
		{
			return FontSize;
		}
		set
		{
			FontSize = value;
		}
	}

	ShapeType ILegendItem.IconType
	{
		get
		{
			return IconType;
		}
		set
		{
			IconType = value;
		}
	}

	Color ILegendItem.TextColor
	{
		get
		{
			return TextColor;
		}
		set
		{
			TextColor = value;
		}
	}

	Thickness ILegendItem.TextMargin
	{
		get
		{
			return TextMargin;
		}
		set
		{
			TextMargin = value;
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
